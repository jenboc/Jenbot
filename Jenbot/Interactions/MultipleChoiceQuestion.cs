using Discord;
using Discord.WebSocket;
using Jenbot.Trivia;

namespace Jenbot.Interactions;

public class MultipleChoiceQuestion : QuestionEmbed
{
    private readonly Dictionary<string, string> _options;

    public MultipleChoiceQuestion(TriviaQuestion question) : base(question)
    {
        _options = new Dictionary<string, string>();
        
        AddOption(Question.CorrectAnswer);
        Question.IncorrectAnswers.ForEach(AddOption);
    }

    private void AddOption(string option)
    {
        _options.Add(Guid.NewGuid().ToString(), option);
    }

    public override async Task HandleInteraction(SocketMessageComponent component)
    {
        var validAnswer = _options.TryGetValue(component.Data.CustomId, out var answer);

        if (!validAnswer)
        {
            await component.RespondAsync("There was a problem when processing your answer");
            return;
        }

        await component.RespondAsync(answer == Question.CorrectAnswer
            ? "Well done! That is correct."
            : "Unlucky! That was incorrect");
    }

    protected override Embed CreateEmbed() => new EmbedBuilder().WithTitle(Question.Question).Build();

    protected MessageComponent CreateComponents()
    {
        var buttons = new List<IMessageComponent>();
        
        foreach (var keyValuePair in _options)
        {
            buttons.Add(new ButtonBuilder()
                .WithCustomId(keyValuePair.Key)
                .WithLabel(keyValuePair.Value)
                .WithStyle(ButtonStyle.Primary).Build());
        }

        var r = new Random();
        buttons = buttons.OrderBy(button => r.Next()).ToList();
        var actionRow = new ActionRowBuilder().WithComponents(buttons);

        return new ComponentBuilder().AddRow(actionRow).Build();
    }
}