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

            if(string.IsNullOrWhiteSpace(commandString))
                continue;

            // split command string into command and arguments
            List<string> commandArgs = [.. commandString.Split(" ").Skip(1)];
            string command = commandString.Split(" ").First();

            if (string.IsNullOrWhiteSpace(command) || command == "exit")
                break;

            if (BuiltInRegistry.BuiltIns.Any(builtIn => builtIn.Name == command))
            {
                try
                {
                    IBuiltInCommand builtInCommand = BuiltInRegistry.BuiltIns.First(builtIn => builtIn.Name == command);
                    builtInCommand.Execute(commandArgs);
                }
                catch (Exception)
                {
                    Console.WriteLine($"{command}: command not found");
                }
            }
            else
            {
                _executableHandler.StartExecutable(command, commandArgs);
            }
        }
    }

}
