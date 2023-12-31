﻿using Discord;
using Jenbot.Chess;
using Color = Discord.Color;

namespace Jenbot;

public class ChessComEmbed
{
    public static Embed Build(Player player)
    {
        var r = new Random();
        var builder = new EmbedBuilder()
            .WithTitle($"{player.Profile.Title} {player.Profile.Username}")
            .WithUrl(player.Profile.Url)
            .WithThumbnailUrl(player.Profile.AvatarUrl)
            .WithColor(new Color(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256)));

        if (player.Profile.FideRating > 0)
            builder.AddField("FIDE Rating:", player.Profile.FideRating, true);

        builder.AddField("Last Online:", GetStringDate(player.Profile.LastOnlineTimestamp), true);
        builder.AddField("Registration Date:", GetStringDate(player.Profile.RegistrationTimestamp), true);
        
        if (player.Profile.League != null)
            builder.AddField("League:", player.Profile.League, true);

        AddGameTypeFields(player.Stats.RapidChess, "Rapid", builder);
        AddGameTypeFields(player.Stats.BlitzChess, "Blitz", builder);
        AddGameTypeFields(player.Stats.BulletChess, "Bullet", builder);
        AddGameTypeFields(player.Stats.DailyChess, "Daily", builder);
        AddGameTypeFields(player.Stats.Daily960Chess, "Daily Chess960", builder);

        return builder.Build();
    }
    
    private static void AddGameTypeFields(GameType? typeData, string typeName, EmbedBuilder builder)
    {
        if (typeData != null)
        {
            builder.AddField($"{typeName} Rating:", typeData.LastGame.Rating);
            if (typeData.BestGame != null)
                builder.AddField($"Best {typeName} Game:", typeData.BestGame.GameUrl);
        }
    }
    
    private static string GetStringDate(int timestamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp);

        return dateTime.ToString("dd/MM/yy");
    }
}