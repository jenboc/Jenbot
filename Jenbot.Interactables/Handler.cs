using Discord;
using Discord.WebSocket;

namespace Jenbot.Interactables;

public abstract class Handler
{
    protected Handler()
    {
        TimeCreated = DateTime.Now;
        MsgComponent = new ComponentBuilder().Build();
    }

    public DateTime TimeCreated { get; }
    protected MessageComponent MsgComponent { get; set; }

    public bool ContainsComponent(SocketMessageComponent toCheck)
    {
        return MsgComponent.Components.Any(row =>
            row.Components.Any(component => component.CustomId == toCheck.Data.CustomId));
    }

    public abstract Task HandleInteraction(SocketMessageComponent component);
}