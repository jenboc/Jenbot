using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Jenbot.ChessModule.Api;

namespace Jenbot.ChessModule;

public class ChessModule : ApplicationCommandModule
{
    private static ChessApi _chessApi = new();
    
    [SlashCommand("chesscom-profile", "Get a chess.com profile")]
    public async Task ChessComProfile(InteractionContext ctx, 
        [Option("username", "chess.com username")] string username)
    {
        await ctx.DeferAsync(); 
        
        var playerData = await _chessApi.GetPlayerAsync(username);

        if (playerData == null || string.IsNullOrEmpty(playerData.Profile.Username))
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent($"{username} does not have a chess.com account"));
            return; 
        }

        try
        {
            var embed = new ChessComEmbedBuilder(playerData);
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed.Build()));
        }
        catch (Exception e)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent($"Something went wrong with your request"));
            Console.WriteLine($"[CHESSCOMPROFILE ERROR] {e}");
        }
    }
}