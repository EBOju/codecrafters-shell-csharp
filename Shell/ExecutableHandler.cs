using System.Diagnostics;

namespace Shell;

public class ExecutableHandler : IExecutableHandler
{
    private static List<string> _pathVariable = [.. Environment.GetEnvironmentVariable("PATH").Split(':').Where(path => path != "$PATH")];

    public void StartExecutable(string command, List<string> args)
    {
        string? fullPath = FindExecutable(command);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{command}: not found");
            return;
        }

        // Testing
        string commandargmuments = "";

        foreach (string arg in args)
        {
            string test = arg.Replace(" ", string.Empty);
            commandargmuments += test + " ";
        }

        commandargmuments = commandargmuments.Trim();
        //Testing

        Process.Start(new ProcessStartInfo
        {
            FileName = command,
            Arguments = string.Join(' ', args),
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            UseShellExecute = false,
            CreateNoWindow = true,
        })?.WaitForExit();
    }

    public string? FindExecutable(string executable)
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
