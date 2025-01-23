namespace Jenbot.MathsModule.LatexCompilation;

public class LatexDocument
{
    // Preamble stuff (i.e. packages and title page shenanigans)
    public IEnumerable<string> Packages { get; private set; }
    public string? Title { get; private set; }
    public string? Author { get; private set; }
    public string? Date { get; private set; }

    public string Content { get; private set; }

    public LatexDocument(IEnumerable<string> packages, string? title,
            string? author, string? date, string content)
    {
        Packages = packages;
        Title = title;
        Author = author;
        Date = date;
        Content = content;
    }

    public string Compile(string filename, CompilationTarget target)
        => LatexCompiler.Compile(this, filename, target);
}
