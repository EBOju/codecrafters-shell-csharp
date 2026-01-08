using System.Collections.Generic;
using Shell.Commands;

namespace Shell;

public class BuiltInRegistry : IBuiltInRegistry
{
    private readonly Dictionary<string, IBuiltInCommand> _builtIns;

    public BuiltInRegistry()
    {
        // initialize built-in commands
        var commands = new List<IBuiltInCommand>
        {
            new ChangeDirectoryCommand(),
            new PrintWorkingDirectoryCommand(),
            new TypeCommand(),
            new EchoCommand(),
            new ExitCommand()
        };

        // build dictionary of built-in commands
        _builtIns = commands.ToDictionary(command => command.Name);
    }

    public IEnumerable<IBuiltInCommand> BuiltIns => _builtIns.Values;

    /// <summary>
    /// Determines whether a command name corresponds to a built-in command in the registry.
    /// </summary>
    /// <param name="commandName">The name of the command to check.</param>
    /// <returns><c>true</c> if the command is a built-in command; otherwise, <c>false</c>.</returns>
    public bool IsBuiltIn(string commandName)
    {
        return !string.IsNullOrEmpty(commandName) && _builtIns.ContainsKey(commandName);
    }

    /// <summary>
    /// Retrieves a built-in command by its name from the registry.
    /// </summary>
    /// <param name="commandName">The name of the command to retrieve.</param>
    /// <returns>The <see cref="IBuiltInCommand"/> instance if found; otherwise, null.</returns>
    public IBuiltInCommand? GetCommand(string commandName)
    {
        return string.IsNullOrEmpty(commandName) ? null : _builtIns.GetValueOrDefault(commandName);
    }
}