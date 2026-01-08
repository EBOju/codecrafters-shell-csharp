using System.Collections.Generic;

namespace Shell;

/// <summary>
/// Defines methods for handling the execution of system commands and locating executables.
/// </summary>
public interface IExecutableHandler
{
    void StartExecutable(string command, List<string> args);
    string? FindExecutable(string executable);
}