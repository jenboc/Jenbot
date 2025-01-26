using System.Text;

namespace Jenbot.MathsModule.LatexCompilation;

public class LatexDocument
{
    private LatexCommand _documentClassCommand;
    private IEnumerable<LatexCommand> _packageCommands;
    private IEnumerable<LatexCommand> _preambleCommands;

    // Preamble stuff (i.e. packages and title page shenanigans)
    public string DocumentClass => _documentClassCommand.GetPrincipleArgument();
    public IEnumerable<string> Packages => _packageCommands.Select(x => x.GetPrincipleArgument());
    public string? Title { get; private set; }
    public string? Author { get; private set; }
    public string? Date { get; private set; }

    public string Content { get; private set; }

    public LatexDocument(LatexCommand docClassCommand, IEnumerable<LatexCommand> packageCommands,
            IEnumerable<LatexCommand> preambleCommands, string? title, string? author, string? date,
            string content)
    {
        _documentClassCommand = docClassCommand;
        _packageCommands = packageCommands;
        _preambleCommands = preambleCommands;
        Title = title;
        Author = author;
        Date = date;
        Content = content;
    }

    public string GetFileContents()
    {
        var builder = new StringBuilder();

        builder.AppendLine(BuildPreambleString());
        builder.AppendLine("\\begin{document}");
        builder.AppendLine(BuildContentString());
        builder.AppendLine("\\end{document}");

        return builder.ToString();
    }

    public string Compile(string filename, CompilationTarget target)
        => LatexCompiler.Compile(this, filename, target);

    private string BuildPreambleString()
    {
        var builder = new StringBuilder();
        builder.AppendLine(_documentClassCommand.GetLatexCode());
     
        foreach (var comm in _packageCommands)
            builder.AppendLine(comm.GetLatexCode());

        foreach (var comm in _preambleCommands)
            builder.AppendLine(comm.GetLatexCode());

        if (ShouldMakeTitle())
            builder.AppendLine(BuildTitlePreambleString());

        return builder.ToString();
    }

    private string BuildContentString()
    {
        if (!ShouldMakeTitle())
        {
            return Content;
        }

        var builder = new StringBuilder();
        builder.AppendLine("\\maketitle");
        builder.AppendLine(Content);
        return builder.ToString();
    }

    private string BuildTitlePreambleString()
    {
        var optionals = new Dictionary<string, string?>()
        { 
            {"title", Title},
            {"author", Author},
            {"date", Date}
        };
        var builder = new StringBuilder();

        foreach (var pair in optionals)
        {
            if (pair.Value == null) continue;

            var command = new LatexCommand(pair.Key, pair.Value);
            builder.AppendLine(command.GetLatexCode());
        }

        return builder.ToString();
    }

    private bool ShouldMakeTitle()
        => Title != null;
}
