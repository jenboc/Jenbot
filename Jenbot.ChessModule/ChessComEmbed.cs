using DSharpPlus.Entities;
using Jenbot.ChessModule.Api;

namespace Jenbot.ChessModule;

public class ChessComEmbedBuilder
{
    private DiscordEmbedBuilder _builder;

    public ChessComEmbedBuilder(Player playerData)
    {
        var r = new Random();
        var randomColour = new DiscordColor(r.NextSingle(), r.NextSingle(), r.NextSingle());
        _builder = new DiscordEmbedBuilder()
            .WithTitle($"{playerData.Profile.Title} {playerData.Profile.Username}")
            .WithUrl(playerData.Profile.Url)
            .WithThumbnail(playerData.Profile.AvatarUrl)
            .WithColor(randomColour);

        if (playerData.Profile.FideRating > 0)
            _builder.AddField("FIDE Rating:", playerData.Profile.FideRating.ToString(), true);

        _builder.AddField("Last Online:", GetStringDate(playerData.Profile.LastOnlineTimestamp), true);
        _builder.AddField("Registraton Date:", GetStringDate(playerData.Profile.RegistrationTimestamp), true);

        if (playerData.Profile.League != null)
            _builder.AddField("League:", playerData.Profile.League, true);

        AddGameTypeFields(playerData.Stats.RapidChess, "Rapid");
        AddGameTypeFields(playerData.Stats.BlitzChess, "Blitz");
        AddGameTypeFields(playerData.Stats.BulletChess, "Bullet");
        AddGameTypeFields(playerData.Stats.DailyChess, "Daily");
        AddGameTypeFields(playerData.Stats.Daily960Chess, "Daily Chess960");
    }

    public DiscordEmbed Build() => _builder.Build();
    
    // Add fields for a specific game type 
    private void AddGameTypeFields(GameType? typeData, string typeName)
    {
        if (typeData == null)
            return;

        _builder.AddField($"{typeName} Rating:", typeData.LastGame?.Rating.ToString());

        if (typeData.BestGame == null)
            return;

        _builder.AddField($"Best {typeName} Game:", typeData.BestGame.GameUrl); 
    }
    
    // Get the string date from a timestamp 
    private string GetStringDate(int timestamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp);

        return dateTime.ToString("dd/MM/yy"); 
    }
}