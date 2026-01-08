using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Shell;

/// <summary>
/// Executable handler.
/// </summary>
public class ExecutableHandler : IExecutableHandler
{
    private static readonly List<string> PathVariable =
        [.. Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator)];

    /// <summary>
    /// Starts an executable.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="args"></param>
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

    public string? FindExecutable(string executable)
    {
        foreach (string dir in PathVariable)
        {
            var fullPath = dir + Path.DirectorySeparatorChar + executable;

            if (File.Exists(fullPath) && IsExecutable(fullPath)) return fullPath;
        }
        
        return null;
    }

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