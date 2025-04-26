using System.Text.RegularExpressions;

namespace Jenbot.MathsModule.LatexCompilation;

public static class LatexUtilities
{
    private static List<string> _unsafeCommands = new()
    {
        "input",
        "immediate",
        "write18"
    };

    private static List<string[]> _mathsBrackets = new()
    {
        new[] { "$", "$" },
        new[] { "\\(", ")\\" },
        new[] { "\\[", "]\\" },
        // $$ $$ is not required as it will be picked up by single $s
    };

    public static bool IsCodeSnippetSafe(string snippet)
        => !SnippetContainsUnsafeCommand(snippet);

    private static bool SnippetContainsUnsafeCommand(string snippet)
    {
        foreach (var comm in _unsafeCommands)
        {
            if (snippet.Contains($"\\{comm}"))
                return true;
        }

        return false;
    }

    public static bool SnippetUsesMathsMode(string snippet)
        => _mathsBrackets.Any(bs => StringUsesBrackets(snippet, bs[0], bs[1]));

    private static bool StringUsesBrackets(string s, string open, string close)
    {
        var startIndex = s.IndexOf(open);
        var endIndex = s.LastIndexOf(close);

        return startIndex >= 0 && endIndex > startIndex;
    }

    public static bool SnippetUsesCommands(string snippet)
        => (new Regex(@"\\\w+", RegexOptions.IgnoreCase))
                .Match(snippet).Success;
}
