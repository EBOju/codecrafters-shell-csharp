using System;
using System.Collections.Generic;
using System.IO;

namespace Shell.Commands;

/// <summary>
/// Represents the "cd" command, which changes the current working directory in the shell environment.
/// </summary>
public class ChangeDirectoryCommand : IBuiltInCommand
{
    public string Name => "cd";

    /// <summary>
    /// Executes the "cd" command, changing the current working directory
    /// to the specified destination directory.
    /// </summary>
    /// <param name="commandArgs">The target directory path to change to.
    /// If "~" is provided, it will navigate to the home directory. If the
    /// directory does not exist, an error message will be displayed.</param>
    public void Execute(string commandArgs)
    {
        var destinationDirectory = commandArgs;

        if (destinationDirectory == "~")
            destinationDirectory = Environment.GetEnvironmentVariable("HOME") ?? string.Empty;

        if (!Directory.Exists(destinationDirectory))
        {
            Console.WriteLine($"cd: {destinationDirectory}: No such file or directory");
            return;
        }

        Directory.SetCurrentDirectory(destinationDirectory);
    }
}