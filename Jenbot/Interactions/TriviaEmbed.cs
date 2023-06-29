using Discord;
using Discord.WebSocket;
using Jenbot.Trivia;

namespace Jenbot.Interactions;

public class TriviaEmbed : Handler, IMessageInteractable
{
    private readonly TriviaQuestion _question;
    private Embed Embed {get;}

    public TriviaEmbed(TriviaQuestion question)
    {
        _question = question;
        Embed = CreateEmbed();
        MsgComponent = CreateComponent(); 
    }

    public override async Task HandleInteraction(SocketMessageComponent component)
    {
        if (component.Data.Value == _question.CorrectAnswer)
        {
            await component.RespondAsync($"Well done {component.User.Mention}! You got it right!");
            return; 
        }

        await component.RespondAsync($"Unlucky {component.User.Mention}, you got it wrong!");
        return;
    }

    public async Task Reply(SocketInteraction interaction) => 
        await interaction.RespondAsync(embed: Embed, components: MsgComponent);
    
    public async Task Send(SocketTextChannel channel) => 
        await channel.SendMessageAsync(embed: Embed, components: MsgComponent);

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
            rowBuilder.AddComponent(buttonComponent);

        return compBuilder.AddRow(rowBuilder).Build();
    }
}