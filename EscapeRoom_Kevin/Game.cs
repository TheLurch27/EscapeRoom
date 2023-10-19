using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscapeRoom_Kevin_Menu;
using EscapeRoom_Kevin;
using Newtonsoft.Json;

namespace EscapeRoom_Kevin_Game
{
    internal class Game
    {
        public static int roomWidth, roomHeight, playerX, playerY, playerResetX, playerResetY, keyX, keyY, keyResetX, keyResetY, doorX, doorY, roomSize;
        public static bool hasKey, isGameFinished, isBeeping = true;
        public static string playerName = "";
        public static int numberOfPlayers;
        public static string[,] map;
        public static List<PlayerInfo> players = new List<PlayerInfo>();
        public static void PlayGame()
        {
            NumberOfPlayers();
            CustomMapCreation();
            InitializeRoom();
            PlacePlayer();
            PlaceKey();
            PlaceDoor();

            for (int playerNumber = 1; playerNumber <= numberOfPlayers; playerNumber++)
            {
                Console.Clear();
                Console.WriteLine($"Player {playerNumber}, it's your turn.");
                NameDeclaration(playerNumber);

                PlayerInfo currentPlayer = new PlayerInfo(playerName);
                currentPlayer.StartTimer();

                GameCompleted(currentPlayer);
                currentPlayer.StopTimer();
                currentPlayer.CalculateTime();

                WonMessage();
                players.Add(currentPlayer);

                if (playerNumber < numberOfPlayers)
                {
                    Console.WriteLine("Continue to the next player. Press any key to continue...");
                    Console.ReadKey();
                    ResetGame();
                }
            }

            PlayerRanking(players);
        }

