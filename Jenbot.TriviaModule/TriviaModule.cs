using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Jenbot.TriviaModule.Api;

namespace Jenbot.TriviaModule;

public class TriviaModule : ApplicationCommandModule
{
    private static TriviaApi _api = new();
    private static HashSet<TriviaInstance> _ongoingTrivia = new();

    [SlashCommand("trivia", "Answer a Trivia Question")]
    public async Task Trivia(InteractionContext ctx)
    {
        await ctx.DeferAsync();

        var question = await _api.GetQuestionAsync();

        if (question == null)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent("There was a problem getting the question"));
            return;
        }
       
        var instance = new TriviaInstance(question);
        await instance.Start(ctx);

        instance.OnQuestionAnswered += (sender, e) => _ongoingTrivia.Remove(instance);
        _ongoingTrivia.Add(instance);
    }
}
