using System.Collections.Generic;

namespace Shell;

public class CommandLineParser
{
    private readonly string _commandLine;
    private readonly int _maxIndex;
    private int _currentIndex;

    public string Command { get; private set; } = string.Empty;
    public string Arguments { get; private set; } = string.Empty;

    public List<string> ArgumentsList { get; private set; } = new();

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
    /// Resets the current index to the beginning of the command line string.
    /// </summary>
    private void ResetIndex()
    {
        _currentIndex = Command.Length > 0 ? Command.Length + 1 : 0;
    }

    /// <summary>
    /// Parses the command line input to extract the command and its arguments.
    /// </summary>
    private void Parse()
    {
        Command = ParseCommand();
        Arguments = ParseArguments();
        ArgumentsList = ParseArgumentsToList();
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
        ResetIndex();

        var argumentString = string.Empty;

        var isSingleQuote = false;
        var isDoubleQuote = false;

        if (Command.Length == _commandLine.Length)
            return argumentString;

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
                    if (NextChar() != null && !NextChar().Equals('\''))
                        isSingleQuote = !isSingleQuote;
                    break;
                case '\\' when isDoubleQuote:
                case '\\' when !isDoubleQuote && !isSingleQuote:
                    if (NextChar() != null)
                        argumentString += NextChar();
                    AdvanceIndex();
                    break;
                case ' ' when !isDoubleQuote && !isSingleQuote:
                    if (NextChar() == ' ')
                        break;
                    argumentString += c;
                    break;
                default:
                    argumentString += c;
                    break;
            }

            AdvanceIndex();
        }

        return argumentString;
    }

    /// <summary>
    /// Parses the arguments from the command line string into a list of individual arguments,
    /// taking into account quoted substrings and escape sequences.
    /// </summary>
    /// <returns>A list of string arguments extracted from the command line string.</returns>
    private List<string> ParseArgumentsToList()
    {
        ResetIndex();

        var argumentList = new List<string>();

        var currentArgument = string.Empty;

        var isSingleQuote = false;
        var isDoubleQuote = false;

        if (Command.Length == _commandLine.Length)
            return argumentList;

        while (!IsAtEndOfString())
        {
            var c = CurrentChar();

            switch (c)
            {
                case '"' when !isSingleQuote:

                    if (NextChar() == null)
                    {
                        argumentList.Add(currentArgument);
                        currentArgument = string.Empty;
                        break;
                    }

                    if (!NextChar().Equals('"'))
                    {
                        if (isDoubleQuote)
                        {
                            argumentList.Add(currentArgument);
                            currentArgument = string.Empty;
                            AdvanceIndex();
                        }

                        isDoubleQuote = !isDoubleQuote;
                    }

                    break;
                case '\'' when !isDoubleQuote:
                    if (NextChar() == null)
                    {
                        argumentList.Add(currentArgument);
                        currentArgument = string.Empty;
                        break;
                    }

                    if (!NextChar().Equals('\''))
                    {
                        if (isSingleQuote)
                        {
                            argumentList.Add(currentArgument);
                            currentArgument = string.Empty;
                            AdvanceIndex();
                        }

                        isSingleQuote = !isSingleQuote;
                    }

                    break;
                case '\\' when !isDoubleQuote && !isSingleQuote:
                    if (NextChar() != null)
                        currentArgument += NextChar();
                    AdvanceIndex();
                    break;
                case ' ' when !isDoubleQuote && !isSingleQuote:
                    argumentList.Add(currentArgument);
                    currentArgument = string.Empty;

                    break;
                default:
                    currentArgument += c;

                    if (NextChar() == null)
                    {
                        argumentList.Add(currentArgument);
                        currentArgument = string.Empty;
                        break;
                    }

                    break;
            }

            AdvanceIndex();
        }

        return argumentList;
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

            if (NextChar() == null)
                break;

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