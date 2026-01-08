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
    /// Changes the current working directory of the shell environment.
    /// </summary>
    /// <param name="commandArgs">A list of arguments where the first element specifies the target directory. Special value "~" resolves to the user's home directory.</param>
    public void Execute(List<string> commandArgs)
    {
        var destinationDirectory = string.Join(' ', commandArgs);

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