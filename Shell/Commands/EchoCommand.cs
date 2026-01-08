using System;
using System.Collections.Generic;

namespace Shell.Commands;

/// <summary>
/// Represents a built-in command for displaying messages in the shell.
/// </summary>
public class EchoCommand : IBuiltInCommand
{
    public string Name => "echo";

    /// <summary>
    /// Executes the command with the provided arguments.
    /// </summary>
    /// <param name="args">A list of arguments passed to the command for execution.</param>
    public void Execute(List<string> args)
    {
        var message = string.Join(' ', args);
        Console.WriteLine(message);
    }
}