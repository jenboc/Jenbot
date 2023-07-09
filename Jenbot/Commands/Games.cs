﻿using Discord;
using Discord.WebSocket;

namespace Jenbot.Commands;

public class Games : ICommand
{
    private const string ITCH_URL = "https://jenboc.itch.io";
    
    public string Name { get; }

    public Games()
    {
        Name = "games";
    }

    public async Task Execute(SocketSlashCommand command) => await command.RespondAsync(ITCH_URL);

    public SlashCommandBuilder GetCommandBuilder() => new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription("Get a link to the creator's itch.io page");
}