using Shell;
using Shell.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

class Program
{
    private readonly static ExecutableHandler _executableHandler = new();

    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");

            // get full command string
            string? commandString = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(commandString))
                continue;

            // split command string into command and arguments
            List<string> input = SplitInput(commandString);

            // extract command and arguments
            string command = input.First();
            List<string> args = input.GetRange(1, input.Count - 1);

            if (string.IsNullOrWhiteSpace(command))
                break;

            if (BuiltInRegistry.BuiltIns.Any(builtIn => builtIn.Name == command))
            {
                try
                {
                    IBuiltInCommand builtInCommand = BuiltInRegistry.BuiltIns.First(builtIn => builtIn.Name == command);
                    builtInCommand.Execute(args);
                }
                catch (Exception)
                {
                    Console.WriteLine($"{command}: command not found");
                }
            }
            else
            {
                _executableHandler.StartExecutable(command, args);
            }
        }
    }

    private static List<string> SplitInput(string commandString)
    {
        List<string> args = [];

        string currentArg = "";
        bool inSingleQuote = false;
        bool inDoubleQuote = false;

        // iterate through each character in the command string
        for (int i = 0; i < commandString.Length; i++)
        {
            if (commandString[i].Equals('"'))
            {
                // toggle inDoubleQuote flag
                inDoubleQuote = !inDoubleQuote;
            }
            else if (commandString[i].Equals('\'') && !inDoubleQuote)
            {
                // toggle inSingleQuote flag
                inSingleQuote = !inSingleQuote;
            }
            else
            {
                // add character to current argument
                currentArg += commandString[i];
            }

            // if we hit a space and we're not in single quotes, or we're at the end of the string, finalize the current argument
            if (i == commandString.Length - 1 || (commandString[i] == ' ' && !inSingleQuote && !inDoubleQuote))
            {
                args.Add(currentArg.Trim());
                currentArg = string.Empty;
            }
        }

        args.RemoveAll(arg => string.IsNullOrWhiteSpace(arg));

        return args;
    }
}
