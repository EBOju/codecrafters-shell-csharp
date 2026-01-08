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
    /// <summary>
    /// Gets a collection of built-in commands available in the shell environment.
    /// </summary>
    /// <remarks>
    /// This property provides access to all the commands registered as built-ins within the shell.
    /// Each command in the collection implements the <see cref="IBuiltInCommand"/> interface
    /// and represents a specific functionality supported by the shell.
    /// </remarks>
    IEnumerable<IBuiltInCommand> BuiltIns { get; }

    /// <summary>
    /// Determines whether a given command name corresponds to a built-in command.
    /// </summary>
    /// <param name="commandName">The name of the command to check.</param>
    /// <returns>
    /// True if the command is a built-in command; otherwise, false.
    /// </returns>
    public bool IsBuiltIn(string commandName);

    /// <summary>
    /// Retrieves the built-in command associated with the specified command name.
    /// </summary>
    /// <param name="commandName">The name of the built-in command to retrieve.</param>
    /// <returns>
    /// The built-in command associated with the specified command name if found;
    /// otherwise, null.
    /// </returns>
    public IBuiltInCommand? GetCommand(string commandName);
}