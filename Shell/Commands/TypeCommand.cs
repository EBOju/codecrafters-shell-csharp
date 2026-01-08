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
    /// Executes the "type" command, which determines if a provided argument corresponds
    /// to a shell built-in command or an executable file. If it is a shell built-in,
    /// it displays a message indicating this. If it is an executable, the full path to
    /// the executable is displayed. If no match is found, a "not found" message is issued.
    /// </summary>
    /// <param name="args">A list of arguments where the first element is the name of the
    /// command to be identified. If the list is empty or the name is invalid, a "not found"
    /// message is displayed.</param>
    public void Execute(List<string> args)
    {
        string commandArgument = args.First();

        if (string.IsNullOrWhiteSpace(commandArgument))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        if (BuiltInRegistry.BuiltIns.Any(builtIn => string.Equals(builtIn.Name, commandArgument)))
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