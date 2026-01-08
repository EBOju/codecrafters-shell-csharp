using System.Collections.Generic;
using Shell.Commands;

namespace Shell;

/// <summary>
/// Registry of built-in commands.
/// </summary>
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