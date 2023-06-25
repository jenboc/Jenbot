using Discord;
using Discord.WebSocket;

namespace Jenbot.Interactions;

public abstract class Handler
{
    public DateTime TimeCreated { get; }
    protected string CustomId { get; }
    protected MessageComponent MsgComponent { get; set; }

    protected Handler()
    {
        TimeCreated = DateTime.Now; 
        CustomId = Guid.NewGuid().ToString();
        MsgComponent = new ComponentBuilder().Build();
    }

    public bool ContainsComponent(SocketMessageComponent toCheck) => MsgComponent.Components.Any(row =>
        row.Components.Any(component => component.CustomId == toCheck.Data.CustomId));

    public abstract Task HandleInteraction(SocketMessageComponent component);
}