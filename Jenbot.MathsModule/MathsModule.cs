using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Jenbot.MathsModule.WolframAlphaApi;
using Jenbot.MathsModule.LatexCompilation;

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
            File.Delete(filePath);
        }
        catch (Exception e)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent($"{ctx.User.Mention}, something went wrong with your request"));
            Console.WriteLine($"[ASKWOLFRAMALPHA ERROR] {e}");
        }
    }

    [SlashCommand("compile-latex", "Compile a Latex snippet as an image and send it to the channel")]
    public async Task CompileLatex(InteractionContext ctx,
            [Option("snippet", "The code snippet")] string snippet)
    {
        await ctx.DeferAsync();

        if (!LatexUtilities.IsCodeSnippetSafe(snippet))
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent($"{ctx.User.Mention}, unsafe content detected. Will not compile."));
            return;
        }

        var latexDoc = MathsUtilities.GetDefaultStandaloneLatexBuilder()
            .SetContent(snippet).Build();

        try 
        {
            var guid = Guid.NewGuid();
            var imgPath = latexDoc.Compile(guid.ToString(), CompilationTarget.Image);
            
            var fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .AddFiles(new Dictionary<string, Stream>() { { "latex.jpg", fs } })
            );

            fs.Close();
            File.Delete(imgPath);
        }
        catch (CompilationFailException e)
        {
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent($"{ctx.User.Mention}, something went wrong in compilation:\n```\n{e.Message}\n```")
            );
            Console.WriteLine($"[LATEXCOMPILATION ERROR] {e}");
            File.Delete(e.PdfPath);
        }
    }

    public static async Task OnMessageSent(DiscordClient s, MessageCreateEventArgs e)
    {
        // Ignore bot messages
        if (e.Author.IsBot)
            return;

        var content = e.Message.Content;
        
        // If we don't use maths mode or commands then why compile?
        // Further, we don't compile if it is not safe to do so
        if ((!LatexUtilities.SnippetUsesMathsMode(content) 
                && !LatexUtilities.SnippetUsesCommands(content))
                || !LatexUtilities.IsCodeSnippetSafe(content))
            return;

        // Content is indeed a latex snippet, and is safe so we can compile
        var latexDoc = MathsUtilities.GetDefaultStandaloneLatexBuilder()
            .SetContent(content).Build();

        try
        {
            var guid = Guid.NewGuid();
            var imgPath = latexDoc.Compile(guid.ToString(), CompilationTarget.Image);

            var fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);

            await e.Message.RespondAsync(new DiscordMessageBuilder()
                .AddFiles(new Dictionary<string, Stream> { { "latex.jpg", fs } })
            );

            fs.Close();
            File.Delete(imgPath);
        }
        catch (CompilationFailException ex)
        {
            await e.Message.RespondAsync(new DiscordMessageBuilder()
                .WithContent($"{e.Author.Mention}, something went wrong in compilation:\n```\n{ex.Message}\n```"));
            File.Delete(ex.PdfPath);
        }
    }
}
