using System;
using System.Collections.Generic;

namespace Shell.Commands;

public class ExitCommand : IBuiltInCommand
{
    public string Name => "exit";

    /// <summary>
    /// Exits the shell application.
    /// </summary>
    public void Execute(string args)
    {
        Environment.Exit(Environment.ExitCode);
    }
}