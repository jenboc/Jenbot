namespace Jenbot.MathsModule.LatexCompilation;

public class LatexDocumentBuilder
{
    private HashSet<string> _packages;
    private string? _title;
    private string? _author;
    private string? _date;

    private string _content;

    public LatexDocumentBuilder()
    {
        _packages = new();
        _content = string.Empty;
    }

    public LatexDocumentBuilder AddPackage(string packageName)
    {
        _packages.Add(packageName);
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
        => new LatexDocument(_packages, _title, _author, _date, _content);
}
