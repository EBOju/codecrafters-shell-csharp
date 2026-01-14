using System.Collections.Generic;

namespace Shell;

public class CommandLineParser
{
    private readonly string _commandLine;
    private readonly int _maxIndex;
    private int _currentIndex;
    private List<char?> _doubleQuoteSpecialChars = ['"', '\\'];

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
            var nextChar = NextChar();

            switch (c)
            {
                case '"' when !isSingleQuote:
                    isDoubleQuote = !isDoubleQuote;
                    break;
                case '\'' when !isDoubleQuote:
                    isSingleQuote = !isSingleQuote;
                    break;
                case '\\' when (isDoubleQuote && _doubleQuoteSpecialChars.Contains(nextChar)) ||
                               (!isDoubleQuote && !isSingleQuote):
                    if (nextChar != null)
                        argumentString += nextChar;
                    AdvanceIndex();
                    break;
                case ' ' when !isDoubleQuote && !isSingleQuote:
                    if (nextChar != ' ') argumentString += c;
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
            var nextChar = NextChar();

            switch (c)
            {
                // 1. Handle Quotes: Toggle state and flush if closing
                case '"' when !isSingleQuote:
                    if (isDoubleQuote) FlushArgument();
                    isDoubleQuote = !isDoubleQuote;
                    break;

                case '\'' when !isDoubleQuote:
                    if (isSingleQuote) FlushArgument();
                    isSingleQuote = !isSingleQuote;
                    break;

                // 2. Handle Escapes: Consume the NEXT character immediately
                case '\\' when (isDoubleQuote && _doubleQuoteSpecialChars.Contains(nextChar)) ||
                               (!isDoubleQuote && !isSingleQuote):
                    if (nextChar != null)
                    {
                        currentArgument += nextChar;
                        AdvanceIndex(); // Skip the escaped char so it's not processed by the loop
                    }

                    break;

                // 3. Handle Delimiters
                case ' ' when !isDoubleQuote && !isSingleQuote:
                    FlushArgument();
                    break;

                // 4. Everything else
                default:
                    currentArgument += c;
                    break;
            }

            AdvanceIndex();
        }

        // Flush the last argument
        FlushArgument();

        return argumentList;

        void FlushArgument()
        {
            if (string.IsNullOrEmpty(currentArgument)) return;

            argumentList.Add(currentArgument);
            currentArgument = string.Empty;
        }
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