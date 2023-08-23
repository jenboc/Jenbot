using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;

namespace Jenbot.Commands;

public class Games : ICommand
{
    private const string ITCH_URL = "https://jenboc.itch.io";
    
    public string Name => "games";

    public async Task Execute(SocketSlashCommand command) => await command.RespondAsync(ITCH_URL);

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Get a link to the creator's itch.io page");
}