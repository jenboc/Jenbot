using Discord.Interactions;
using Jenbot.Chess;

namespace Jenbot.Modules;


public class StatsCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    private static ChessApi _chessApi = new(); 
    
    [SlashCommand("chesscom-profile", "Look at someone's chess.com profile", runMode: RunMode.Async)]
    public async Task ChessComProfile(string username)
    {
        await DeferAsync();
        
        var playerData = await _chessApi.GetPlayerAsync(username);

        if (playerData == null || string.IsNullOrEmpty(playerData.Profile.Username))
        {
            await FollowupAsync($"{username} does not have a profile on chess.com");
            return;
        }

        try
        {
            var embed = ChessComEmbed.Build(playerData);
            await FollowupAsync(embed: embed);
        }
        catch (Exception e)
        {
            await FollowupAsync("There was a problem processing your request");
            Console.WriteLine(e);
        }
    }
}