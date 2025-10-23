using System.Net.Sockets;

namespace TriviaGame.Shared
{
    // Player connected to the game
    public class Player
    {
        // Players client
        public TcpClient Client { get; set; }
        // Players Name
        public string Name { get; set; }
        // Current score of player
        public int Score { get; set; }
        // Network stream for sending/receiving messages
        public NetworkStream Stream { get; set; }

        // Creates a new player with TCP connection
        public Player(TcpClient client, string name)
        {
            Client = client;
            Name = name;
            Score = 0;
            Stream = client.GetStream();
        }
    }
}