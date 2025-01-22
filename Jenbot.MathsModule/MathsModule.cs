using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Jenbot.MathsModule.WolframAlphaApi;

namespace Jenbot.MathsModule;

public class MathsModule : ApplicationCommandModule
{
    private static WolframApi? _wolframApi;

    ///<summary>
    /// Use the given API key to access the Wolfram Alpha API
    ///</summary>
    public static void UseWolframApiKey(string apiKey)
    {
        _wolframApi = new WolframApi(apiKey);
    }

    [SlashCommand("ask-wolfram", "Ask Wolfram Alpha a question")]
    public async Task AskWolfram(InteractionContext ctx,
            [Option("question", "The question to ask")] string question)
    {
        await ctx.DeferAsync();

        if (_wolframApi == null)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent($"{ctx.User.Mention}, I cannot ask Wolfram Alpha a question right now"));
            return;
        }

        try
        {
            var answerImageBytes = await _wolframApi.AskQuestionAsync(question);
            var filePath = await MathsUtilities.SaveImageAsync(answerImageBytes);
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent(ctx.User.Mention)
                .AddFiles(new Dictionary<string, Stream>() { { "response.jpg", fs } })
            );

            fs.Close();
            MathsUtilities.DeleteImage(filePath);
        }
        catch (Exception e)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent($"{ctx.User.Mention}, something went wrong with your request"));
            Console.WriteLine($"[ASKWOLFRAMALPHA ERROR] {e}");
        }
    }
}
