using System.Collections.Generic;

namespace Shell;

/// <summary>
/// Represents a registry for built-in commands in the shell environment.
/// </summary>
/// <remarks>
/// This interface provides a structure to manage and store the built-in commands
/// available within the shell. It includes a collection of commands that
/// implement the functionalities provided by the shell, such as changing
/// directories, printing the working directory, and more.
/// </remarks>
public interface IBuiltInRegistry
{
    List<IBuiltInCommand> BuiltIns { get; }
}