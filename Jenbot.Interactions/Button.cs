using DSharpPlus;
using DSharpPlus.Entities;

namespace Jenbot.Interactions;

public class Button : InteractionObject
{
    public event InteractionEventHandler? OnButtonClick;

    private DiscordButtonComponent _discordButton;

    public Button(string label, Colour colour=Colour.Blurple, bool enabled=true) : base()
    {
        _discordButton = new(ColourToStyle(colour), GetGuid().ToString(), label, !enabled);
        
        // Only register if enabled
        if (enabled)
            InteractionHandler.Register(this);
    }

    public async override Task HandleInteraction(DiscordInteraction interaction)
    {
        // Only possible event is a click
        var args = new InteractionEventArgs(interaction);
        await EventHandlingUtils.InvokeEventAsync(OnButtonClick, this, args);
    }

    // Expose fields on the DiscordButtonComponent object
    public string Label => _discordButton.Label;
    public bool IsEnabled => _discordButton.Disabled;

    // Enable and Disable the button
    public void Enable()
    {
        _discordButton.Enable();

        // Make sure it's registered with the InteractionHandler
        InteractionHandler.Register(this);
    }

    public void Disable()
    {
        _discordButton.Disable();

        // We don't need to respond to events now either
        InteractionHandler.Unregister(this);
    }

    // Expose component field so it can actually be used.
    public DiscordButtonComponent GetComponent() => _discordButton;

    // Define a new enum colour as a less confusing replacement for ButtonStyle
    public enum Colour
    {
        Blurple,
        Grey,
        Green,
        Red
    }

    private ButtonStyle ColourToStyle(Colour c) => c switch
    {
        Colour.Blurple => ButtonStyle.Primary,
        Colour.Grey => ButtonStyle.Secondary,
        Colour.Green => ButtonStyle.Success,
        Colour.Red => ButtonStyle.Danger,
        _ => ButtonStyle.Primary
    };

    public override int GetHashCode()
    {
        // Hash Code only includes things that are fixed
        var hashCode = GetGuid().GetHashCode();
        hashCode = (hashCode * 397) ^ Label.GetHashCode();
        hashCode = (hashCode * 397) ^ _discordButton.Style.GetHashCode();
        hashCode = (hashCode * 397) ^ _discordButton.Emoji.GetHashCode();

        return hashCode;
    }
}
