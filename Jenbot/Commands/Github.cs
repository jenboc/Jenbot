using Discord;
using Discord.WebSocket;
using Jenbot.Interactions;

namespace Jenbot.Commands;

public class Github : ICommand
{
    private const string REPO_URL = "https://github.com/jenboc/Jenbot";
    
    public string Name => "github";

    public async Task Execute(SocketSlashCommand command) => await command.FollowupAsync(REPO_URL);

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("See the github repo for this bot"); 
}