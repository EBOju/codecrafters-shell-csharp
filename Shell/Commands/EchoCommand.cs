using System;
using System.Collections.Generic;

namespace Shell.Commands;

public class EchoCommand : IBuiltInCommand
{
    public string Name => "echo";

    public void Execute(List<string> args)
    {
        var message = string.Join(' ', args);
        Console.WriteLine(message);
    }
}