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
    /// Executes the "type" command. Determines whether a specified command
    /// is a shell built-in or an executable and provides its path if it is
    /// an executable.
    /// </summary>
    /// <param name="args">The name of the command to identify.</param>
    public void Execute(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            Console.WriteLine($"{args}: not found");
            return;
        }

        if (BuiltInRegistry.IsBuiltIn(args))
        {
            Console.WriteLine($"{args} is a shell builtin");
            return;
        }

        var executableHandler = new ExecutableHandler();
        var fullPath = executableHandler.FindExecutable(args);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{args}: not found");
            return;
        }

        Console.WriteLine($"{args} is {fullPath}");
    }
}