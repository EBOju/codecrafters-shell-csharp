using Shell.Commands;

namespace Shell;

public class BuiltInRegistry : IBuiltInRegistry
{
    public List<IBuiltInCommand> BuiltIns { get; } =
    [
        new ChangeDirectoryCommand(),
        new PrintWorkingDirectoryCommand(),
        new TypeCommand(),
        new EchoCommand(),
        new ExitCommand(),
    ];
}