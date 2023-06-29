using Discord.WebSocket;

namespace Jenbot.Interactions;

public interface IMessageInteractable
{
    public Task Reply(SocketInteraction interaction);
    public Task Send(ISocketMessageChannel channel);
}