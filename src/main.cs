class Program
{
    static void Main()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Write("$ ");

            string command = Console.ReadLine();
            Console.WriteLine($"{command}: command not found");
        }
    }
}
