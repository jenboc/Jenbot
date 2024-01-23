using System.Web;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Jenbot.TriviaModule.Api;

namespace Jenbot.TriviaModule;

public class TriviaInstance
{
    private InteractionContext _ctx;
    private TriviaQuestion _question;
    private List<DiscordButtonComponent> _buttons;
    private string _correctAnswerId;
    private ulong _followupMessageId;

    public TriviaInstance(InteractionContext ctx, TriviaQuestion question)
    {
        _ctx = ctx;
        _question = question;
        _buttons = new List<DiscordButtonComponent>();
        CreateButtons(); 
    }
    
    private void CreateButtons()
    {
        var r = new Random();

        var options = _question.IncorrectAnswers.ToList();
        options.Add(_question.CorrectAnswer);

        var correctAnswer = HttpUtility.HtmlDecode(_question.CorrectAnswer);

        foreach (var button in options.OrderBy(x => r.Next()).Select(o =>
                     new DiscordButtonComponent(ButtonStyle.Primary, "T"+Guid.NewGuid().ToString(),
                         HttpUtility.HtmlDecode(o))))
        {
            _buttons.Add(button);

            if (string.Equals(button.Label, correctAnswer, StringComparison.CurrentCultureIgnoreCase))
                _correctAnswerId = button.CustomId;
        }
    }

    private void RecolourButtons()
    {
        var newButtons = new List<DiscordButtonComponent>();
        
        foreach (var button in _buttons)
        {
            var colour = button.CustomId == _correctAnswerId ? ButtonStyle.Success : ButtonStyle.Danger;
            var newButton = new DiscordButtonComponent(colour, button.CustomId, button.Label, true);
            newButtons.Add(newButton);
        }

        _buttons = newButtons;
    }

    public async Task HandleAnswer(string customId, DiscordInteraction interaction)
    {
        // Edit the message with recoloured buttons
        RecolourButtons();
        await _ctx.EditFollowupAsync(_followupMessageId,
            new DiscordWebhookBuilder().AddEmbed(GetEmbed()).AddComponents(_buttons));
        
        // Tell the user if they are right or wrong 
        var message = customId == _correctAnswerId
            ? $"Well done, {interaction.User.Mention}! You got it right!"
            : $"Unlucky {interaction.User.Mention}. You got it wrong.";
        await interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(message));
    }

    public bool UsesCustomId(string customId)
    {
        foreach (var button in _buttons)
            if (button.CustomId == customId)
                return true;

        return false;
    }

    public void SetFollowupId(ulong id) => _followupMessageId = id; 
    public DiscordEmbed GetEmbed() => (new TriviaEmbedBuilder(_question)).Build();
    public List<DiscordButtonComponent> GetButtons() => _buttons;
}