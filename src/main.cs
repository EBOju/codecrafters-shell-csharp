class Program
{
    static void Main()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Write("$ ");

            string command = Console.ReadLine();

            // Checking if the exit command is given
            if (command == "exit")
                exit = true;

            // Printing command not found for any other input
            Console.WriteLine($"{command}: command not found");
        }
    }
}
