using System.Text;

namespace Jenbot.MathsModule.LatexCompilation;

public class LatexCommand
{
    private string _commandName;
    private Dictionary<string, string> _optionalArguments;
    private string _argument;

    public LatexCommand(string commandName, string argument)
    {
        _commandName = commandName;
        _argument = argument;
        _optionalArguments = new();
    }
    
    public LatexCommand(string commandName, string argument, IDictionary<string, string> optionalArgs)
    {
        _commandName = commandName;
        _argument = argument;
        _optionalArguments = optionalArgs.ToDictionary<string, string>();
    }

    public void AddOptionalArgument(string argumentName, object? value=null)
    {
        var valueString = value != null ? value.ToString() : "";
        
        if (_optionalArguments.ContainsKey(argumentName))
        {
            _optionalArguments[argumentName] = valueString;
            return;
        }

        _optionalArguments.Add(argumentName, valueString);
    }

    public string GetLatexCode()
    {
        var builder = new StringBuilder();

        builder.Append($"\\{_commandName}");

        if (_optionalArguments.Count > 0)
            builder.Append($"[{BuildOptionalArgsString()}]");

        builder.Append("{");
        builder.Append(_argument);
        builder.Append("}");

        return builder.ToString();
    }

    private string BuildOptionalArgsString()
    {
        var builder = new StringBuilder();
        var parts = new List<string>();

        foreach (var pair in _optionalArguments)
        {
            builder.Clear();
            builder.Append(pair.Key);

            if (!string.IsNullOrEmpty(pair.Value))
                builder.Append($"={pair.Value}");

            parts.Add(builder.ToString());
        }

        return string.Join(",", parts);
    }

    public string GetPrincipleArgument()
        => _argument;
}
