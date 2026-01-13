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
    /// Executes the command using the provided arguments.
    /// </summary>
    /// <param name="args">The arguments to be processed by the command.</param>
    public void Execute(string args)
    {
        var message = args;
        Console.WriteLine(message);
    }
}