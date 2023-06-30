using Discord;
using Discord.WebSocket;

namespace Jenbot.Interactions;

public class MultipageEmbed : Handler, IMessageInteractable
{
    public MultipageEmbed(Dictionary<string, string> pages, string startingPage)
    {
        MenuId = Guid.NewGuid().ToString();
        Pages = pages;
        MsgComponent = BuildComponent(startingPage);
        CurrentEmbed = BuildEmbed(startingPage);
    }

    private Dictionary<string, string> Pages { get; }
    private Embed CurrentEmbed { get; set; }

    private string MenuId { get; }

    public async Task Reply(SocketInteraction interaction)
    {
        await interaction.RespondAsync(embed: CurrentEmbed, components: MsgComponent);
    }

    public async Task Send(ISocketMessageChannel channel)
    {
        await channel.SendMessageAsync(embed: CurrentEmbed, components: MsgComponent);
    }

    private MessageComponent BuildComponent(string selected)
    {
        var builder = new ComponentBuilder();

        var menu = new SelectMenuBuilder().WithCustomId(MenuId);
        foreach (var key in Pages.Keys)
            menu.AddOption(key, key, isDefault: key == selected);

        return builder.WithSelectMenu(menu).Build();
    }

    private Embed BuildEmbed(string key)
    {
        return new EmbedBuilder().WithTitle(key)
            .WithDescription(Pages[key]).Build();
    }

    private void EditMessage(MessageProperties properties)
    {
        properties.Components = MsgComponent;
        properties.Embed = CurrentEmbed;
    }

    public override async Task HandleInteraction(SocketMessageComponent component)
    {
        var selected = string.Join("", component.Data.Values);
        CurrentEmbed = BuildEmbed(selected);
        MsgComponent = BuildComponent(selected);

        await component.Message.ModifyAsync(EditMessage);
        await component.DeferAsync();
    }
}