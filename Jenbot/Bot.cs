using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Jenbot.Interactables;

namespace Jenbot;

public class Bot
{
    private const string CONFIG_FILE = "appsettings.json";
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly BotConfig _config;

    public static readonly Random Random = new Random();
    
    public Bot()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            UseInteractionSnowflakeDate = false
        });
        _interactionService = new InteractionService(_client);
        
        _client.Log += Log;
        _client.Ready += Ready;
        _client.ButtonExecuted += InteractableManager.HandleComponents;
        _client.SelectMenuExecuted += InteractableManager.HandleComponents;
        _client.InteractionCreated += async (x) =>
        {
            var ctx = new SocketInteractionContext(_client, x);
            await _interactionService.ExecuteCommandAsync(ctx, null);
        };
        
        var configFile = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(CONFIG_FILE).Build();
        _config = configFile.GetSection("BotConfig").Get<BotConfig>()
                  ?? throw new Exception($"Error loading {CONFIG_FILE} for Bot Configuration");
    }

    public async Task Start()
    {
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        
        await _client.LoginAsync(TokenType.Bot, _config.Token);
        await _client.StartAsync();
        
        // Delay task until program closed
        await Task.Delay(-1);
    }

    private async Task Ready()
    {
        await _interactionService.RegisterCommandsGloballyAsync();
        await _client.SetGameAsync("Being Bottastic");
    }

    private Task Log(LogMessage message)
    {
        // TODO: Write logging system
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}