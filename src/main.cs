using System;
using System.Linq;
using System.Text;

class Program
{
    private static readonly List<string> _builtIns = ["exit", "echo", "type"];
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
            List<string> commandArgs = [.. commandString.Split(" ")];
            string command = commandArgs.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(command) || command == "exit")
                break;

            switch (command)
            {
                case "echo":
                    EchoCommand(commandArgs);
                    break;
                case "type":
                    TypeCommand(commandArgs);
                    break;
                default:

                    CheckExecutable();

                    Console.WriteLine($"{command}: command not found");
                    break;
            }
        }
    }

    private static void CheckExecutable()
    {
        // Comment
    }

    private static void TypeCommand(List<string> commandArgs)
    {
        string commandArgument = commandArgs.ElementAt(1);

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

        bool notfound = true;

        foreach (string dir in _pathVariable)
        {
            string fullPath = dir + "/" + commandArgument;

            if (File.Exists(fullPath) && IsExecutable(fullPath))
            {
                Console.WriteLine($"{commandArgument} is {fullPath}");
                notfound = false;
                break;
            }
        }

        if (notfound)
            Console.WriteLine($"{commandArgument}: not found");
    }

    private static void EchoCommand(List<string> commandArgs)
    {
        string message = string.Join(' ', commandArgs.Skip(1));
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
