using Discord;
using Discord.WebSocket;

namespace Jenbot.Commands;

public class Ping : ICommand
{
    public string Name { get; }

    public Ping()
    {
        Name = "ping";
    }
    
    public async Task Execute(SocketSlashCommand command)
    {
        await command.RespondAsync("pong!");
    }

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Ping the bot to check that it is running");
}