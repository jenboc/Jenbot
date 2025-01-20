using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Jenbot;

public class BaseModule : ApplicationCommandModule
{
    [SlashCommand("ping", "Ping the bot to ensure that it is running")]
    public async Task Ping(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Pong!")); 
    }
}
