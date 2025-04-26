using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Jenbot.Interactions;

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
        var intents = DiscordIntents.MessageContents
            | DiscordIntents.GuildMessages
            | DiscordIntents.DirectMessages;
        var discordConfig = new DiscordConfiguration()
        {
            Token = _config.Token,
            TokenType = TokenType.Bot,
            Intents = intents,
            MinimumLogLevel = LogLevel.Debug
        };
        _client = new DiscordClient(discordConfig);
    
        SetupModules();

        _client.MessageCreated += OnMessageSent;
    }
    
    ///<summary>
    /// Setup the bot modules, i.e. setup API keys, etc.
    ///</summary>
    private void SetupModules()
    {
        MathsModule.MathsModule.UseWolframApiKey(_config.WolframAppId);
    }

    ///<summary>
    /// Call all functions which should run on Message Sent
    ///</summary>
    private async Task OnMessageSent(DiscordClient s, MessageCreateEventArgs e)
    {
        Console.WriteLine("Message Sent");
        await MathsModule.MathsModule.OnMessageSent(s, e);
    }

    /// <summary>
    /// Start the bot 
    /// </summary>
    public async Task Start()
    {
        RegisterSlashCommands();
        _client.ComponentInteractionCreated += InteractionHandler.OnComponentInteractionCreated;
        
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
        slash.RegisterCommands<MathsModule.MathsModule>();
        slash.RegisterCommands<GamesModule.GamesModule>();
    }

    /// <summary>
    /// Class for storing the config data
    /// </summary>
    private class BotConfig
    {
        public required string Token { get; set; }
        public required string WolframAppId { get; set; }
    }
}
