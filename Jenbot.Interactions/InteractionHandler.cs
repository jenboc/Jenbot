using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Jenbot.Interactions;

public static class InteractionHandler
{
    private static Dictionary<Guid, InteractionObject> _registeredObjects;

    static InteractionHandler()
    {
        _registeredObjects = new();
    }

    public static void Register(InteractionObject obj)
        => _registeredObjects.Add(obj.GetGuid(), obj);

    /// <summary>
    /// Handle DiscordClient.ComponentInteractionCreated event
    /// </summary>
    public static async Task OnComponentInteractionCreated(DiscordClient client, ComponentInteractionCreateEventArgs e)
    {
        var guid = new Guid(e.Id);
        
        if (!_registeredObjects.TryGetValue(guid, out var obj))
            return;

        obj.HandleInteraction(e.Interaction); 
    }
}
