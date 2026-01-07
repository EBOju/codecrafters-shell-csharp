namespace Shell.Commands;

public class PrintWorkingDirectoryCommand : IBuiltInCommand
{
    public string Name => "pwd";

    public void Execute(List<string> args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
    }
}
