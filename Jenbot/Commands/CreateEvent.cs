using System.Collections.Immutable;
using Discord;
using Discord.WebSocket;

namespace Jenbot.Commands;

public class CreateEvent : ICommand
{
    public string Name { get; }

    public CreateEvent()
    {
        Name = "create-event";
    }
    
    public async Task Execute(SocketSlashCommand command)
    {
        var options = new CommandOptions(command);

        if (!DateIsValid(options.StartDate))
        {
            await command.RespondAsync($"{command.User.Mention}, you did not enter a valid date");
            return;
        }

        if (!TimeIsValid(options.StartTime))
        {
            await command.RespondAsync($"{command.User.Mention}, you did not mention a valid time");
            return;
        }

        var channel = command.Channel as SocketGuildChannel;
        var guild = channel?.Guild;

        if (guild != null)
            await guild.CreateEventAsync(options.EventName, ParseDateTime(options.StartDate, options.StartTime),
                ParseEventType(options.Channel, options.PhysicalLocation), description: options.Description,
                channelId: options.Channel?.Id, location: options.PhysicalLocation);
    }

    // TODO: ParseDateTime
    private DateTimeOffset ParseDateTime(string startDate, string startTime)
    {
        var time = startTime.Split(":").Select(int.Parse).ToArray();
        var date = startDate.Split("/").Select(int.Parse).ToArray();
        var dateTime = new DateTime(date[2], date[1], date[0], time[0], time[1], 0);

        return new DateTimeOffset(dateTime);
    }
    
    // TODO: ParseEventType
    private GuildScheduledEventType ParseEventType(IGuildChannel? channel, string? physicalLocation)
    {
        return channel switch
        {
            null when physicalLocation == null => GuildScheduledEventType.None,
            IVoiceChannel => GuildScheduledEventType.Voice,
            _ => GuildScheduledEventType.External
        };
    }
    
    private bool DateIsValid(string dateInput)
    {
        var split = dateInput.Split("/");
        var currentYear = DateTime.Now.Year; 
        
        if (split.Length != 3)
            return false;

        if (int.Parse(split[2]) < currentYear || int.Parse(split[1]) > 12 || int.Parse(split[1]) < 1)
            return false;

        return int.Parse(split[0]) >= 1 &&
               int.Parse(split[0]) <= DateTime.DaysInMonth(int.Parse(split[2]), int.Parse(split[1]));
    }

    private bool TimeIsValid(string timeInput)
    {
        var split = timeInput.Split(":");

        return split.Length == 2 && (int.Parse(split[0]) >= 0 && int.Parse(split[0]) < 24)
                                 && (int.Parse(split[1]) >= 0 && int.Parse(split[1]) < 60);
    }

    public SlashCommandBuilder GetCommandBuilder()
    {
        var name = new SlashCommandOptionBuilder().WithName("event-name")
            .WithDescription("Name for the event")
            .WithRequired(true)
            .WithType(ApplicationCommandOptionType.String);

        var startDate = new SlashCommandOptionBuilder().WithName("date")
            .WithDescription("Date of the event (DD/MM/YYYY)")
            .WithRequired(true)
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
        
        // TODO: Add cover image

        return new SlashCommandBuilder().WithName(Name)
            .WithDescription("Create a new guild event")
            .AddOption(name)
            .AddOption(startDate)
            .AddOption(startTime)
            .AddOption(description)
            .AddOption(channel)
            .AddOption(physicalLocation);
    }
    
    private struct CommandOptions
    {
        public string EventName { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string? Description { get; set; }
        public IGuildChannel? Channel { get; set; }
        public string? PhysicalLocation { get; set; }

        public CommandOptions(SocketSlashCommand command)
        {
            foreach (var option in command.Data.Options)
            {
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
                }
            }
        }
    }
}