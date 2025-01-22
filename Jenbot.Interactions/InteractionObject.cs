using DSharpPlus.Entities;

namespace Jenbot.Interactions;

///<summary>
/// Abstract Class representing object that can be interacted with
/// e.g. Buttons, etc.
///</summary>
public abstract class InteractionObject : IEquatable<InteractionObject>
{
    private Guid _guid;
    
    protected InteractionObject()
        => _guid = Guid.NewGuid();

    public Guid GetGuid() => _guid;
    public abstract Task HandleInteraction(DiscordInteraction interaction);

    // Equality Overrides
    public static bool operator ==(InteractionObject? a, InteractionObject? b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.GetGuid() == b.GetGuid();
    }

    public static bool operator !=(InteractionObject? a, InteractionObject? b)
        => !(a == b);

    public bool Equals(InteractionObject? other)
        => this == other;

    public override bool Equals(object? other)
        => Equals(other as InteractionObject);

    // Hash Code Override
    public override abstract int GetHashCode();
}
