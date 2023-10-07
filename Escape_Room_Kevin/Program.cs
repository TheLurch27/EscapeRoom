﻿using Microsoft.VisualBasic;

namespace Escape_Room_Kevin
{
    internal class Program
    {
        #region Deklaration

        private static int roomWidth, roomHeight, playerX, playerY, playerResetX, playerResetY, keyX, keyY, keyResetX, keyResetY, doorX, doorY;
        private static bool hasKey, isGameFinished;
        private static string playerName = "";
        private static int numberOfPlayers;
        private static string[,] map;
        private static List<PlayerInfo> players = new List<PlayerInfo>();
        private static string upArrow = "\u2191", downArrow = "\u2193", rightArrow = "\u2192", leftArrow = "\u2190";

        #endregion

        static void Main(string[] args)
        {
            WelcomeMessage();
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


        
        private static void ResetGame()
        {
            playerX = playerResetX;
            playerY = playerResetY;
            keyX = keyResetX;
            keyY = keyResetY;
            hasKey = false;
            isGameFinished = false;
        }



        #region Welcome | Instructions | Controls

        private static void WelcomeMessage()
        {
            // Welcome
            Console.WriteLine("Welcome!");
            Console.ReadKey();
            Console.Clear();

            // Instructions
            Console.WriteLine("Instructions");
            Console.WriteLine("Move the player (P) through the room, collect the key (K) to exit the room through the door (D).");
            Console.ReadKey();
            Console.Clear();

            // Controls
            Console.WriteLine("Controls");
            Console.WriteLine("W|▲ = Up");
            Console.WriteLine("A|◄ = Left");
            Console.WriteLine("S|▼ = Down");
            Console.WriteLine("D|► = Right");
            Console.ReadKey();
            Console.Clear();
        }

        #endregion

        #region Number of Players

        private static void NumberOfPlayers()
        {
            int minPlayers = 1;
            int maxPlayers = 4;

            while (true)
            {
                Console.Write($"Enter the number of players (between {minPlayers} and {maxPlayers}): ");
                int input = Convert.ToInt32(Console.ReadLine());

                if (input >= minPlayers && input <= maxPlayers)
                {
                    numberOfPlayers = input;
                    break; // Exit the loop when input is valid
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

        #region Player Name

        private static void NameDeclaration(int playerNumber)
        {
            Console.Write($"Player {playerNumber}, enter your name: ");
            playerName = Console.ReadLine();
        }

        #endregion

        #region Custom Map Dimensions

        private static void CustomMapCreation()
        {
            Console.Write("Enter the width of the room: ");
            roomWidth = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the height of the room: ");
            roomHeight = Convert.ToInt32(Console.ReadLine());

            Console.Clear();
        }

        #endregion

        #region Player Ranking

        private static void PlayerRanking(List<PlayerInfo> players)
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

        #region Player Info

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

        #region Room Initialization

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

        #region Place Player

        private static void PlacePlayer()
        {
            Random rnd = new Random();
            playerX = rnd.Next(1, roomWidth - 2);
            playerY = rnd.Next(1, roomHeight - 2);

            playerResetX = playerX;
            playerResetY = playerY;
        }

        #endregion

        #region Player Movement

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

        #region Place Key

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

        #region Place Door

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
            Console.WriteLine("Congratulations! You opened the door and escaped the room!");
            Console.ReadKey();
        }

        #endregion

        #region Winning Message

        private static void WonMessage()
        {
            Console.WriteLine("Congratulations! You opened the door and escaped the room!");
            Console.ReadKey();
            Console.Clear();
        }

        #endregion

        #region Beep

        private static void Beep()
        {
            Console.Beep();
        }

        #endregion

        #region When the Game Ends

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

    }

}