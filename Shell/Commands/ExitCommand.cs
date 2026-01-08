using System;
using System.Collections.Generic;

namespace Shell.Commands;

public class ExitCommand : IBuiltInCommand
{
    public string Name => "exit";

    /// <summary>
    /// Executes the command with the provided arguments.
    /// </summary>
    /// <param name="args">A list of arguments passed to the command for execution.</param>
    public void Execute(List<string> args)
    {
        Environment.Exit(Environment.ExitCode);
    }
}