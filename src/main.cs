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

            if (commandArgs.Count > 1)
            {
                switch (command)
                {
                    case "echo":
                        EchoCommand(commandArgs);
                        break;
                    case "type":
                        TypeCommand(commandArgs);
                        break;
                    default:
                        Console.WriteLine($"{command}: command not found");
                        break;
                }
            }
        }
    }

    private static void TypeCommand(List<string> commandArgs)
    {
        string commandArgument = commandArgs.ElementAt(1);

        if (_builtIns.Contains(commandArgument))
        {
            Console.WriteLine($"{commandArgument} is a shell builtin");
        }
        else if (!string.IsNullOrWhiteSpace(commandArgument))
        {
            foreach (string dir in _pathVariable)
            {
                string fullPath = dir + "/" + commandArgument;
                if (File.Exists(fullPath) 
                    && IsExecutable(fullPath))
                {
                    Console.WriteLine($"{commandArgument} is {fullPath}");
                    continue;
                }

                Console.WriteLine($"{commandArgument}: not found");
            }
        }
        else
        {
            Console.WriteLine($"{commandArgument}: not found");
        }

    }

    private static void EchoCommand(List<string> commandArgs)
    {
        string message = string.Join(' ', commandArgs.Skip(1));
        Console.WriteLine(message);
    }

    private static bool IsExecutable(string fullExecutablePath)
    {
        //if (OperatingSystem.IsWindows())
        //{
        //    var firstBytes = new byte[2];
        //    using (var fileStream = File.Open(fullExecutablePath, FileMode.Open))
        //    {
        //        fileStream.Read(firstBytes, 0, 2);
        //    }
        //    return Encoding.UTF8.GetString(firstBytes) == "MZ";
        //}
        //else
        //{
        //}

        try
        {
            // Requires .NET 6+ (available on .NET 9)
            var mode = File.GetUnixFileMode(fullExecutablePath);
            return (mode & UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute) != 0;
        }
        catch
        {
            // If we cannot obtain the mode, conservatively return false
            return false;
        }
    }
}
