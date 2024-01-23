using System.Web;
using DSharpPlus.Entities;
using Jenbot.TriviaModule.Api;

namespace Jenbot.TriviaModule;

public class TriviaEmbedBuilder
{
    private DiscordEmbedBuilder _builder;
    
    public TriviaEmbedBuilder(TriviaQuestion question)
    {
        _builder = new DiscordEmbedBuilder();

        _builder.WithTitle(HttpUtility.HtmlDecode(question.Question));
        _builder.AddField("Difficulty", question.Difficulty);
        _builder.AddField("Category", question.Category); 
    }

    public DiscordEmbed Build() => _builder.Build();
}