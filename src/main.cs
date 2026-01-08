using Shell;
using Shell.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

class Program
{
    private static readonly ExecutableHandler ExecutableHandler = new();
    private static readonly BuiltInRegistry BuiltInRegistry = new();

    private static void Main()
    {
        while (true)
        {
            Console.Write("$ ");

            // get full command string
            var userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
                continue;

            // split command string into command and arguments
            var inputList = SplitInput(userInput);

            // extract command and arguments
            var commandString = inputList.First();
            var args = inputList.GetRange(1, inputList.Count - 1);

            if (string.IsNullOrWhiteSpace(commandString))
                break;
            
            if (BuiltInRegistry.BuiltIns.Any(builtIn => builtIn.Name == commandString))
            {
                try
                {
                    var builtInCommand = BuiltInRegistry.BuiltIns.First(builtIn => builtIn.Name == commandString);
                    builtInCommand.Execute(args);
                }
                catch (Exception)
                {
                    Console.WriteLine($"{commandString}: command not found");
                }
            }
            else
            {
                ExecutableHandler.StartExecutable(commandString, args);
            }
        }
    }

    private static List<string> SplitInput(string commandString)
    {
        List<string> args = [];

        var currentArg = "";
        var inSingleQuote = false;
        var inDoubleQuote = false;

        // iterate through each character in the command string
        for (var index = 0; index < commandString.Length; index++)
        {
            // handle quotes
            if (!commandString[index].Equals('"'))
            {
                // handle single quotes
                if (commandString[index].Equals('\'') && !inDoubleQuote)
                {
                    // toggle inSingleQuote flag
                    inSingleQuote = !inSingleQuote;
                }
                else
                {
                    // add character to the current argument
                    currentArg += commandString[index];
                }
            }
            else
            {
                // toggle inDoubleQuote flag
                inDoubleQuote = !inDoubleQuote;
            }

            // check for argument boundary
            if (index != commandString.Length - 1 && (commandString[index] != ' ' || inSingleQuote || inDoubleQuote)) continue;

            // end of argument reached, add to an args list
            args.Add(currentArg.Trim());
            currentArg = string.Empty;
        }

        // remove empty arguments
        args.RemoveAll(string.IsNullOrWhiteSpace);

        return args;
    }
}