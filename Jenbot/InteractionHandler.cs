using System.Reflection;
using Discord;
using Discord.WebSocket;
using Jenbot.Commands;

namespace Jenbot;

public class InteractionHandler
{
    private List<ICommand> _commands;

    public InteractionHandler()
    {
        _commands = LoadCommands().ToList();
    }

    /// <summary>
    /// Use reflection to make an instance of every class which implements ICommand
    /// </summary>
    /// <returns>IEnumerable containing every implementation of ICommand</returns>
    private IEnumerable<ICommand> LoadCommands() => from t in Assembly.GetExecutingAssembly().GetTypes()
            where t.GetInterfaces().Contains(typeof(ICommand))
                  && t.GetConstructor(Type.EmptyTypes) != null
            select Activator.CreateInstance(t) as ICommand;
    
    /// <summary>
    /// Build all the slash commands so they can be overwritten on launch
    /// </summary>
    /// <returns>An IEnumerable containing all the data for every command</returns>
    public IEnumerable<ApplicationCommandProperties> PrepCommandsForOverwrite() =>
        _commands.Select(command => command.GetCommandBuilder().Build());

    /// <summary>
    /// Handle slash command interactions
    /// </summary>
    public async Task HandleSlashCommand(SocketSlashCommand slashCommand)
    {
        foreach (var command in _commands)
        {
            if (command.Name != slashCommand.Data.Name)
                continue;

            await command.Execute(slashCommand);
            return;
        }

        await slashCommand.RespondAsync( $"{slashCommand.User.Mention} {slashCommand.Data.Name} does not exist!");
    }
}