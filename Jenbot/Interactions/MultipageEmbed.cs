using Discord;
using Discord.WebSocket;

namespace Jenbot.Interactions;

public class MultipageEmbed : Handler, IMessageInteractable
{
    public MultipageEmbed(Dictionary<string, string> pages, string startingPage)
    {
        MenuId = Guid.NewGuid().ToString();
        
        var embedPages = new Dictionary<string, Embed>();

        foreach (var key in pages.Keys)
        {
            var pageData = pages[key];
            embedPages.Add(key, BuildEmbed(key, pageData));
        }

        Pages = embedPages;
        CurrentEmbed = Pages[startingPage];
        MsgComponent = BuildComponent(startingPage);
    }

    public MultipageEmbed(Dictionary<string, Embed> pages, string startingPage)
    {
        MenuId = Guid.NewGuid().ToString();
        Pages = pages;
        CurrentEmbed = pages[startingPage];
        MsgComponent = BuildComponent(startingPage);
    }

    private Dictionary<string, Embed> Pages { get; }
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

    private Embed BuildEmbed(string title, string data)
    {
        return new EmbedBuilder().WithTitle(title)
            .WithDescription(data).Build();
    }

    private void EditMessage(MessageProperties properties)
    {
        properties.Components = MsgComponent;
        properties.Embed = CurrentEmbed;
    }

    public override async Task HandleInteraction(SocketMessageComponent component)
    {
        var selected = string.Join("", component.Data.Values);
        CurrentEmbed = Pages[selected];
        MsgComponent = BuildComponent(selected);

        await component.Message.ModifyAsync(EditMessage);
        await component.DeferAsync();
    }
}