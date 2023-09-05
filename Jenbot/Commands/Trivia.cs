using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;
using Jenbot.Trivia;

namespace Jenbot.Commands;

public class Trivia : ICommand
{
    private readonly TriviaApi _api = new();

    public string Name => "trivia";

    public async Task Execute(SocketSlashCommand command)
    {
        var question = await _api.GetQuestion();

        if (question == null)
        {
            await command.FollowupAsync("There was a problem retrieving the question.");
            return;
        }

        var questionEmbed = new TriviaEmbed(question);
        InteractionManager.AddHandler(questionEmbed);
        
        await questionEmbed.Followup(command);
    }

    public SlashCommandBuilder GetCommandBuilder()
    {
        return new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription("Answer a Trivia Question");
    }
}