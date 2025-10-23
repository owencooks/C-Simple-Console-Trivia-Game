namespace TriviaGame.Server
{
    class Program
    {
        // Displays the UI for the server
        static void Main(string[] args)
        {
            Console.WriteLine("=================================");
            Console.WriteLine("   TRIVIA GAME SERVER");
            Console.WriteLine("=================================");
            Console.WriteLine();

            int port = 12706;

            if (args.Length > 0 && int.TryParse(args[0], out int customPort))
            {
                port = customPort;
            }

            TriviaServer server = new TriviaServer(port);
            Console.WriteLine($"Starting server on port {port}...");
            Console.WriteLine("Press Ctrl+C to stop the server");
            Console.WriteLine();

            server.Start();
        }
    }
}