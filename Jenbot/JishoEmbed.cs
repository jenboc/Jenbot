using Discord;
using JishoNET.Models;
using Color = SixLabors.ImageSharp.Color;

namespace Jenbot;

public class JishoEmbed
{
    public static Embed BuildKanjiEmbed(JishoKanjiDefinition def)
    {
        var embed = new EmbedBuilder()
            .WithTitle(def.Kanji)
            .WithColor(Discord.Color.Green);

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
    
    public static Embed BuildDefinitionEmbed(JishoDefinition def)
    {
        var embed = new EmbedBuilder()
            .WithTitle(def.Slug)
            .WithColor(Discord.Color.Green);

        var readings = string.Join("、", def.Japanese.Select(j => j.Reading));
        embed.WithDescription(readings);

        foreach (var sense in def.Senses)
        {
            embed.AddField(
                new EmbedFieldBuilder().WithName(sense.EnglishDefinitions.First())
                    .WithValue(string.Join(", ", sense.PartsOfSpeech)).WithIsInline(false));
        }
        
        return embed.Build();
    }
}