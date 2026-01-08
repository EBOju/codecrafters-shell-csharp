using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.Commands;

public class TypeCommand : IBuiltInCommand
{
    private static readonly BuiltInRegistry BuiltInRegistry = new();

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

        IExecutableHandler executableHandler = new ExecutableHandler();
        var fullPath = executableHandler.FindExecutable(commandArgument);

        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine($"{commandArgument}: not found");
            return;
        }

        Console.WriteLine($"{commandArgument} is {fullPath}");
    }
}