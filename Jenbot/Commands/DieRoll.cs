using Discord;
using Discord.WebSocket;

namespace Jenbot.Commands;

public class DieRoll : ICommand
{
    public string Name => "die-roll";

    public async Task Execute(SocketSlashCommand command)
    {
        var numSides = Convert.ToInt32(command.Data.Options.First().Value);
        var result = Bot.Random.Next(1, numSides + 1);

        await command.RespondAsync($"{command.User.Mention}, you rolled a {result} with your D{numSides}");
    }

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Roll a dice")
        .AddOption(
            new SlashCommandOptionBuilder()
                .WithName("sides")
                .WithDescription("Number of sides on the die")
                .WithType(ApplicationCommandOptionType.Integer)
                .WithRequired(true));
}