using System.Text.RegularExpressions;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Jenbot.MiscModule;

public class MiscModule : ApplicationCommandModule
{
    [SlashCommand("change-colour", "Change your name colour in the server")]
    public async Task ChangeColour(InteractionContext ctx,
            [Option("colour", "Hexadecimal code of choice (e.g. #ffffff)")]
            string hexInput
    )
    {
        await ctx.DeferAsync(ephemeral: true);

        // Extract Colour
        var cleanHex = Regex.Replace(hexInput, "[^0-9a-fA-F]", "");
        if (cleanHex.Length != 6)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(
                $"{ctx.User.Mention}, you have entered an invalid colour. "
                + "Please enter a valid 6-character hex code (e.g. #ffffff)."
            ));
            return;
        }
        var colour = new DiscordColor(cleanHex);

        // Change the member's colour
        var roleName = ctx.User.Id.ToString();
        var role = ctx.Guild.Roles.Values
            .FirstOrDefault(r => r.Name == roleName);

        if (role is not null)
        {
            await role.ModifyAsync(color: colour, mentionable: false);
        }
        else
        {
            role = await ctx.Guild.CreateRoleAsync(
                name: roleName,
                color: colour,
                mentionable: false,
                hoist: false
            );

            await ctx.Member.GrantRoleAsync(role);
        }

        // Response
        var embed = new DiscordEmbedBuilder()
            .WithTitle("Colour Selected!")
            .WithDescription($"Applied colour: `#{cleanHex.ToUpper()}`")
            .WithColor(colour);

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
    }
}
