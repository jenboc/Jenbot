using System.Globalization;
using DSharpPlus.Entities;
using Jenbot.TriviaModule.Api;

namespace Jenbot.TriviaModule;

public class TriviaEmbedBuilder
{
    private DiscordEmbedBuilder _builder;
    
    public TriviaEmbedBuilder(TriviaQuestion question)
    {
        _builder = new DiscordEmbedBuilder();

        var textInfo = new CultureInfo("en-GB", false).TextInfo;

        _builder.WithTitle(question.Question);
        _builder.AddField(
                "Difficulty",
                textInfo.ToTitleCase(question.Difficulty)
        );
        _builder.AddField(
                "Category", 
                textInfo.ToTitleCase(question.Category)
        ); 
    }

    public DiscordEmbed Build() => _builder.Build();
}
