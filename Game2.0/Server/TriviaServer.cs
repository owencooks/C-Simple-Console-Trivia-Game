using System.Net;
using System.Net.Sockets;
using System.Text;
using TriviaGame.Shared;

namespace TriviaGame.Server
{
    // This is the main server that is responsible for running
    // The whole trivia game. It is in charge of logic for the game,
    // sending out messages, and the players connections.
    public class TriviaServer
    {
        private TcpListener server;
        private List<Player> players = new List<Player>();
        private List<TriviaQuestion> questions = new List<TriviaQuestion>();
        private List<TriviaQuestion> availableQuestions = new List<TriviaQuestion>();
        private List<TriviaQuestion> displayedQuestions = new List<TriviaQuestion>();
        private Random random = new Random();
        private bool gameStarted = false;
        private int currentPlayerIndex = 0;
        private object lockObj = new object();

        // Creates a new server on the specified port
        public TriviaServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            InitializeQuestions();
        }

        // Gets all of the questions loaded and gets 10 of them ready
        // to display to users
        private void InitializeQuestions()
        {
            questions = QuestionBank.GetQuestions();
            availableQuestions = new List<TriviaQuestion>(questions);
            RefillDisplayedQuestions();
        }

        // This part chooses 10 random quesitons to be displayed
        private void RefillDisplayedQuestions()
        {
            // shuffle the questions
            var shuffled = availableQuestions.OrderBy(x => random.Next()).ToList();

            int count = Math.Min(10, shuffled.Count);
            displayedQuestions = shuffled.Take(count).ToList();
        }

