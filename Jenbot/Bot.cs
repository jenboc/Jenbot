using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Jenbot;

public class Bot
{
    private DiscordSocketClient _client;
    private BotConfig _config;
    
    private const string CONFIG_FILE = "appsettings.json";

    public Bot()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.Ready += Ready;

        var configFile = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(CONFIG_FILE).Build();
        _config = configFile.GetSection("BotConfig").Get<BotConfig>() 
                  ?? throw new Exception($"Error loading {CONFIG_FILE} for Bot Configuration");
    }

    public async Task Start()
    {
        await _client.LoginAsync(TokenType.Bot, _config.Token);
        await _client.StartAsync(); 
        
        // Delay task until program closed
        await Task.Delay(-1);
    }

    private async Task Ready()
    {
        // Prepare commands 
    }

    private Task Log(LogMessage message)
    {
        // TODO: Write logging system
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}