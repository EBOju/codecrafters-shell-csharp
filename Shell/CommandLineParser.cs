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
    /// Parses the argument portion of the command line input, handling quotes, escape sequences,
    /// and spaces appropriately. This method processes the input to extract the arguments as a single string.
    /// </summary>
    /// <returns>
    /// A string representing the extracted arguments from the command line input.
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
    /// Parses the command line input and returns a list of arguments, handling quoted and escaped characters appropriately.
    /// </summary>
    /// <returns>
    /// A list of strings representing individual arguments extracted from the command line input.
    /// </returns>
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
                case '"' when !isSingleQuote:
                    if (isDoubleQuote) FlushArgument();
                    isDoubleQuote = !isDoubleQuote;
                    break;

                case '\'' when !isDoubleQuote:
                    if (isSingleQuote) FlushArgument();
                    isSingleQuote = !isSingleQuote;
                    break;
                case '\\' when (isDoubleQuote && _doubleQuoteSpecialChars.Contains(nextChar)) ||
                               (!isDoubleQuote && !isSingleQuote):
                    if (nextChar != null)
                    {
                        currentArgument += nextChar;
                        AdvanceIndex(); // Skip the escaped char so it's not processed by the loop
                    }
                    break;
                case ' ' when !isDoubleQuote && !isSingleQuote:
                    FlushArgument();
                    break;
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