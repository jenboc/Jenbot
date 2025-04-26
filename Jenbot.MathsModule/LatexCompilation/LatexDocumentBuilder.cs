namespace Jenbot.MathsModule.LatexCompilation;

public class LatexDocumentBuilder
{
    private const string DOCUMENT_CLASS_COMMAND = "documentclass";
    private const string USE_PACKAGE_COMMAND = "usepackage";

    private LatexCommand _documentClassCommand;
    private List<LatexCommand> _packageCommands;
    private List<LatexCommand> _otherPreambleCommands;
    private string? _title;
    private string? _author;
    private string? _date;

    private string _content;

    public LatexDocumentBuilder()
    {
        _documentClassCommand = new(DOCUMENT_CLASS_COMMAND, "article");
        _packageCommands = new();
        _otherPreambleCommands = new();
        _content = string.Empty;
    }

    public LatexDocumentBuilder SetDocumentClass(LatexDocumentClass documentClass,
            IDictionary<string, string> optionalArguments)
    {
        _documentClassCommand = new(DOCUMENT_CLASS_COMMAND, GetDocClassString(documentClass), optionalArguments);
        return this;
    }

    public LatexDocumentBuilder SetDocumentClass(LatexDocumentClass documentClass)
    {
        _documentClassCommand = new(DOCUMENT_CLASS_COMMAND, GetDocClassString(documentClass));
        return this;
    }

    private static string GetDocClassString(LatexDocumentClass dc) => dc switch 
    {
        LatexDocumentClass.Article => "article",
        LatexDocumentClass.Report => "report",
        LatexDocumentClass.Book => "book",
        LatexDocumentClass.Standalone => "standalone",
        // This shouldn't happen
        _ => throw new ArgumentOutOfRangeException("You cannot use that document class here")
    };

    public LatexDocumentBuilder AddPackage(string packageName, IDictionary<string, string> optionalArguments)
    {
        var command = new LatexCommand(USE_PACKAGE_COMMAND, packageName, optionalArguments);
        _packageCommands.Add(command);

        return this;
    }

    public LatexDocumentBuilder AddPackage(string packageName)
    {
        var command = new LatexCommand(USE_PACKAGE_COMMAND, packageName);
        _packageCommands.Add(command);

        return this;
    }

    public LatexDocumentBuilder AddPreambleCommand(string commandName, string argument)
    {
        var command = new LatexCommand(commandName, argument);
        _otherPreambleCommands.Add(command);

        return this;
    }

    public LatexDocumentBuilder AddPreambleCommand(string commandName, string argument,
            IDictionary<string, string> optionalArguments)
    {
        var command = new LatexCommand(commandName, argument, optionalArguments);
        _packageCommands.Add(command);

        return this;
    }

    public LatexDocumentBuilder SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public LatexDocumentBuilder SetAuthor(string author)
    {
        _author = author;
        return this;
    }

    public LatexDocumentBuilder SetDate(string dateString)
    {
        _date = dateString;
        return this;
    }
    
    public LatexDocumentBuilder AppendContent(string content, string joinWith="")
    {
        _content += joinWith + content;
        return this;
    }

    public LatexDocumentBuilder SetContent(string content)
    {
        _content = content;
        return this;
    }

    public LatexDocumentBuilder ClearContent()
    {
        _content = string.Empty;
        return this;
    }

    public LatexDocument Build()
        => new LatexDocument(_documentClassCommand, _packageCommands, _otherPreambleCommands,
                _title, _author, _date, _content);
}
