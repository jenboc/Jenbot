using Discord;
using Discord.WebSocket;
using Jenbot.Trivia;

namespace Jenbot.Interactions;

public abstract class QuestionEmbed : Handler, IMessageInteractable
{
    protected TriviaQuestion Question { get; }

    public QuestionEmbed(TriviaQuestion question) => Question = question;

    protected abstract Embed CreateEmbed();

    public async Task Reply(SocketInteraction interaction)
    {
        await interaction.RespondAsync(embed: CreateEmbed(), components: MsgComponent);
    }

    public async Task Send(SocketTextChannel channel)
    {
        await channel.SendMessageAsync(embed: CreateEmbed(), components: MsgComponent);
    }
}