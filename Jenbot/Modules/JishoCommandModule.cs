using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Jenbot.Interactables;
using JishoNET;
using JishoNET.Models;

namespace Jenbot.Modules;

public class JishoCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly JishoClient _jishoClient = new(); 
    
    [SlashCommand("jisho-word", "Search jisho.org for a word")]
    public async Task JishoWord(string word)
    {
        await DeferAsync();
        
        var result = await _jishoClient.GetDefinitionAsync(word);

        if (result == null || !result.Success || result.Data.Length == 0)
        {
            await FollowupAsync($"{Context.User.Mention}, {word} was not found. Please check that you " +
                                        $"have spelt the word correctly and then try again.");
            return;
        }

        if (result.Data.Length == 1)
        {
            var embed = JishoEmbed.BuildDefinitionEmbed(result.Data[0]);
            await FollowupAsync(embed: embed);
            return;
        }
        
        var multipageDict = new Dictionary<string, Embed>();

        foreach (var def in result.Data)
        {
            var key = def.Slug;
            var embed = JishoEmbed.BuildDefinitionEmbed(def);
            multipageDict.Add(key, embed);
        }
        
        var multipage = new MultipageEmbed(multipageDict, multipageDict.Keys.First());
        InteractableManager.AddHandler(multipage);
        await multipage.Followup(Context.Interaction);
    }
    
    [SlashCommand("jisho-kanji", "Search jisho.org for a kanji character")]
    public async Task Execute(string kanji)
    {
        await DeferAsync();
        
        if (kanji.Length > 1)
        {
            await FollowupAsync($"{Context.User.Mention}, please only search for individual characters. " +
                                        $"Please use the jisho-word command to search for words.");
            return;
        }
        
        var result = await _jishoClient.GetKanjiDefinitionAsync(kanji);
        
        if (result == null || !result.Success)
        {
            await FollowupAsync($"{Context.User.Mention}, {kanji} was not found. Please check that you " +
                                        $"have entered the kanji character you want to search for, not one of " +
                                        $"its readings");
            return;
        }

        var embed = JishoEmbed.BuildKanjiEmbed(result.Data);
        await FollowupAsync(embed: embed);
    }
}