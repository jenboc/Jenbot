using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;
using Jenbot.Trivia;

namespace Jenbot.Commands;

public class Trivia : ICommand
{
    public string Name { get; }
    private TriviaApi _api;     
    
    public Trivia()
    {
        Name = "trivia";
        _api = new TriviaApi();
    }
    
    public async Task Execute(SocketSlashCommand command)
    {
        var question = await _api.GetQuestion();

        if (question == null)
        {
            await command.RespondAsync("There was a problem retrieving the question.");
            return;
        }

        var questionEmbed = BuildQuestionEmbed(question);
        await questionEmbed.Reply(command);
    }

    private QuestionEmbed BuildQuestionEmbed(TriviaQuestion question)
    {
        return new MultipleChoiceQuestion(question);
    }

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Answer a Trivia Question");
}