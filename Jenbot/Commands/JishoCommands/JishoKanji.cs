using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;
using JishoNET;
using JishoNET.Models;
using Color = Discord.Color;

namespace Jenbot.Commands;

public class JishoKanji : ICommand
{
    public string Name => "jisho-kanji";
    private readonly JishoClient _client = new();
    
    public async Task Execute(SocketSlashCommand command)
    {
        var kanji = (string)command.Data.Options.First().Value;

        if (kanji.Length > 1)
        {
            await command.FollowupAsync($"{command.User.Mention}, please only search for individual characters. " +
                                       $"Please use the jisho-word command to search for words.");
            return;
        }
        
        var result = await _client.GetKanjiDefinitionAsync(kanji);
        
        if (result == null || !result.Success)
        {
            await command.FollowupAsync($"{command.User.Mention}, {kanji} was not found. Please check that you " +
                                       $"have entered the kanji character you want to search for, not one of " +
                                       $"its readings");
            return;
        }

        var embed = BuildDefinitionEmbed(result.Data);
        await command.FollowupAsync(embed: embed);
    }

    private static Embed BuildDefinitionEmbed(JishoKanjiDefinition def)
    {
        var embed = new EmbedBuilder()
            .WithTitle(def.Kanji)
            .WithColor(Color.Green);

        if (def.Jlpt != null)
        {
            var jlpt = new EmbedFieldBuilder().WithName("JLPT Level").WithValue(def.Jlpt).WithIsInline(true);
            embed.AddField(jlpt);
        }

        var strokes = new EmbedFieldBuilder().WithName("Strokes").WithValue(def.Strokes).WithIsInline(true);
        var meanings = new EmbedFieldBuilder().WithName("Meanings").WithValue(string.Join(", ", def.Meanings))
            .WithIsInline(false);
        var kunyomi = new EmbedFieldBuilder().WithName("Kunyomi").WithValue(string.Join("、", def.KunyomiReadings))
            .WithIsInline(false);
        var onyomi = new EmbedFieldBuilder().WithName("Onyomi").WithValue(string.Join("、", def.OnyomiReadings))
            .WithIsInline(false);

        embed.AddField(strokes);
        embed.AddField(kunyomi);
        embed.AddField(onyomi);
        embed.AddField(meanings);

        return embed.Build();
    }

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Search jisho.org for a kanji character")
        .AddOption(
            new SlashCommandOptionBuilder()
                .WithName("kanji")
                .WithDescription("The Kanji to search for")
                .WithType(ApplicationCommandOptionType.String)
                .WithRequired(true));
}