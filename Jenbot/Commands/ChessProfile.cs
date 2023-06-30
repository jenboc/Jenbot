using Discord;
using Discord.WebSocket;
using Jenbot.Chess;

namespace Jenbot.Commands;

public class ChessProfile : ICommand
{
    public string Name { get; }

    private ChessApi _api; 

    public ChessProfile()
    {
        Name = "chess-profile";
        _api = new ChessApi();
    }
    
    public async Task Execute(SocketSlashCommand command)
    {
        var name = (string)command.Data.Options.First();
        var playerData = await _api.GetPlayerAsync(name);

        try
        {
            var profile = playerData.Profile;
            var username = profile.Username;
            var stats = playerData.Stats;
            var rapidStats = stats.RapidChess;
            var bestGame = rapidStats.BestGame;
            var url = bestGame.GameUrl;
            
            await command.RespondAsync(
                $"{username}: {url}");
        }
        catch (Exception e)
        {
            await command.RespondAsync($"There was a problem processing your request, {command.User.Mention}");
            Console.WriteLine(e);
        }
    }

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Lookup someone's user data on chess.com")
        .AddOption(new SlashCommandOptionBuilder()
            .WithName("username")
            .WithDescription("Their chess.com username")
            .WithRequired(true)
            .WithType(ApplicationCommandOptionType.String)
        );
}