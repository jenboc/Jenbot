using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Jenbot.TriviaModule.Api;

namespace Jenbot.TriviaModule;

public class TriviaModule : ApplicationCommandModule
{
    private static List<TriviaInstance> _triviaInstances = new();
    private static TriviaApi _api = new();

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

        var instance = new TriviaInstance(ctx, question);
        _triviaInstances.Add(instance);
        
        var embed = instance.GetEmbed();
        var message = await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
            .AddEmbed(embed)
            .AddComponents(instance.GetButtons()));
        
        instance.SetFollowupId(message.Id);
    }

    public static async Task HandleInteraction(DiscordClient client, ComponentInteractionCreateEventArgs eventArgs)
    {
        var id = eventArgs.Id;

        var toRemove = new List<int>(); 
        for (int i = 0; i < _triviaInstances.Count; i++)
        {
            var instance = _triviaInstances[i];
            
            if (!instance.UsesCustomId(id))
                continue;
            
            await instance.HandleAnswer(id, eventArgs.Interaction);
            toRemove.Add(i);
        }

        var diff = 0;
        toRemove.ForEach(i =>
        {
            _triviaInstances.RemoveAt(i - diff);
            diff++;
        });
    }
}