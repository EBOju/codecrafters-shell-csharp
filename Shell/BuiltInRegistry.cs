using System.Collections.Generic;
using Shell.Commands;

namespace Shell;

/// <summary>
/// Represents a registry for managing and storing built-in commands available in the shell environment.
/// </summary>
/// <remarks>
/// The <c>BuiltInRegistry</c> class provides a centralized structure to define and access default commands
/// that are built into the shell. These commands include functionalities such as changing directories,
/// printing the current working directory, echoing text output, executing commands, and exiting the shell.
/// </remarks>
public class BuiltInRegistry : IBuiltInRegistry
{
    public List<IBuiltInCommand> BuiltIns { get; } =
    [
        new ChangeDirectoryCommand(),
        new PrintWorkingDirectoryCommand(),
        new TypeCommand(),
        new EchoCommand(),
        new ExitCommand()
    ];
}