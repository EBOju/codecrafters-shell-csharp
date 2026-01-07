namespace Shell.Commands;

public class TypeCommand : IBuiltInCommand
{
    public string Name => "type";

    public void Execute(List<string> args)
    {
        string commandArgument = args.First();

        if (string.IsNullOrWhiteSpace(commandArgument))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        if (BuiltInRegistry.BuiltIns.Any(builtIn => string.Equals(builtIn.Name, commandArgument)))
        {
            Console.WriteLine($"{commandArgument} is a shell builtin");
            return;
        }

        ExecutableHandler executableHandler = new();
        string? fullPath = executableHandler.FindExecutable(commandArgument);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        Console.WriteLine($"{commandArgument} is {fullPath}");
    }
}
