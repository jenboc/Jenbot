using System.Reflection;
using Discord;
using Discord.WebSocket;

namespace Jenbot.Interactables;

public static class InteractableManager
{
    private static readonly List<Handler> _handlers;

    private static readonly TimeSpan _handlerTimeToLive;

    static InteractableManager()
    {
        _handlers = new List<Handler>();
        _handlerTimeToLive = TimeSpan.FromMinutes(5);
    }

    public static void AddHandler(Handler handler)
    {
        _handlers.Add(handler);
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