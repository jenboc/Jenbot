namespace Jenbot.Interactions;

public static class EventHandlingUtils
{
    public static async Task InvokeEventAsync(InteractionEventHandler? handler,
            object sender, InteractionEventArgs e)
    {
        if (handler == null)
        {
            return;
        }

        // Get a list of all the subscribed functions and init array for their tasks.
        Delegate[] invocationList = handler.GetInvocationList();
        Task[] tasks = new Task[invocationList.Length];

        // Execute all subscribed functions
        for (var i = 0; i < invocationList.Length; i++)
            tasks[i] = ((InteractionEventHandler)invocationList[i])(sender, e);

        // Wait for all subscribed functions to finish
        await Task.WhenAll(tasks);
    }
}
