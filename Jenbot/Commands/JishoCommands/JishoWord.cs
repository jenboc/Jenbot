using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;
using JishoNET;
using JishoNET.Models;
using Color = Discord.Color;

namespace Jenbot.Commands;

public class JishoWord : ICommand
{
    public string Name => "jisho-word";
    private readonly JishoClient _client = new();

    public async Task Execute(SocketSlashCommand command)
    {
        var word = (string)command.Data.Options.First().Value;

        var result = await _client.GetDefinitionAsync(word);

        if (result == null || !result.Success || result.Data.Length == 0)
        {
            await command.RespondAsync($"{command.User.Mention}, {word} was not found. Please check that you " +
                                       $"have spelt the word correctly and then try again.");
            return;
        }

        if (result.Data.Length == 1)
        {
            var embed = BuildDefinitionEmbed(result.Data[0]);
            await command.RespondAsync(embed: embed);
            return;
        }
        
        var multipageDict = new Dictionary<string, Embed>();

        foreach (var def in result.Data)
        {
            var key = def.Slug;
            var embed = BuildDefinitionEmbed(def);
            multipageDict.Add(key, embed);
        }
        
        var multipage = new MultipageEmbed(multipageDict, multipageDict.Keys.First());
        InteractionManager.AddHandler(multipage);
        await multipage.Reply(command);
    }

    private static Embed BuildDefinitionEmbed(JishoDefinition def)
    {
        var embed = new EmbedBuilder()
            .WithTitle(def.Slug)
            .WithColor(Color.Green);

        var readings = string.Join("、", def.Japanese.Select(j => j.Reading));
        embed.WithDescription(readings);

        var englishDefs = def.Senses.Select(s => new EnglishDefinition(s));
        foreach (var e in englishDefs)
        {
            embed.AddField(
                new EmbedFieldBuilder().WithName(e.Definition).WithValue(e.PartsOfSpeech).WithIsInline(false));
        }
        
        return embed.Build();
    }

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Search jisho.org for a word")
        .AddOption(
            new SlashCommandOptionBuilder()
                .WithName("word")
                .WithDescription("The word to search for")
                .WithType(ApplicationCommandOptionType.String)
                .WithRequired(true));

    private struct EnglishDefinition
    {
        public string Definition { get; private set; }
        public string PartsOfSpeech { get; private set; }

        public EnglishDefinition(JishoEnglishSense sense)
        {
            Definition = sense.EnglishDefinitions.First();
            PartsOfSpeech = string.Join(", ", sense.PartsOfSpeech);
        }
    }
}
