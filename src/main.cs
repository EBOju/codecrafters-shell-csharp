using Shell;
using Shell.Command;
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

            if (string.IsNullOrWhiteSpace(command) || command == "exit")
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
                foreach (var arg in args)
                {
                    _executableHandler.StartExecutable(command, arg.Split(' ').ToList());
                }
            }
        }
    }

    private static List<string> SplitInput(string commandString)
    {
        List<string> args = [];

        string currentArg = "";
        bool inSingleQuote = false;

        // quick check for no quotes
        if (!commandString.Contains('\''))
        {
            return commandString.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        // iterate through each character in the command string
        for (int i = 0; i < commandString.Length; i++)
        {
            // check for single quotes
            if (commandString[i] == '\'')
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
            if (i == commandString.Length - 1 || (commandString[i] == ' ' && !inSingleQuote))
            {
                args.Add(currentArg.Trim());
                currentArg = "";
            }
        }

        return args;
    }
}
