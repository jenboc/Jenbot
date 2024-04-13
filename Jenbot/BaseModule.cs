using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Jenbot;

public class BaseModule : ApplicationCommandModule
{
    private static Dictionary<ulong, List<ulong>> _broadcastIgnore = [];

    [SlashCommand("ping", "Ping the bot to ensure that it is running")]
    public async Task Ping(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Pong!")); 
    }

    [SlashCommand("broadcast-exclude", "Exclude a server member from Jenbot broadcasts")]
    [SlashRequirePermissions(Permissions.Administrator)]
    public async Task BroadcastExclude(InteractionContext ctx,
        [Option("user", "user to exclude")]
        DiscordUser user)
    {
        var guildId = ctx.Guild.Id;

        if (!_broadcastIgnore.ContainsKey(guildId)) 
            _broadcastIgnore.Add(guildId, []);

        _broadcastIgnore[guildId].Add(user.Id);

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .WithContent("Excluded user successfully")
                .AsEphemeral(true));
    }

    [SlashCommand("broadcast-unexclude", "Re-include a server member in Jenbot broadcasts")]
    [SlashRequirePermissions(Permissions.Administrator)]
    public async Task BroadcastUnexclude(InteractionContext ctx,
        [Option("user", "user to reinclude")]
        DiscordUser user)
    {
        var guildId = ctx.Guild.Id;

        if (!_broadcastIgnore.TryGetValue(guildId, out var excluded) || !excluded.Contains(user.Id))
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent("This user wasn't excluded from broadcasts")
                    .AsEphemeral(true));
            return;
        }

        _broadcastIgnore[guildId].Remove(user.Id);

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("User was re-included in broadcasts")
                .AsEphemeral(true));
    }

    [SlashCommand("broadcast-list", "List the members of the server who will receive broadcast messages")]
    [SlashRequirePermissions(Permissions.Administrator)]
    public async Task BroadcastList(InteractionContext ctx)
    {
        // Filter guild members based on if they will receive messages or not
        var guildMembers = ctx.Guild.Members.Values;

        var excludedIds = _broadcastIgnore.TryGetValue(ctx.Guild.Id, out var ids) ? ids : [];

        var excluded = new StringBuilder();
        var included = new StringBuilder();

        foreach (var m in guildMembers) 
        {
            if (excludedIds.Contains(m.Id))
            {
                excluded.AppendLine(m.DisplayName);
                continue;
            }

            included.AppendLine(m.DisplayName);
        }

        // Create an embed
        var embed = new DiscordEmbedBuilder().WithTitle("Broadcast Recipients");
        
        if (included.Length > 0)
            embed.AddField("Recipients", included.ToString(), true);

        if (excluded.Length > 0)
            embed.AddField("Excluded", excluded.ToString(), true);

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(embed)
                .AsEphemeral(true));
    }

    [SlashCommand("broadcast", "Send a DM to everyone (apart from the excluded members) in the server")]
    [SlashRequirePermissions(Permissions.Administrator)]
    public async Task Broadcast(InteractionContext ctx,
        [Option("message", "message to broadcast")]
        string message) 
    {
        await ctx.DeferAsync(true);

        // Construct a list of recipients
        var memberList = ctx.Guild.Members.Values;

        if (_broadcastIgnore.TryGetValue(ctx.Guild.Id, out var excluded))
            memberList = memberList.Where(m => !excluded.Contains(m.Id));
        
        // Send a private message to each user that isn't excluded
        var dmEmbed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Red)
            .WithTitle("Automated Message")
            .WithAuthor(ctx.Member.DisplayName, iconUrl: ctx.User.AvatarUrl)
            .WithFooter($"Please respond to {ctx.User.Username} (aka {ctx.Member.DisplayName} from {ctx.Guild.Name})")
            .WithDescription(message);

        var usernames = new StringBuilder();
        foreach (var u in memberList)
        {
            // Don't DM bots 
            if (u.IsBot)
                continue;

            await u.SendMessageAsync(dmEmbed);
            usernames.AppendLine(u.DisplayName);
        }

        // Create a result embed and send it
        var summaryEmbed = new DiscordEmbedBuilder()
            .WithTitle("Sent Broadcast Message")
            .WithAuthor(ctx.User.Username, iconUrl: ctx.User.AvatarUrl)
            .WithDescription(message)
            .AddField("Recipients", usernames.ToString()); 

        await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .AddEmbed(summaryEmbed)
        );  
    }
}