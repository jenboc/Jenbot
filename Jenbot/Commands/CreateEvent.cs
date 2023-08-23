using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;

namespace Jenbot.Commands;

public class CreateEvent : ICommand
{
    public string Name => "create-event";

    public async Task Execute(SocketSlashCommand command)
    {
        var options = new CommandOptions(command);

        var eventType = ParseEventType(options.Channel, options.PhysicalLocation, options.EndDate, options.EndTime);

        if (eventType == GuildScheduledEventType.External && options.EndTime != null)
            options.EndDate ??= options.StartDate;

        var startDate = ParseDateTime(options.StartDate, options.StartTime);
        var endDate = ParseDateTime(options.EndDate, options.EndTime);

        if (startDate == null ||
            ((endDate == null || endDate < startDate) && eventType == GuildScheduledEventType.External))
        {
            await command.RespondAsync($"{command.User.Mention}, please ensure that dates and times are entered " +
                                       $"in the correct format; and that if it is a physical event, you provide a end" +
                                       $" date and time");
            return;
        }

        var guild = (command.Channel as SocketGuildChannel)?.Guild;

        try
        {
            await guild.CreateEventAsync(options.EventName, ParseDateTime(options.StartDate, options.StartTime).Value,
                eventType, description: options.Description, endTime: ParseDateTime(options.EndDate, options.EndTime),
                channelId: options.Channel?.Id, location: options.PhysicalLocation);

            await command.RespondAsync("Event created successfully");
        }
        catch
        {
            await command.RespondAsync("There was a problem trying to create that event");
        }
    }

    public SlashCommandBuilder GetCommandBuilder()
    {
        var name = new SlashCommandOptionBuilder().WithName("event-name")
            .WithDescription("Name for the event")
            .WithRequired(true)
            .WithType(ApplicationCommandOptionType.String);

        var startDate = new SlashCommandOptionBuilder().WithName("date")
            .WithDescription("Date of the event (DD/MM/YYYY)")
            .WithRequired(false)
            .WithType(ApplicationCommandOptionType.String);

        var startTime = new SlashCommandOptionBuilder().WithName("time")
            .WithDescription("Start time of the event (24hr format)")
            .WithRequired(true)
            .WithType(ApplicationCommandOptionType.String);

        var description = new SlashCommandOptionBuilder().WithName("description")
            .WithDescription("Description of the event")
            .WithRequired(false)
            .WithType(ApplicationCommandOptionType.String);

        var channel = new SlashCommandOptionBuilder().WithName("channel")
            .WithDescription("Channel where the event will take place (if applicable)")
            .WithRequired(false)
            .WithType(ApplicationCommandOptionType.Channel);

        var physicalLocation = new SlashCommandOptionBuilder().WithName("physical-location")
            .WithDescription("Physical location where the event will take place (if applicable)")
            .WithRequired(false)
            .WithType(ApplicationCommandOptionType.String);

        var endDate = new SlashCommandOptionBuilder().WithName("end-date")
            .WithDescription("End date of the event (DD/MM/YYYY)")
            .WithRequired(false)
            .WithType(ApplicationCommandOptionType.String);

        var endTime = new SlashCommandOptionBuilder().WithName("end-time")
            .WithDescription("End time of the event (24hr format)")
            .WithRequired(false)
            .WithType(ApplicationCommandOptionType.String);

        // TODO: Add cover image

        return new SlashCommandBuilder().WithName(Name)
            .WithDescription("Create a new guild event")
            .AddOption(name)
            .AddOption(startDate)
            .AddOption(startTime)
            .AddOption(description)
            .AddOption(channel)
            .AddOption(physicalLocation)
            .AddOption(endDate)
            .AddOption(endTime);
    }

    private DateTimeOffset? ParseDateTime(string? date, string? time)
    {
        date ??= DateTime.Now.ToString("dd/MM/yyyy");

        var splitTime = time?.Split(":").Select(int.Parse).ToArray();
        var splitDate = date?.Split("/").Select(int.Parse).ToArray();

        if (splitTime == null || splitDate == null || splitTime.Length != 2 || splitDate.Length != 3)
            return null;

        var dateTime = new DateTime(splitDate[2], splitDate[1], splitDate[0], splitTime[0],
            splitTime[1], 0);

        if (dateTime < DateTime.Now)
            return null;

        return new DateTimeOffset(dateTime);
    }

    private GuildScheduledEventType ParseEventType(IGuildChannel? channel, string? physicalLocation,
        string? endDate, string? endTime)
    {
        // Voice if voice channel provided
        // External required physical location and end datetime 
        // Otherwise, None event type

        if (channel is IVoiceChannel)
            return GuildScheduledEventType.Voice;

        if (physicalLocation != null && endTime != null)
            return GuildScheduledEventType.External;

        return GuildScheduledEventType.None;
    }

    private bool DateIsValid(string dateInput)
    {
        var split = dateInput.Split("/");
        var currentYear = DateTime.Now.Year;

        if (split.Length != 3)
            return false;

        try
        {
            var datetime = new DateTime(int.Parse(split[2]), int.Parse(split[1]), int.Parse(split[0]));
            return datetime < DateTime.Now;
        }
        catch
        {
            return false;
        }
    }

    private bool TimeIsValid(string timeInput)
    {
        var split = timeInput.Split(":");

        try
        {
            return split.Length == 2 && int.Parse(split[0]) >= 0 && int.Parse(split[0]) < 24
                   && int.Parse(split[1]) >= 0 && int.Parse(split[1]) < 60;
        }
        catch
        {
            return false;
        }
    }

    private struct CommandOptions
    {
        public string EventName { get; }
        public string? StartDate { get; }
        public string StartTime { get; }
        public string? EndDate { get; set; }
        public string? EndTime { get; }
        public string? Description { get; }
        public IGuildChannel? Channel { get; }
        public string? PhysicalLocation { get; }

        public CommandOptions(SocketSlashCommand command)
        {
            foreach (var option in command.Data.Options)
                switch (option.Name)
                {
                    case "event-name":
                        EventName = (string)option.Value;
                        break;
                    case "date":
                        StartDate = (string)option.Value;
                        break;
                    case "time":
                        StartTime = (string)option.Value;
                        break;
                    case "description":
                        Description = (string)option.Value;
                        break;
                    case "channel":
                        Channel = (IGuildChannel)option.Value;
                        break;
                    case "physical-location":
                        PhysicalLocation = (string)option.Value;
                        break;
                    case "end-date":
                        EndDate = (string)option.Value;
                        break;
                    case "end-time":
                        EndTime = (string)option.Value;
                        break;
                }
        }
    }
}