        #region NumberOfPlayers
        public static void NumberOfPlayers()
        {
            int minPlayers = 1;
            int maxPlayers = 4;

            while (true)
            {
                Console.Clear();
                Console.Write($"Enter the number of players (between {minPlayers} and {maxPlayers}): ");
                int input = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                if (input >= minPlayers && input <= maxPlayers)
                {
                    numberOfPlayers = input;
                    break; // Exit the loop when input is valid16
                }
                else
                {
                    Console.WriteLine("Oops... Something went wrong!");
                    Console.WriteLine("Please enter a number between 1 and 4!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        #endregion

        #region CustomMapCreation
        private static void CustomMapCreation()
        {
            int minRoomSize = 5;
            int maxRoomSize = 30;

            while (true)
            {
                Console.Write($"Enter the room size (between {minRoomSize} and {maxRoomSize}): ");
                roomSize = Convert.ToInt32(Console.ReadLine());

                if (roomSize >= minRoomSize && roomSize <= maxRoomSize)
                {
                    roomWidth = roomSize;
                    roomHeight = roomSize;
                    break; // Exit the loop when input is valid
                }
                else
                {
                    Console.WriteLine("Oops... Something went wrong!");
                    Console.WriteLine($"Please enter a number between {minRoomSize} and {maxRoomSize}!");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        #endregion

        #region InitializeRoom
        private static void InitializeRoom()
        {
            map = new string[roomWidth, roomHeight];

            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (x == 0 || x == roomWidth - 1 || y == 0 || y == roomHeight - 1)
                    {
                        map[x, y] = "██";
                    }
                    else
                    {
                        map[x, y] = "  ";
                    }
                }
            }
        }
        #endregion

        #region SaveScoreboard
        public static void SaveScoreboard()
        {
            Console.Clear();
            Console.WriteLine("Scoreboard:");
            List<Game.PlayerInfo> players = Menu.LoadScoreboard("scoreboard.json"); // Lade die Daten aus der JSON-Datei
            players.Sort((player1, player2) => player1.ElapsedTime.CompareTo(player2.ElapsedTime));

            for (int i = 0; i < Math.Min(players.Count, 5); i++)
            {
                Console.WriteLine($"Position {i + 1}: {players[i].PlayerName} - Time: {players[i].ElapsedTime.ToString("mm':'ss'.'ff")}");
            }

            Console.WriteLine("Press 'B' to go back to the Main Menu.");
            while (Console.ReadKey().Key != ConsoleKey.B) ;
        }
        #endregion

        #region PlayerInfo
        public class PlayerInfo
        {
            public string PlayerName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public TimeSpan ElapsedTime { get; set; }

            public PlayerInfo(string playerName)
            {
                PlayerName = playerName;
            }

            public void StartTimer()
            {
                StartTime = DateTime.Now;
            }

            public void StopTimer()
            {
                EndTime = DateTime.Now;
            }

            public void CalculateTime()
            {
                ElapsedTime = EndTime - StartTime;
            }
        }
        #endregion

        #region PlacePlayer
        private static void PlacePlayer()
        {
            Random rnd = new Random();
            playerX = rnd.Next(1, roomWidth - 2);
            playerY = rnd.Next(1, roomHeight - 2);

            playerResetX = playerX;
            playerResetY = playerY;
        }
        #endregion

        #region PlaceKey
        private static void PlaceKey()
        {
            Random rnd = new Random();
            keyX = rnd.Next(1, roomWidth - 1);
            keyY = rnd.Next(1, roomHeight - 1);

            keyResetX = keyX;
            keyResetY = keyY;
        }
        #endregion

        #region Collect Key

        private static void CollectKey()
        {
            hasKey = true;
            Beep();
            keyX = -1; // Key disappears
        }

        #endregion

        #region PlaceDoor
        private static void PlaceDoor()
        {
            Random rnd = new Random();
            int side = rnd.Next(4); // 0: top, 1: right, 2: bottom, 3: left

            switch (side)
            {
                case 0: // top
                    doorX = rnd.Next(1, roomWidth - 1);
                    doorY = 0;
                    break;
                case 1: // right
                    doorX = roomWidth - 1;
                    doorY = rnd.Next(1, roomHeight - 1);
                    break;
                case 2: // bottom
                    doorX = rnd.Next(1, roomWidth - 1);
                    doorY = roomHeight - 1;
                    break;
                case 3: // left
                    doorX = 0;
                    doorY = rnd.Next(1, roomHeight - 1);
                    break;
            }

            map[doorX, doorY] = "░░";
        }
        #endregion

        #region Open Door

        private static void OpenDoor()
        {
            isGameFinished = true;
            Beep();
            Beep();
            Beep();
            Console.Clear();
            DrawRoom();
            Console.Clear();
            Console.ReadKey();
        }

        #endregion

        #region PlayerRanking
        private static void PlayerRanking(List<PlayerInfo> Player)
        {
            Console.WriteLine("Scoreboard:");

            // Sort players by elapsed time
            players.Sort((player1, player2) => player1.ElapsedTime.CompareTo(player2.ElapsedTime));

            // Display results for each player
            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine($"Position {i + 1}: {players[i].PlayerName} - Time: {players[i].ElapsedTime.ToString("mm':'ss'.'ff")}");
            }
        }
        #endregion

        #region NameDeclaration
        private static void NameDeclaration(int playerNumber)
        {
            Console.Write($"Player {playerNumber}, enter your name: ");
            playerName = Console.ReadLine();
            Console.Clear();
        }
        #endregion

        #region Room Drawing

        private static void DrawRoom()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            for (int y = 0; y < roomHeight; y++)
            {
                Console.ForegroundColor = ConsoleColor.White;

                for (int x = 0; x < roomWidth; x++)
                {
                    // Überprüfe nur die Änderungen in der Karte
                    if (x == playerX && y == playerY)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(":)");
                    }
                    else if (x == keyX && y == keyY)
                    {
                        if (!hasKey)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("├o");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                    else if (x == doorX && y == doorY)
                    {
                        if (hasKey)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("░░");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("▓▓");
                        }
                    }
                    else
                    {
                        // Übertrage nur den alten Wert, wenn es keine Änderung gibt
                        if (map[x, y] == "██")
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("██");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion

        #region GameCompleted
        private static void GameCompleted(PlayerInfo player)
        {
            while (!isGameFinished)
            {
                DrawRoom();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                HandleInput(keyInfo);

                if (isGameFinished)
                {
                    player.StopTimer();
                    player.CalculateTime();
                }
            }
        }
        #endregion

        #region WonMessage
        private static void WonMessage()
        {
            Console.WriteLine("Congratulations! You opened the door and escaped the room!");
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

        #region ResetGame
        private static void ResetGame()
        {
            playerX = playerResetX;
            playerY = playerResetY;
            keyX = keyResetX;
            keyY = keyResetY;
            hasKey = false;
            isGameFinished = false;
        }
        #endregion

        #region HandleInput
        private static void HandleInput(ConsoleKeyInfo keyInfo)
        {
            int newPlayerX = playerX;
            int newPlayerY = playerY;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    newPlayerY--;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    newPlayerX++;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    newPlayerY++;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    newPlayerX--;
                    break;
                case ConsoleKey.Escape:
                    isGameFinished = true;
                    return;
            }

            if (IsValidMove(newPlayerX, newPlayerY))
            {
                MovePlayer(newPlayerX, newPlayerY);

                if (newPlayerX == keyX && newPlayerY == keyY && !hasKey)
                {
                    CollectKey();
                }

                if (newPlayerX == doorX && newPlayerY == doorY && hasKey)
                {
                    OpenDoor();
                }
            }
        }

        private static bool IsValidMove(int x, int y)
        {
            if (x < 0 || x >= roomWidth || y < 0 || y >= roomHeight)
            {
                return false;
            }

            if (map[x, y] == "██")
            {
                return false;
            }

            return true;
        }

        private static void MovePlayer(int x, int y)
        {
            map[playerX, playerY] = "  ";
            playerX = x;
            playerY = y;
            map[playerX, playerY] = ":)";
        }
        #endregion

        #region Beep

        private static void Beep()
        {
            if (isBeeping == true)
            {
                Console.Beep();
            }
        }

        #endregion
    }
}
