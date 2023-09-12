using Discord.Interactions;
using Jenbot.Interactables;
using Jenbot.Trivia;

namespace Jenbot.Modules;

public class FunCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    private static readonly TriviaApi _triviaApi = new();
    
    [SlashCommand("die-roll", "Roll a dice")]
    public async Task DieRoll(int numSides = 4)
    {
        var result = Bot.Random.Next(1, numSides + 1);
        await RespondAsync($"{Context.User.Mention}, you rolled a {result} with your D{numSides}");
    }

    [SlashCommand("trivia", "Answer a trivia question", runMode: RunMode.Async)]
    public async Task Trivia()
    {
        await DeferAsync();
        
        var q = await _triviaApi.GetQuestion();

        if (q == null)
        {
            await FollowupAsync("There was a problem retrieving the question");
            return; 
        }

        var embed = new TriviaEmbed(q);
        InteractableManager.AddHandler(embed);
        await embed.Followup(Context.Interaction);
    }
}