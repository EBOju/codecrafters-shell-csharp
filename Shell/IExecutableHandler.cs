using System.Collections.Generic;

namespace Shell;

public interface IExecutableHandler
{
    void StartExecutable(string command, List<string> args);
    string? FindExecutable(string executable);
}