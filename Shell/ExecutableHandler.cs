using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Shell;

/// <summary>
/// Handles operations related to executing system commands and locating executables in the system's PATH.
/// </summary>
public class ExecutableHandler : IExecutableHandler
{
    private static readonly List<string> PathVariable =
        [.. Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator)];

    /// <summary>
    /// Starts an executable process with the specified command and arguments.
    /// </summary>
    /// <param name="command">The name or path of the executable to be run.</param>
    /// <param name="args">A list of arguments to be passed to the executable.</param>
    public void StartExecutable(string command, List<string> args)
    {
        var fullPath = FindExecutable(command);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{command}: not found");
            return;
        }

        Process.Start(new ProcessStartInfo(command, args))?.WaitForExit();
    }

    /// <summary>
    /// Searches for the specified executable in the system's PATH environment variable and verifies its existence.
    /// </summary>
    /// <param name="executable">The name of the executable to locate.</param>
    /// <returns>The full path to the executable if found and valid; otherwise, null.</returns>
    public string? FindExecutable(string executable)
    {
        foreach (string dir in PathVariable)
        {
            var fullPath = dir + Path.DirectorySeparatorChar + executable;

            if (File.Exists(fullPath) && IsExecutable(fullPath)) return fullPath;
        }
        
        return null;
    }

    /// <summary>
    /// Determines whether the specified file is an executable based on the file extension,
    /// file header, or permissions, depending on the operating system.
    /// </summary>
    /// <param name="fullExecutablePath">The full file path of the executable to check.</param>
    /// <returns>True if the file is an executable; otherwise, false.</returns>
    private static bool IsExecutable(string fullExecutablePath)
    {
        if (OperatingSystem.IsWindows())
        {
            // Check PATHEXT
            string ext = Path.GetExtension(fullExecutablePath);
            var pathext = Environment.GetEnvironmentVariable("PATHEXT")
                          ?? ".COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC";
            var allowed = new HashSet<string>(
                pathext.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                StringComparer.OrdinalIgnoreCase);

            if (allowed.Contains(ext)) return true;

            // Fallback: basic PE header check (MZ + PE\0\0) for native binaries
            try
            {
                using var fs = File.OpenRead(fullExecutablePath);
                Span<byte> mz = stackalloc byte[2];
                if (fs.Read(mz) >= 2 && mz[0] == (byte)'M' && mz[1] == (byte)'Z')
                {
                    // e_lfanew is a 4-byte little-endian value at offset 0x3C
                    fs.Seek(0x3C, SeekOrigin.Begin);
                    Span<byte> e_lfanewBytes = stackalloc byte[4];
                    if (fs.Read(e_lfanewBytes) == 4)
                    {
                        int e_lfanew = BitConverter.ToInt32(e_lfanewBytes);
                        if (e_lfanew > 0)
                        {
                            fs.Seek(e_lfanew, SeekOrigin.Begin);
                            Span<byte> pe = stackalloc byte[4];
                            if (fs.Read(pe) == 4 && pe[0] == (byte)'P' && pe[1] == (byte)'E' && pe[2] == 0 &&
                                pe[3] == 0) return true;
                        }
                    }
                }
            }
            catch
            {
                // conservative: if we can't read header, treat as non-executable
                return false;
            }
        }

        try
        {
            // Requires .NET 6+ (available on .NET 9)
            return (File.GetUnixFileMode(fullExecutablePath) &
                    (UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute)) != 0;
        }
        catch
        {
            // If we cannot get the mode, conservatively return false
            return false;
        }
    }
}