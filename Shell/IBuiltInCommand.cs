using System.Collections.Generic;

namespace Shell;

/// <summary>
/// Defines the interface for a built-in command within the shell environment.
/// </summary>
/// <remarks>
/// Implementations of this interface correspond to individual built-in commands
/// provided by the shell, such as "echo", "exit", or "pwd". Each command is identified
/// by a unique name and implements functionality to execute the command logic.
/// </remarks>
public interface IBuiltInCommand
{
    string Name { get; }
    void Execute(List<string> args);
}