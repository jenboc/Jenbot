using DSharpPlus.Entities;

namespace Jenbot.Interactions;

public class SelectMenu : InteractionObject
{ 
    public event InteractionEventHandler? OnOptionChanged;

    private DiscordSelectComponent _discordSelectMenu;
    private Dictionary<Guid, SelectMenuOption> _options;
    private Guid? _selectedGuid;

    public SelectMenu(IEnumerable<SelectMenuOption> options, bool enabled) : base()
    {
        var optionComponents = options.Select(x => x.GetComponent());
        _discordSelectMenu = new(GetGuid().ToString(), null, optionComponents, !enabled);

        // Create options dictionary
        _options = new();
        foreach (var option in options)
            _options.Add(option.GetGuid(), option);

        if (enabled)
            InteractionHandler.Register(this);
    }

    public async override Task HandleInteraction(DiscordInteraction interaction)
    {
        // Interaction values always contains Values array of length 1
        // This element is the GUID of the option clicked
        var selectedOptionGuid = new Guid(interaction.Data.Values[0]);
        
        if (!_options.TryGetValue(selectedOptionGuid, out var selected))
        {
            Console.WriteLine("[ERROR] Select Menu does not contain this option");
            return;
        }

        _selectedGuid = selectedOptionGuid;
        
        var eventArgs = new InteractionEventArgs(interaction);
        await EventHandlingUtils.InvokeEventAsync(OnOptionChanged, this, eventArgs);
    }

    public void Enable()
    {
        _discordSelectMenu.Enable();
        InteractionHandler.Register(this);
    }
    
    public void Disable()
    {
        _discordSelectMenu.Disable();
        InteractionHandler.Unregister(this);
    }

    public bool HasOption(SelectMenuOption option)
        => _options.ContainsKey(option.GetGuid());

    public DiscordSelectComponent GetComponent() => _discordSelectMenu;

    public SelectMenuOption? GetSelectedOption()
    {
        if (_selectedGuid == null)
            return null;

        return _options[_selectedGuid.Value];
    }

    public override int GetHashCode()
    {
        var hashCode = GetGuid().GetHashCode();

        foreach (var option in _options)
            hashCode = (hashCode * 397) ^ option.GetHashCode();

        return hashCode;
    }
}
