using System.Reflection;
using Discord;
using Discord.WebSocket;

namespace Jenbot.Interactions;

public static class InteractionManager
{
    private static readonly List<ICommand> _commands;
    private static readonly List<Handler> _handlers;

    private static readonly TimeSpan _handlerTimeToLive;

    static InteractionManager()
    {
        _commands = LoadCommands().ToList();
        _handlers = new List<Handler>();
        _handlerTimeToLive = TimeSpan.FromMinutes(5);
    }

    public static void AddHandler(Handler handler)
    {
        _handlers.Add(handler);
    }

    /// <summary>
    ///     Use reflection to make an instance of every class which implements ICommand
    /// </summary>
    /// <returns>IEnumerable containing every implementation of ICommand</returns>
    private static IEnumerable<ICommand> LoadCommands()
    {
        return from t in Assembly.GetEntryAssembly()?.GetTypes()
            where t.GetInterfaces().Contains(typeof(ICommand))
                  && t.GetConstructor(Type.EmptyTypes) != null
            select Activator.CreateInstance(t) as ICommand;
    }

    /// <summary>
    ///     Build all the slash commands so they can be overwritten on launch
    /// </summary>
    /// <returns>An IEnumerable containing all the data for every command</returns>
    public static IEnumerable<ApplicationCommandProperties> PrepCommandsForOverwrite()
    {
        return _commands.Select(command => command.GetCommandBuilder().Build());
    }

    /// <summary>
    ///     Handle slash command interactions
    /// </summary>
    public static async Task HandleSlashCommand(SocketSlashCommand slashCommand)
    {
        Console.WriteLine($"Received /{slashCommand.Data.Name}");
        
        foreach (var command in _commands)
        {
            if (command.Name != slashCommand.Data.Name)
                continue;

            await command.Execute(slashCommand);
            return;
        }

        await slashCommand.RespondAsync($"{slashCommand.User.Mention} {slashCommand.Data.Name} does not exist!");
    }

    /// <summary>
    ///     Handle component interactions of any type, and clear any interactions which are out of date
    /// </summary>
    /// <param name="component">The message component which was interacted with</param>
    public static async Task HandleComponents(SocketMessageComponent component)
    {
        var toRemove = new List<int>();

        for (var i = 0; i < _handlers.Count; i++)
        {
            var handler = _handlers[i];
            var difference = DateTime.Now - handler.TimeCreated;

            if (difference >= _handlerTimeToLive)
                toRemove.Add(i);

            if (handler.ContainsComponent(component))
            {
                await handler.HandleInteraction(component);
                return;
            }
        }

        RemoveHandlers(toRemove);
    }

    /// <summary>
    ///     Remove handlers at provided indexes
    /// </summary>
    /// <param name="indexes">The indexes of the handlers to remove</param>
    private static void RemoveHandlers(IEnumerable<int> indexes)
    {
        foreach (var index in indexes)
            _handlers.RemoveAt(index);
    }
}