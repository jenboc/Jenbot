using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Jenbot;

public class BaseModule : ApplicationCommandModule
{
    private const string WEBSITE_URL = "https://www.jensoncain.co.uk";
    private const string ITCH_URL = "https://jenboc.itch.io";
    private const string GITHUB_PROFILE_URL = "https://github.com/jenboc";
    private const string GITHUB_REPO_URL = "https://github.com/jenboc/Jenbot";

    [SlashCommand("ping", "Ping the bot to ensure that it is running")]
    public async Task Ping(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Pong!")); 
    }
    
    [SlashCommand("creator-website", "Get a link to the creator's website")]
    public async Task CreatorWebsite(InteractionContext ctx)
    {
        var message = $"Check out Jenson's website here: {WEBSITE_URL}";

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(message));
    }

    [SlashCommand("creator-itch", "Get a link to the creator's itch.io page")]
    public async Task CreatorItch(InteractionContext ctx)
    {
        var message = $"Check out Jenson's itch.io page here: {ITCH_URL}";

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(message));
    }

    [SlashCommand("creator-github", "Get a link to the creator's github page")]
    public async Task CreatorGithub(InteractionContext ctx)
    {
        var message = $"Check out Jenson's github page here: {GITHUB_PROFILE_URL}";

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(message));
    }

    [SlashCommand("bot-github", "Get a link to the bot's github repository")]
    public async Task BotGithub(InteractionContext ctx)
    {
        var message = $"Check out this bot's github repository here: {GITHUB_REPO_URL}";

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(message));
    }
}
