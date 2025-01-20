using DSharpPlus.Entities;

namespace Jenbot.Interactions;

///<summary>
/// Abstract Class representing object that can be interacted with
/// e.g. Buttons, etc.
///</summary>
public abstract class InteractionObject
{
    private Guid _guid;
    
    protected InteractionObject()
        => Guid.NewGuid();

    public Guid GetGuid() => _guid;
    public abstract Task HandleInteraction(DiscordInteraction interaction);
}
