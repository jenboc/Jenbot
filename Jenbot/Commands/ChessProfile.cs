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
            await command.RespondAsync(
                $"{playerData.Profile.Username}: {playerData.Stats.RapidChess.BestGame.GameUrl}");
        }
        catch
        {
            await command.RespondAsync($"There was a problem processing your request, {command.User.Mention}");
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