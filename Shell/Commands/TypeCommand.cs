using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.Commands;

/// <summary>
/// Represents the "type" command, which identifies whether a given
/// command is a shell built-in or an executable and displays its path
/// if it is an executable.
/// </summary>
public class TypeCommand : IBuiltInCommand
{
    private static readonly BuiltInRegistry BuiltInRegistry = new();

    public string Name => "type";

    /// <summary>
    /// Executes the "type" command, which determines if a given argument corresponds to a
    /// shell built-in command or an executable, and outputs its type or location.
    /// </summary>
    /// <param name="args">
    /// A list of command-line arguments where the first argument represents the name
    /// of the command or executable to check.
    /// </param>
    public void Execute(List<string> args)
    {
        if (args.Count == 0) return;

        string commandArgument = args[0];

        if (string.IsNullOrWhiteSpace(commandArgument))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        if (BuiltInRegistry.IsBuiltIn(commandArgument))
        {
            Console.WriteLine($"{commandArgument} is a shell builtin");
            return;
        }

        var executableHandler = new ExecutableHandler();
        var fullPath = executableHandler.FindExecutable(commandArgument);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        Console.WriteLine($"{commandArgument} is {fullPath}");
    }
}