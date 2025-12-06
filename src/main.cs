using System.Linq;

class Program
{
    static void Main()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Write("$ ");

            string command = Console.ReadLine();

            string[] commandParts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<string> builtins = ["exit", "echo", "type"];

            switch (commandParts.First())
            {
                case "exit":
                    exit = true;
                    break;
                case "echo":
                    string message = string.Join(' ', commandParts.Skip(1));
                    Console.WriteLine(message);
                    break;
                case "type":
                    string commandFristPart = commandParts.Skip(1).First();
                    if (builtins.Contains(commandFristPart))
                    {
                        Console.WriteLine($"{commandFristPart} is a shell builtin");
                    }
                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;
            }

            if (exit)
                return;
        }
    }
}
