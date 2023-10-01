using System.Text;
using Discord.Interactions;
using Jenbot.Interactables;
using Jenbot.Trivia;
using ZXing;
using ZXing.Common;

namespace Jenbot.Modules;

public class FunCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    private static readonly TriviaApi _triviaApi = new();

    private const string VIRTUAL_CARD_URL = "http://www.jensoncain.co.uk/virtual-card";
    
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

    private string GetVirtualCardUrl(string header, string message)
    {
        if (string.IsNullOrEmpty(header) || string.IsNullOrEmpty(message))
        {
            return ""; 
        }

        var encodedHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
        var encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        return $"{VIRTUAL_CARD_URL}/card.html?h={encodedHeader}&m={encodedMessage}";
    }

    [SlashCommand("virtual-card-link", "Write a Virtual Card using my website!")]
    public async Task VirtualCardLink(string header = "", string message = "")
    {
        await DeferAsync();
        var url = GetVirtualCardUrl(header, message);

        if (string.IsNullOrEmpty(url))
        {
            await FollowupAsync($"{Context.User.Mention}, either provide a header and a message or use " +
                                $"{VIRTUAL_CARD_URL} to create a virtual card");
            return;
        }

        await FollowupAsync(url);
    }
}