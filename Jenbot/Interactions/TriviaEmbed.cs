using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Jenbot.Trivia;

namespace Jenbot.Interactions;

public class TriviaEmbed : Handler, IMessageInteractable
{
    private readonly TriviaQuestion _question;
    private string _correctAnswerId;
    private RestUserMessage _message;
    private Embed Embed { get; }


    public TriviaEmbed(TriviaQuestion question)
    {
        _question = question;
        Embed = CreateEmbed();
        MsgComponent = CreateComponent(); 
    }

    public override async Task HandleInteraction(SocketMessageComponent component)
    {
        RecolourButtons();
        await _message.ModifyAsync(p => p.Components = MsgComponent);

        await component.RespondAsync(component.Data.CustomId == _correctAnswerId ? $"Well done {component.User.Mention}! You got it right!" 
            : $"Unlucky {component.User.Mention}, you got it wrong!");
    }

    private void RecolourButtons()
    {
        var rowBuilder = new ActionRowBuilder();
        
        foreach (var row in MsgComponent.Components)
        {
            foreach (var comp in row.Components)
            {
                if (comp is not ButtonComponent)
                    continue;

                var button = comp as ButtonComponent;

                var editedButton = new ButtonBuilder()
                    .WithLabel(button.Label)
                    .WithCustomId(button.CustomId)
                    .WithStyle(button.CustomId == _correctAnswerId ? ButtonStyle.Success : ButtonStyle.Danger)
                    .WithDisabled(true);
                rowBuilder.WithButton(editedButton);
            }
        }

        MsgComponent = new ComponentBuilder().AddRow(rowBuilder).Build();
    }

    public async Task Reply(SocketInteraction interaction) => 
        await interaction.RespondAsync(embed: Embed, components: MsgComponent);
    
    public async Task Send(ISocketMessageChannel channel) => 
        _message = await channel.SendMessageAsync(embed: Embed, components: MsgComponent);

    private Embed CreateEmbed() => new EmbedBuilder()
        .WithTitle(_question.Question)
        .AddField("Difficulty", _question.Difficulty)
        .AddField("Category", _question.Category)
        .Build();

    private MessageComponent CreateComponent()
    {
        var compBuilder = new ComponentBuilder();
        var rowBuilder = new ActionRowBuilder();

        var r = new Random();
        var options = _question.IncorrectAnswers.ToList();
        options.Add(_question.CorrectAnswer);

        foreach (var buttonComponent in options.OrderBy(x => r.Next()).Select(o => new ButtonBuilder()
                     .WithLabel(o).WithStyle(ButtonStyle.Primary).WithCustomId(Guid.NewGuid().ToString()).Build()))
        {
            rowBuilder.AddComponent(buttonComponent);

            if (buttonComponent.Label.ToLower() == _question.CorrectAnswer.ToLower())
                _correctAnswerId = buttonComponent.CustomId;
        }

        return compBuilder.AddRow(rowBuilder).Build();
    }
}