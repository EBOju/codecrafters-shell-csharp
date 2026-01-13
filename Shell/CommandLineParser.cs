namespace Shell;

public class CommandLineParser
{
    private readonly string _commandLine;
    private readonly int _maxIndex;
    private int _currentIndex;

    public string Command { get; private set; } = string.Empty;
    public string Arguments { get; private set; } = string.Empty;

    public CommandLineParser(string commandLine)
    {
        _commandLine = commandLine;
        _maxIndex = commandLine.Length - 1;
        _currentIndex = 0;

        Parse();
    }

    /// <summary>
    /// Determines whether the current index is beyond the end of the command line string.
    /// </summary>
    private bool IsAtEndOfString()
    {
        return _currentIndex > _maxIndex;
    }

    /// <summary>
    /// Returns the next character in the command line string.
    /// </summary>
    private char? NextChar()
    {
        if (_currentIndex + 1 > _maxIndex)
            return null;

        return _commandLine[_currentIndex + 1];
    }

    /// <summary>
    /// Returns the current character in the command line string.
    /// </summary>
    private char CurrentChar()
    {
        return _commandLine[_currentIndex];
    }

    /// <summary>
    /// Advances the current index by one character.
    /// </summary>
    private int AdvanceIndex()
    {
        return _currentIndex++;
    }

    /// <summary>
    /// Parses the command line input to extract the command and its arguments.
    /// </summary>
    private void Parse()
    {
        Command = ParseCommand();
        Arguments = ParseArguments();
    }

    /// <summary>
    /// Parses the arguments from the command line input, handling quoted sections
    /// and special characters appropriately.
    /// </summary>
    /// <returns>
    /// A string containing the parsed arguments from the command line.
    /// </returns>
    private string ParseArguments()
    {
        var returnValue = string.Empty;

        var isSingleQuote = false;
        var isDoubleQuote = false;

        while (!IsAtEndOfString())
        {
            var c = CurrentChar();

            switch (c)
            {
                case '"' when !isSingleQuote:
                    if (NextChar() != null && !NextChar().Equals('"'))
                        isDoubleQuote = !isDoubleQuote;
                    break;
                case '\'' when !isDoubleQuote:
                    isSingleQuote = !isSingleQuote;
                    break;
                case '\\' when !isDoubleQuote && !isSingleQuote:
                    if (NextChar() != null)
                        returnValue += NextChar();

                    AdvanceIndex();
                    break;
                case ' ' when !isDoubleQuote && !isSingleQuote:
                    if (NextChar() == ' ')
                        break;
                    returnValue += c;
                    break;
                default:
                    returnValue += c;
                    break;
            }

            AdvanceIndex();
        }

        return returnValue;
    }

    /// <summary>
    /// Parses the command portion of the command line input, stopping at the first space.
    /// </summary>
    /// <returns>
    /// A string representing the command extracted from the command line input.
    /// </returns>
    private string ParseCommand()
    {
        var returnValue = string.Empty;
        var isEnd = false;

        while (!isEnd)
        {
            returnValue += CurrentChar();

            if (NextChar() == ' ')
            {
                AdvanceIndex();
                isEnd = true;
            }

            AdvanceIndex();
        }

        return returnValue;
    }
}