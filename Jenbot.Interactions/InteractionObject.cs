using DSharpPlus.Entities;

namespace Jenbot.Interactions;

public abstract class InteractionObject
{
    private Guid _guid;
    
    protected InteractionObject()
        => Guid.NewGuid();

    public Guid GetGuid() => _guid;
    public abstract void HandleInteraction(DiscordInteraction interaction);
}
