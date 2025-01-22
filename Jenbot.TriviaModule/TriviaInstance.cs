using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using Jenbot.Interactions;
using Jenbot.TriviaModule.Api;

namespace Jenbot.TriviaModule;

public class TriviaInstance
{
    private TriviaQuestion _question;
    private int[] _ordering;

    private ulong? _messageId;
    private InteractionContext? _ctx;

    public TriviaInstance(TriviaQuestion question)
    {
        _question = question;

        // Ordering is a random permutation of integers in [0,#IncorrectAnswers]
        // Largest integer represents correct answer, the others represent their 
        // corresponding array elements in IncorrectAnswers
        _ordering = new int[question.IncorrectAnswers.Length + 1];
        
        for (var i = 0; i < _ordering.Length; i++)
            _ordering[i] = i;

        // We shuffle this
        _ordering = _ordering.OrderBy(x => Random.Shared.Next()).ToArray();
    }

    ///<summary>
    /// Start the trivia
    ///</summary>
    public async Task Start(InteractionContext ctx)
    {
        var buttons = CreateButtons(false, true);
        var embed = (new TriviaEmbedBuilder(_question)).Build();
        
        // Send the message
        var buttonComponents = buttons.Select(b => b.GetComponent());
        var message = await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .AddEmbed(embed).AddComponents(buttonComponents)
        );

        // Set the context and message ID so that we can edit later.
        _ctx = ctx;
        _messageId = message.Id;
    }

    private Button CreateButton(string label, Button.Colour colour, bool enabled, InteractionEventHandler? eventHandler)
    {
        var button = new Button(label, colour: colour, enabled: enabled);

        if (enabled && eventHandler != null)
            button.OnButtonClick += eventHandler;

        return button;
    }

    private IEnumerable<Button> CreateButtons(bool coloured, bool enabled)
    {
        var (correctColour, incorrectColour) = coloured 
            ? (Button.Colour.Green, Button.Colour.Red)
            : (Button.Colour.Blurple, Button.Colour.Blurple);

        foreach (var i in _ordering)
        {
            // Represents correct answer
            if (i >= _question.IncorrectAnswers.Length)
            {
                yield return CreateButton(_question.CorrectAnswer, correctColour, enabled, CorrectAnswerClickCallback);
                continue;
            }
            
            yield return CreateButton(_question.IncorrectAnswers[i], incorrectColour, enabled, IncorrectAnswerClickCallback);
        }
    }

    private async Task SendResults(DiscordInteraction interaction, bool wasCorrect)
    {
        if (_messageId == null || _ctx == null)
            return;

        var embed = (new TriviaEmbedBuilder(_question)).Build();
        var colouredButtons = CreateButtons(true, false);
        var buttonComponents = colouredButtons.Select(b => b.GetComponent());        

        var mention = interaction.User.Mention;
        var messageText = wasCorrect
            ? $"Well done, {mention}! You got it right!"
            : $"Unlucky {mention}. You got it wrong.";

        // Edit original message to disable & colour buttons
        await _ctx.EditFollowupAsync(_messageId.Value,
            new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(buttonComponents)
        );

        // Send success/fail message
        await interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(messageText)
        );
    }

    private async Task CorrectAnswerClickCallback(object sender, InteractionEventArgs e)
        => await SendResults(e.Interaction, true);
    private async Task IncorrectAnswerClickCallback(object sender, InteractionEventArgs e)
        => await SendResults(e.Interaction, false);
}
