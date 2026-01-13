using System;
using System.Collections.Generic;
using System.IO;

namespace Shell.Commands;

/// <summary>
/// Represents the "pwd" command, which prints the current working directory to the console in a shell environment.
/// </summary>
/// <remarks>
/// The "pwd" command is a built-in command used to display the absolute path of the current working directory.
/// This command does not accept or require any arguments for execution.
/// </remarks>
public class PrintWorkingDirectoryCommand : IBuiltInCommand
{
    public string Name => "pwd";

    /// <summary>
    /// Executes the built-in "pwd" command, which prints the current working directory
    /// to the console in a shell environment.
    /// </summary>
    /// <param name="args">
    /// Reserved for potential arguments passed to the command. This implementation
    /// does not require or utilize any arguments.
    /// </param>
    public void Execute(string args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
    }
}