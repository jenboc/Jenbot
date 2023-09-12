using Discord;
using Discord.Interactions;

namespace Jenbot.Modules;

public class ManagementCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    [RequireOwner]
    [SlashCommand("change-status", "Change the bot's discord status")]
    public async Task ChangeStatus(string newStatus)
    {
        await DeferAsync(ephemeral: true);
        await Context.Client.SetCustomStatusAsync(newStatus);
        await FollowupAsync("Status changed", ephemeral: true);
    }
    
    [SlashCommand("create-event", "Create an event in the server")]
    public async Task CreateEvent(string eventName, string description, [ComplexParameter] Time start, 
        string physicalLocation = "", 
        [ChannelTypes(ChannelType.Voice)] IGuildChannel? channel = null)
    {
        await DeferAsync(); 
        var eventType = GuildScheduledEventType.None;

        if (channel is IVoiceChannel)
            eventType = GuildScheduledEventType.Voice;
        else if (!string.IsNullOrEmpty(physicalLocation))
            eventType = GuildScheduledEventType.External;

        var guild = Context.Guild;

        try
        {
            await guild.CreateEventAsync(eventName, new DateTimeOffset(start.AsDateTime()), eventType,
                description: description, channelId: channel?.Id, endTime: null,
                location: string.IsNullOrEmpty(physicalLocation) ? null : physicalLocation);
            await FollowupAsync("Your event was created successfully");
        }
        catch (Exception e)
        {
            await FollowupAsync("There was a problem trying to create your event\nPlease ensure you " +
                                "have provided either a voice channel, or name of a physical location");

            if (channel != null && !string.IsNullOrEmpty(physicalLocation))
            {
                Console.WriteLine(e);
                Console.WriteLine($"Name: {eventName}\nDescription: {description}\n" +
                                  $"Start: {start.Day}/{start.Month}/{start.Year} @ {start.Hour}:{start.Minute}\n" +
                                  $"Channel: {channel?.Name}\nPhysical Location: {physicalLocation}");
            }
        }
    }

    public class Time
    {
        public int Day { get; }
        public int Month { get; }
        public int Year { get; }
        public int Hour { get; }
        public int Minute { get; }

        [ComplexParameterCtor]
        public Time(int day, int month, int year, int hour, int minute)
        {
            Day = day;
            Month = month;
            Year = year;
            Hour = hour;
            Minute = minute;
        }

        public DateTime AsDateTime() => new DateTime(Year, Month, Day, Hour, Minute, 0);
    }
}