using Discord.Interactions;

namespace Jenbot.Modules;

public class BasicCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Replies with pong!")]
    public async Task Ping()
    {
        await RespondAsync("pong!");
    }

    [SlashCommand("github", "Get a link to the bot's github page")]
    public async Task Github()
    {
        await RespondAsync("https://github.com/jenboc/Jenbot");
    }

    [SlashCommand("itch-io", "Get a link to the creator's itch.io page")]
    public async Task ItchIo()
    {
        await RespondAsync("https://jenboc.itch.io");
    }
}