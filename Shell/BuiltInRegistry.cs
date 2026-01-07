using Shell.Command;

namespace Shell;

public class BuiltInRegistry
{
    public static readonly List<IBuiltInCommand> BuiltIns =
    [
        new ChangeDirectoryCommand(),
        new PrintWorkingDirectoryCommand(),
        new TypeCommand(),
        new EchoCommand(),
    ];
}
