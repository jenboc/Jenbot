using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Jenbot.Interactions;

///<summary>
/// Static class designed to deal with non-slash-command interactions
///</summary>
public static class InteractionHandler
{
    private static Dictionary<Guid, InteractionObject> _registeredObjects;

    static InteractionHandler()
    {
        _registeredObjects = new();
    }

    ///<summary>
    /// Register an Interaction Object with the InteractionHandler
    ///</summary>
    public static void Register(InteractionObject obj)
        => _registeredObjects.TryAdd(obj.GetGuid(), obj);

    ///<summary>
    /// Unregister an Interaction Object from the InteractionHandler
    ///</summary>
    public static void Unregister(InteractionObject obj)
    {
        if (_registeredObjects.ContainsKey(obj.GetGuid()))
            _registeredObjects.Remove(obj.GetGuid());
    }

    ///<summary>
    /// Handle DiscordClient.ComponentInteractionCreated event
    ///</summary>
    public static async Task OnComponentInteractionCreated(DiscordClient client, ComponentInteractionCreateEventArgs e)
    {
        var guid = new Guid(e.Id);
        
        if (!_registeredObjects.TryGetValue(guid, out var obj))
            return;

        await obj.HandleInteraction(e.Interaction); 
    }
}
