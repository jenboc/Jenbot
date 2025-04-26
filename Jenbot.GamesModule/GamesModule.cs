using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Jenbot.GamesModule;

public class GamesModule : ApplicationCommandModule
{
    [SlashCommand("noughtsandcrosses", "Play a game of noughts and crosses")]
    public async Task PlayNoughtsAndCrosses(InteractionContext ctx,
            [Option("opponent", "Who are you playing against?")] 
            DiscordUser opponent,
            [Option("board_size", "How big is the board going to be?")] 
            NoughtsAndCrossesSize boardSize = NoughtsAndCrossesSize.ThreeByThree,
            [Option("max_tiles", 
                "Each player can only have this amount of tiles on the board at a given time, 0 means it is disabled")] 
            long maxTiles = 0)
    {
        await ctx.DeferAsync();

        if (ctx.User.IsBot)
            return;

        var boardSizeNum = boardSize switch
        {
            NoughtsAndCrossesSize.FourByFour => 4,
            NoughtsAndCrossesSize.FiveByFive => 5,
            _ => 3
        };

        try
        {
            var game = new NoughtsAndCrosses.Game(ctx.User, opponent, boardSizeNum, Convert.ToInt32(maxTiles));
            await game.Start(ctx);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(
                    $"{ctx.User.Mention} failed to create Noughts and Crosses Game\n{ex.Message}"
            ));
            return;
        }
    }

    public enum NoughtsAndCrossesSize
    {
        [ChoiceName("3x3")]
        ThreeByThree,
        [ChoiceName("4x4")]
        FourByFour,
        [ChoiceName("5x5")]
        FiveByFive
    }
}
