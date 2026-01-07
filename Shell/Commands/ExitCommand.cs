namespace Shell.Commands;

public class ExitCommand : IBuiltInCommand
{
    public string Name => "exit";

    public void Execute(List<string> args)
    {
        Environment.Exit(Environment.ExitCode);
    }
}