        // The start of the server and starts taking client connections
        public void Start()
        {
            server.Start();
            Console.WriteLine("Server started. Waiting for players...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        // This part gets the clients name and adds them
        // To the game server
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            try
            {
                // Getting players name
                SendMessage(stream, "Welcome to Trivia! Please enter your name:");
                string name = ReceiveMessage(stream);

                if (string.IsNullOrWhiteSpace(name))
                {
                    SendMessage(stream, "Invalid name. Disconnecting from server.");
                    client.Close();
                    return;
                }

                Player player = new Player(client, name);

                // Getting game set up with players
                lock (lockObj)
                {
                    if (gameStarted)
                    {
                        SendMessage(stream, "Game already started. Please wait for the next round.");
                        client.Close();
                        return;
                    }

                    players.Add(player);
                    Console.WriteLine($"{name} joined the game. Total player count: {players.Count}");
                    BroadcastMessage($"{name} has joined the game! ({players.Count} players)");

                    if (players.Count >= 2)
                    {
                        SendMessage(stream, "Waiting for game to start... (Type 'START' to begin the round)");
                    }
                    else
                    {
                        SendMessage(stream, "Waiting for more players to join...");
                    }
                }

                // Waiting for START
                while (!gameStarted)
                {
                    string message = ReceiveMessage(stream);
                    if (message != null && message.ToUpper() == "START" && players.Count >= 2)
                    {
                        lock (lockObj)
                        {
                            if (!gameStarted)
                            {
                                gameStarted = true;
                                Thread gameThread = new Thread(RunGame);
                                gameThread.Start();
                            }
                        }
                    }
                    Thread.Sleep(100);
                }

                // Keeping player connection
                while (client.Connected && gameStarted)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }

        // This section is in charge of running the entire game 
        private void RunGame()
        {
            // Announcing game start
            BroadcastMessage("\n=== GAME STARTING ===");
            BroadcastMessage($"Players: {string.Join(", ", players.Select(p => p.Name))}");
            BroadcastMessage("First to 5 points wins!\n");
            Thread.Sleep(2000);

            // The loop that will continue until someone
            // has gotten a score of 5 points.
            while (true)
            {
                lock (lockObj)
                {
                    if (availableQuestions.Count == 0)
                    {
                        BroadcastMessage("All questions have been used up! Getting new questions...");
                        availableQuestions = new List<TriviaQuestion>(questions);
                        RefillDisplayedQuestions();
                    }

                    // Getting more questions
                    if (displayedQuestions.Count < 5 && availableQuestions.Count > 0)
                    {
                        RefillDisplayedQuestions();
                    }

                    Player currentPlayer = players[currentPlayerIndex];

                    BroadcastMessage($"\n--- {currentPlayer.Name}'s Turn ---");
                    ShowScoreboard();

                    // Current questions
                    SendMessage(currentPlayer.Stream, "\nAvailable Questions:");
                    for (int i = 0; i < displayedQuestions.Count; i++)
                    {
                        SendMessage(currentPlayer.Stream, $"{i + 1}. {displayedQuestions[i].Question}");
                    }

                    SendMessage(currentPlayer.Stream, $"\nPick a question (1-{displayedQuestions.Count}):");

                    // Telling the game whose turn it is
                    // to pick a question
                    BroadcastMessageExcept(currentPlayer, $"{currentPlayer.Name} is picking a question...");

                    string choice = ReceiveMessage(currentPlayer.Stream);

                    // If an invalid number is chosen, it skips that player and moves to the next
                    if (!int.TryParse(choice, out int questionIndex) || questionIndex < 1 || questionIndex > displayedQuestions.Count)
                    {
                        SendMessage(currentPlayer.Stream, "Invalid option! Skipping turn.");
                        BroadcastMessageExcept(currentPlayer, $"{currentPlayer.Name} made an incorrect choice. Moving on to the next player.");
                        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                        Thread.Sleep(2000);
                        continue;
                    }

                    TriviaQuestion selectedQuestion = displayedQuestions[questionIndex - 1];

                    // Removing the question that was picked
                    displayedQuestions.RemoveAt(questionIndex - 1);
                    availableQuestions.Remove(selectedQuestion);

                    // Adding a new question if there is less than 10
                    if (displayedQuestions.Count < 10 && availableQuestions.Count > 0)
                    {
                        var newQuestion = availableQuestions.OrderBy(x => random.Next()).First();
                        if (!displayedQuestions.Contains(newQuestion))
                        {
                            displayedQuestions.Add(newQuestion);
                        }
                    }

                    // Showing current question to all everyone playing
                    BroadcastMessage($"\nQuestion: {selectedQuestion.Question}");
                    for (int i = 0; i < selectedQuestion.Options.Count; i++)
                    {
                        BroadcastMessage($"{i + 1}. {selectedQuestion.Options[i]}");
                    }

                    SendMessage(currentPlayer.Stream, "\nYour answer (1-4):");
                    string answer = ReceiveMessage(currentPlayer.Stream);

                    if (!int.TryParse(answer, out int answerIndex) || answerIndex < 1 || answerIndex > 4)
                    {
                        BroadcastMessage($"{currentPlayer.Name} gave an incorrect answer!");
                        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                        Thread.Sleep(2000);
                        continue;
                    }

                    // If player got the answer correct and/or won the game
                    if (answerIndex - 1 == selectedQuestion.CorrectAnswer)
                    {
                        currentPlayer.Score++;
                        BroadcastMessage($"\nCORRECT! {currentPlayer.Name} now has {currentPlayer.Score} points!");

                        if (currentPlayer.Score >= 5)
                        {
                            BroadcastMessage($"\n{currentPlayer.Name} WINS THE GAME!");
                            ShowScoreboard();
                            gameStarted = false;
                            ResetGame();
                            return;
                        }
                    }
                    // PLayer got the wrong answer
                    else
                    {
                        string correctAnswer = selectedQuestion.Options[selectedQuestion.CorrectAnswer];
                        BroadcastMessage($"\nIncorrect! The correct answer was: {correctAnswer}");
                    }

                    Thread.Sleep(3000);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                }
            }
        }

        //Displaying the current score to each player
        private void ShowScoreboard()
        {
            BroadcastMessage("\n--- Scoreboard ---");
            foreach (var player in players.OrderByDescending(p => p.Score))
            {
                BroadcastMessage($"{player.Name}: {player.Score} points");
            }
            BroadcastMessage("------------------");
        }

        // Game has ended and resets
        private void ResetGame()
        {
            players.Clear();
            currentPlayerIndex = 0;
            availableQuestions = new List<TriviaQuestion>(questions);
            RefillDisplayedQuestions();
            Console.WriteLine("Game ended. Ready for new players.");
        }

        // Sends a message to a specific client
        private void SendMessage(NetworkStream stream, string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                stream.Write(data, 0, data.Length);
            }
            catch { }
        }

        // Receives a message from a specific client
        private string ReceiveMessage(NetworkStream stream)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
            }
            catch
            {
                return null!;
            }
        }

        // Sends a message to all the connected players
        private void BroadcastMessage(string message)
        {
            lock (lockObj)
            {
                foreach (var player in players)
                {
                    SendMessage(player.Stream, message);
                }
            }
            Console.WriteLine(message);
        }

        // Sends a message to all the players except one
        private void BroadcastMessageExcept(Player except, string message)
        {
            lock (lockObj)
            {
                foreach (var player in players)
                {
                    if (player != except)
                    {
                        SendMessage(player.Stream, message);
                    }
                }
            }
        }
    }
}