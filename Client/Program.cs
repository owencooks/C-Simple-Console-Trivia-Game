using System.Net.Sockets;
using System.Text;

namespace TriviaGame.Client
{
    // Client app that connects to the server
    // which handles sending user input and server messages
    class TriviaClient
    {
        // Connections
        private TcpClient? client;
        private NetworkStream? stream;
        private Thread? receiveThread;
        private bool isConnected = false;

        // Connection to the trivia game server
        public void Connect(string host, int port)
        {
            try
            {
                // Creating TCP connection
                client = new TcpClient(host, port);
                stream = client.GetStream();
                isConnected = true;

                Console.WriteLine($"Connected to server at {host}:{port}");
                Console.WriteLine();

                
                receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();
                HandleUserInput();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to server: {ex.Message}");
            }
        }

        // Looking for messages from the server
        private void ReceiveMessages()
        {
            if (stream == null) return;

            try
            {
                byte[] buffer = new byte[2048];

                while (isConnected)
                {
                    int bytes = stream.Read(buffer, 0, buffer.Length);

                    if (bytes == 0)
                    {
                        Console.WriteLine("\nDisconnected from server.");
                        isConnected = false;
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytes);
                    Console.Write(message);
                }
            }
            catch (Exception)
            {
                if (isConnected)
                {
                    Console.WriteLine("\nConnection lost.");
                    isConnected = false;
                }
            }
        }

        // Gets user input from console and sends it to the server
        private void HandleUserInput()
        {
            try
            {
                while (isConnected)
                {
                    string input = Console.ReadLine()!;

                    if (!isConnected)
                        break;

                    if (string.IsNullOrEmpty(input))
                        continue;

                    SendMessage(input);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        // Sends a message to the server
        private void SendMessage(string message)
        {
            if (stream == null) return;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                stream.Write(data, 0, data.Length);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to send message.");
                isConnected = false;
            }
        }

        // Disconnects from the server
        private void Disconnect()
        {
            isConnected = false;

            if (stream != null)
                stream.Close();

            if (client != null)
                client.Close();

            Console.WriteLine("Disconnected.");
        }
    }

    // Game client's connection
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================================");
            Console.WriteLine("   TRIVIA GAME CLIENT");
            Console.WriteLine("=================================");
            Console.WriteLine();

            string host = "localhost";
            int port = 12706; 

            if (args.Length > 0)
                host = args[0];

            if (args.Length > 1 && int.TryParse(args[1], out int customPort))
                port = customPort;

            Console.WriteLine($"Connecting to {host}:{port}...");
            Console.WriteLine();

            TriviaClient client = new TriviaClient();
            client.Connect(host, port);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}