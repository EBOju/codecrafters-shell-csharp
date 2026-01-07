namespace Shell;

public interface IBuiltInCommand
{
    string Name { get; }
    void Execute(List<string> args);
}