using System;
using System.Collections.Generic;
using System.IO;

namespace Shell.Commands;

public class ChangeDirectoryCommand : IBuiltInCommand
{
    public string Name => "cd";

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