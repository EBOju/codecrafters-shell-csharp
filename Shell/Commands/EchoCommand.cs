namespace Shell.Command;

public class EchoCommand : IBuiltInCommand
{
    public string Name => "echo";

    public void Execute(List<string> args)
    {
        string message = string.Join(' ', args);
        Console.WriteLine(message);
    }
}
