using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

class Program
{
    private static readonly List<string> _builtIns = ["exit", "echo", "type", "pwd", "cd"];
    private static List<string> _pathVariable = [.. Environment.GetEnvironmentVariable("PATH").Split(':').Where(path => path != "$PATH")];
    //private static List<string> _pathVariable = [.. "/usr/bin:/usr/local/bin:$PATH".Split(':').Where(path => path != "$PATH")];

    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");

            // get full command string
            string commandString = Console.ReadLine();

            // split command string into command and arguments
            List<string> commandArgs = [.. commandString.Split(" ").Skip(1)];
            string command = commandString.Split(" ").First();

            if (string.IsNullOrWhiteSpace(command) || command == "exit")
                break;

            if (_builtIns.Contains(command))
            {
                switch (command)
                {
                    case "echo":
                        EchoCommand(commandArgs);
                        break;
                    case "type":
                        TypeCommand(commandArgs);
                        break;
                    case "pwd":
                        PrintWorkingDirectoryCommand(); 
                        break;
                    case "cd":
                        ChangeDirectoryCommand(commandArgs);
                        break;
                    default:
                        Console.WriteLine($"{command}: command not found");
                        break;
                }
            }
            else
            {
                StartExecutable(command, commandArgs);
            }
        }
    }

    private static void ChangeDirectoryCommand(List<string> commandArgs)
    {
        string destinationDirectory = string.Join(' ', commandArgs);

        if (!Directory.Exists(destinationDirectory))
        {
            Console.WriteLine($"cd: {destinationDirectory}: No such file or directory");
            return;
        }
        
        Directory.SetCurrentDirectory(destinationDirectory);
    }

    private static void PrintWorkingDirectoryCommand()
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
    }

    private static void StartExecutable(string command, List<string> commandArgs)
    {
        string? fullPath = FindExecutable(command);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{command}: not found");
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = command,
            Arguments = string.Join(' ', commandArgs),
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            UseShellExecute = false,
            CreateNoWindow = true,
        })?.WaitForExit();
    }

    private static void TypeCommand(List<string> commandArgs)
    {
        string commandArgument = commandArgs.First();

        if (_builtIns.Contains(commandArgument))
        {
            Console.WriteLine($"{commandArgument} is a shell builtin");
            return;
        }

        if (string.IsNullOrWhiteSpace(commandArgument))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        string? fullPath = FindExecutable(commandArgument);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        Console.WriteLine($"{commandArgument} is {fullPath}");
    }

    private static string? FindExecutable(string executable)
    {
        foreach (string dir in _pathVariable)
        {
            string fullPath = dir + "/" + executable;

            if (File.Exists(fullPath) && IsExecutable(fullPath))
            {
                return fullPath;
            }
        }

        return null;
    }

    private static void EchoCommand(List<string> commandArgs)
    {
        string message = string.Join(' ', commandArgs);
        Console.WriteLine(message);
    }

    private static bool IsExecutable(string fullExecutablePath)
    {
        if (OperatingSystem.IsWindows())
        {
            return false;
        }

        try
        {
            // Requires .NET 6+ (available on .NET 9)
            return ((File.GetUnixFileMode(fullExecutablePath)) & (UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute)) != 0;
        }
        catch
        {
            // If we cannot obtain the mode, conservatively return false
            return false;
        }
    }
}
