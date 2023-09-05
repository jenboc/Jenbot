using Discord;
using Discord.WebSocket;

namespace Jenbot.Interactions;

public interface ICommand
{
    // Name used to identify the command 
    public string Name { get; }

    // What the command should do when executed
    public Task Execute(SocketSlashCommand command);

    // Creates the builder so that it can be updated on launch
    public SlashCommandBuilder GetCommandBuilder();
}