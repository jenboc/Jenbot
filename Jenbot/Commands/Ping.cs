using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;

namespace Jenbot.Commands;

public class Ping : ICommand
{
    public string Name => "ping";

    public async Task Execute(SocketSlashCommand command)
    {
        await command.RespondAsync("pong!");
    }

    public SlashCommandBuilder GetCommandBuilder()
    {
        return new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Ping the bot to check that it is running");
    }
}