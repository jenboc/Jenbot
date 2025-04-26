using DSharpPlus.Entities;

namespace Jenbot.Interactions;

public class InteractionEventArgs : EventArgs
{
    public DiscordInteraction Interaction { get; private set; }

    public InteractionEventArgs(DiscordInteraction interaction)
        => Interaction = interaction;
}
