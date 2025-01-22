using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Jenbot.Interactions;

///<summary>
/// Static class designed to deal with non-slash-command interactions
///</summary>
public static class InteractionHandler
{
    private const float OBJECT_EXPIRY_TIME_MINS = 5f;
    private static Dictionary<Guid, InteractionObjectRecord> _registeredObjects;

    static InteractionHandler()
    {
        _registeredObjects = new();
    }

    ///<summary>
    /// Register an Interaction Object with the InteractionHandler
    ///</summary>
    public static void Register(InteractionObject obj)
    {
        var record = new InteractionObjectRecord() 
        {
            Object = obj,
            CreationTime = DateTime.UtcNow
        };

        _registeredObjects.TryAdd(obj.GetGuid(), record);
    }

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

        await obj.Object.HandleInteraction(e.Interaction); 
    }

    ///<summary>
    /// Remove Interaction Objects that aren't being used anymore 
    /// from the dictionary
    ///</summary>
    private static void RemoveUnusedObjects()
    {
        foreach (var record in _registeredObjects.Values)
        {
            if (ShouldRemoveRecord(record))
                _registeredObjects.Remove(record.Object.GetGuid());
        }
    }

    ///<summary>
    /// Check if we should remove this record
    ///</summary>
    private static bool ShouldRemoveRecord(InteractionObjectRecord record)
    {
        var currentTime = DateTime.UtcNow;
        var timeSinceCreation = currentTime - record.CreationTime;

        return timeSinceCreation.TotalMinutes >= OBJECT_EXPIRY_TIME_MINS;
    }

    ///<summary>
    /// A struct to be stored in the dictionary of registered Interaction Objects
    /// Stores the object itself and the time of registration
    ///</summary>
    internal struct InteractionObjectRecord
    {
        public InteractionObject Object { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
