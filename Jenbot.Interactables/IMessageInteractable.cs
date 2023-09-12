using Discord.WebSocket;

namespace Jenbot.Interactables;

public interface IMessageInteractable
{
    public Task Reply(SocketInteraction interaction);
    public Task Send(ISocketMessageChannel channel);
    public Task Followup(SocketInteraction interaction);
}