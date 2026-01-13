using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shell;

internal abstract class Program
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

            var parser = new CommandLineParser(userInput);

            // check if the command is a built-in command
            if (string.IsNullOrWhiteSpace(parser.Command))
                break;

            // execute the command
            var builtIn = BuiltInRegistry.GetCommand(parser.Command);
            if (builtIn != null)
            {
                try
                {
                    builtIn.Execute(parser.Arguments);
                }
                catch (Exception)
                {
                    Console.WriteLine($"{parser.Command}: command not found");
                }
            }
            else if (ExecutableHandler.FindExecutable(parser.Command) != null &&
                     !string.IsNullOrWhiteSpace(parser.Arguments))
            {
                ExecutableHandler.StartExecutable(parser.Command, parser.ArgumentsList);
            }
            else
            {
                Console.WriteLine($"{parser.Command}: command not found");
            }
        }
    }
}