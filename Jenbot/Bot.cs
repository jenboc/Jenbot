﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Jenbot;

public class Bot
{
    private const string CONFIG_FILE = "appsettings.json";
    private BotConfig _config;
    private DiscordClient _client;
    
    public Bot()
    {
        // Load config from appsettings.json 
        var configFile = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(CONFIG_FILE)
            .Build();
        _config = configFile.GetSection("BotConfig").Get<BotConfig>()
                  ?? throw new Exception($"Error loading {CONFIG_FILE} for Bot Configuration");
        
        // Instantiate Client 
        var discordConfig = new DiscordConfiguration()
        {
            Token = _config.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged,
            MinimumLogLevel = LogLevel.Debug
        };
        _client = new DiscordClient(discordConfig); 
    }
    
    /// <summary>
    /// Start the bot 
    /// </summary>
    public async Task Start()
    {
        RegisterSlashCommands();
        _client.ComponentInteractionCreated += ComponentInteractionCreated;
        
        // Start the bot 
        await _client.ConnectAsync();
        Console.WriteLine("Bot has started");
        await Task.Delay(-1); 
    }

    /// <summary>
    /// Register Bot Slash Commands
    /// </summary>
    private void RegisterSlashCommands()
    {
        var slash = _client.UseSlashCommands();
        slash.RegisterCommands<BaseModule>(); 
        slash.RegisterCommands<ChessModule.ChessModule>();
        slash.RegisterCommands<TriviaModule.TriviaModule>();
    }

    /// <summary>
    /// Interaction Created Callback
    /// </summary>
    private async Task ComponentInteractionCreated(DiscordClient client, ComponentInteractionCreateEventArgs eventArgs)
    {
        // Check where the interaction came from 
        var flag = eventArgs.Id[0];

        switch (flag)
        {
            case 'T':
                await TriviaModule.TriviaModule.HandleInteraction(client, eventArgs);
                break;
        }
    }
    
    // Struct for appsettings.json 
    private class BotConfig
    {
        public string Token { get; set; }
    }
}