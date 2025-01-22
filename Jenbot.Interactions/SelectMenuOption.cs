using DSharpPlus.Entities;

namespace Jenbot.Interactions;

public class SelectMenuOption : InteractionObject
{
    public event InteractionEventHandler? OnOptionSelected;

    private DiscordSelectComponentOption _discordOption;

    public string Label => _discordOption.Label;
    public string Description => _discordOption.Description;

    public SelectMenuOption(string label, string description="") : base()
    {
        _discordOption = new(label, GetGuid().ToString(), description);
    }

    public void SetParentMenu(SelectMenu selectMenu)
    {
        // Only adhere to this if the menu actually contains the option
        if (!selectMenu.HasOption(this))
            return;

        selectMenu.OnOptionChanged += SelectMenu_OnOptionChanged;
    }

    private async Task SelectMenu_OnOptionChanged(object sender, InteractionEventArgs e)
    {
        // This will not be null by design 
        var menu = sender as SelectMenu;

        // Invoke OnOptionSelected if it was selected
        if (this != menu!.GetSelectedOption())
            return;

        await EventHandlingUtils.InvokeEventAsync(OnOptionSelected, this, e);
    }

    public DiscordSelectComponentOption GetComponent()
        => _discordOption;

    // Doesn't need to handle any interactions since it will be handled by the menu it belongs to
    public override Task HandleInteraction(DiscordInteraction interaction)
    { 
        return Task.Run(() => {});
    }

    public override int GetHashCode()
    {
        var hashCode = GetGuid().GetHashCode();
        hashCode = (hashCode * 397) ^ Label.GetHashCode();
        hashCode = (hashCode * 397) ^ Label.GetHashCode();

        return hashCode;
    }
}
