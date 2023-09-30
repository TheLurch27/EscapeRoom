using Microsoft.VisualBasic;

namespace Escape_Room_Kevin
{
    internal class Program
    {
        #region Deklaration

        // Room
        static int roomWidth;
        static int roomHeight;
        static string[,] map;

        // Spieler
        static int playerX;
        static int playerY;

        // Schlüssel
        static int keyX;
        static int keyY;
        static bool hasKey = false;

        // Tür
        static int doorX;
        static int doorY;

        // Game Mechanik
        static bool isGameFinished = false;

        // Spieler Name
        static string PlayerName;

        //Player Anzahl
        static int numberOfPlayer;


        #endregion

        #region Zeichen (Unwichtig)

        string upArrow = "\u2191";
        string downArrow = "\u2193";
        string rightArrow = "\u2192";
        string leftArrow = "\u2190";

        #endregion

        #region Spieler Liste

        static List<PlayerInfo> players = new List<PlayerInfo>();

        #endregion

        static void Main(string[] args)
        {
            WelcomeMessage();
            NumberOfPlayer();

            List<PlayerInfo> players = new List<PlayerInfo>();

            for (int playerNumber = 1; playerNumber <= numberOfPlayer; playerNumber++)
            {
                Console.Clear();
                Console.WriteLine($"Spieler {playerNumber}, es ist jetzt deine Runde.");

                NameDeclaration(playerNumber); // Übergeben des playerNumber-Parameters

                CustomMapCreation(); // Abfrage der Map-Maße nur einmal am Anfang
                InitializeRoom();
                PlacePlayer();
                PlaceKey();
                PlaceDoor();

                PlayerInfo currentPlayer = new PlayerInfo(PlayerName);
                currentPlayer.StartTimer(); // Starte den Timer vor dem Spiel

                GameCompleted(currentPlayer); // Übergeben des aktuellen Spielers an die Methode
                currentPlayer.StopTimer(); // Stoppe den Timer nach dem Spiel
                currentPlayer.CalculateTime(); // Berechne die verstrichene Zeit

                WonMessage();
                players.Add(currentPlayer); // Den aktuellen Spieler zur Liste hinzufügen

                if (playerNumber < numberOfPlayer)
                {
                    Console.WriteLine("Weiter zum nächsten Spieler. Drücken Sie eine Taste, um fortzufahren...");
                    Console.ReadKey();
                }
            }

            // Scoreboard anzeigen und Spielergebnisse anzeigen
            PlayerRanking(players);
        }



        #region Begrüßung | Anleitung | Tastenbelegung

        static void WelcomeMessage()
        {

            // Begrüßung
            Console.WriteLine("Herzlich Willkommen!");
            Console.ReadKey();
            Console.Clear();

            // Anleitung
            Console.WriteLine("Anleitung");
            Console.WriteLine("Bewege den Player (P) durch den Raum und sammle den Schlüssel (K) ein um den Raum durch die Tür (D) zu verlassen.");
            Console.ReadKey();
            Console.Clear();

            // Tastenbelegung
            Console.WriteLine("Tastenbelegung");
            Console.WriteLine("W|▲ = Hoch");
            Console.WriteLine("A|◄ = Links");
            Console.WriteLine("S|▼ = Runter");
            Console.WriteLine("D|► = Rechts");
            Console.ReadKey();
            Console.Clear();


        }

        #endregion

        #region Anzahl der Spieler

        static void NumberOfPlayer()
        {
            Console.Write("Geben Sie die Anzahl der Spieler (max. 4) ein: ");
            numberOfPlayer = Convert.ToInt32(Console.ReadLine());
            numberOfPlayer = Math.Max(1, Math.Min(4, numberOfPlayer)); // Begrenzen auf 1 bis 4 Spieler
        }

        #endregion

        #region Name eingeben

        static void NameDeclaration(int playerNumber)
        {
            Console.Write($"Spieler {playerNumber}, geben Sie Ihren Namen ein: ");
            PlayerName = Console.ReadLine();
        }




        #endregion

        #region Eingabe der Map Maße

        static void CustomMapCreation()
        {
            Console.Write("Geben Sie die Breite des Raums ein: ");
            roomWidth = Convert.ToInt32(Console.ReadLine());

            Console.Write("Geben Sie die Hohe des Raums ein: ");
            roomHeight = Convert.ToInt32(Console.ReadLine());

            Console.Clear();
        }

        #endregion

        #region Spielzeit berechnen



        #endregion

        #region zeigt spieler Ranking an

        static void PlayerRanking(List<PlayerInfo> players)
        {
            Console.WriteLine("Scoreboard:");

            // Sortiere die Spieler nach der gemessenen Zeit (angenommen, PlayerInfo hat eine Eigenschaft "ElapsedTime" für die gemessene Zeit)
            players.Sort((player1, player2) => player1.ElapsedTime.CompareTo(player2.ElapsedTime));

            // Zeige die Ergebnisse für jeden Spieler an
            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine($"Platz {i + 1}: {players[i].PlayerName} - Zeit: {players[i].ElapsedTime}");
            }

            // Du kannst hier auch andere Informationen anzeigen, je nachdem, was du erfasst hast.
        }


        #endregion

        #region Spieler Info

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



        #region Map Aufbau

        static void InitializeRoom()
        {
            map = new string[roomWidth, roomHeight];

            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (x == 0 || x == roomWidth - 1 || y == 0 || y == roomHeight - 1)
                    {
                        map[x, y] = "██"; // Wand
                    }
                    else
                    {
                        map[x, y] = "  "; // Boden
                    }
                }
            }
        }

        static void DrawRoom()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            for (int y = 0; y < roomHeight; y++)
                Console.ForegroundColor = ConsoleColor.White;

            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    if (x == playerX && y == playerY)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(":)"); // Spielfigur
                    }
                    else if (x == keyX && y == keyY)
                    {
                        if (!hasKey)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("├o"); // Schlüssel
                        }
                        else
                        {
                            Console.Write("  "); // Schlüssel eingesammelt
                        }
                    }
                    else if (x == doorX && y == doorY)
                    {
                        if (hasKey)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("░░"); // Tür geöffnet
                            
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("▓▓"); // Verschlossene Tür
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(map[x, y]); // Boden oder Wand
                    }
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion

        #region Spieler Platzieren

        static void PlacePlayer()
        {
            Random rnd = new Random();
            playerX = rnd.Next(1, roomWidth - 2);
            playerY = rnd.Next(1, roomHeight - 2);
        }

        #endregion

        #region Spieler Bewegen

        static void HandleInput(ConsoleKeyInfo keyInfo)
        {
            int newPlayerX = playerX;
            int newPlayerY = playerY;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    newPlayerY--;
                    break;
                case ConsoleKey.W:
                    newPlayerY--;
                    break;
                case ConsoleKey.RightArrow:
                    newPlayerX++;
                    break;
                case ConsoleKey.D:
                    newPlayerX++;
                    break;
                case ConsoleKey.DownArrow:
                    newPlayerY++;
                    break;
                case ConsoleKey.S:
                    newPlayerY++;
                    break;
                case ConsoleKey.LeftArrow:
                    newPlayerX--;
                    break;
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

        static bool IsValidMove(int x, int y)
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

        static void MovePlayer(int x, int y)
        {
            map[playerX, playerY] = "  ";
            playerX = x;
            playerY = y;
            map[playerX, playerY] = ":)";
        }

        #endregion

        #region Schlüssel Platzieren

        static void PlaceKey()
        {
            Random rnd = new Random();
            keyX = rnd.Next(1, roomWidth - 1);
            keyY = rnd.Next(1, roomHeight - 1);
        }

        #endregion

        #region Schlüssel Aufnehmen

        static void CollectKey()
        {
            hasKey = true;
            Beep();
            keyX = -1; // Schlüssel verschwindet
        }

        #endregion

        #region Tür Platzieren

        static void PlaceDoor()
        {
            Random rnd = new Random();
            int side = rnd.Next(4); // 0: oben, 1: rechts, 2: unten, 3: links

            switch (side)
            {
                case 0: // oben
                    doorX = rnd.Next(1, roomWidth - 1);
                    doorY = 0;
                    break;
                case 1: // rechts
                    doorX = roomWidth - 1;
                    doorY = rnd.Next(1, roomHeight - 1);
                    break;
                case 2: // unten
                    doorX = rnd.Next(1, roomWidth - 1);
                    doorY = roomHeight - 1;
                    break;
                case 3: // links
                    doorX = 0;
                    doorY = rnd.Next(1, roomHeight - 1);
                    break;
            }

            map[doorX, doorY] = "░░";
        }

        #endregion

        #region Tür Öffnen

        static void OpenDoor()
        {
            isGameFinished = true;
            Beep();
            Beep();
            Beep();
            Console.Clear();
            DrawRoom();
            Console.Clear();
            // Console.WriteLine("Glückwunsch! Du hast die Tür geöffnet und konntest den Raum verlassen!");
            Console.ReadKey();
        }

        #endregion

        #region Gewinner Nachricht

        static void WonMessage()
        {
            Console.WriteLine("Herzlichen Glückwunsch! Du hast die Tür geöffnet und konntest den Raum verlassen!");
            Console.ReadKey();
            Console.Clear();
        }

        #endregion

        #region BEEEEEEEP

        static void Beep()
        {
            Console.Beep();
        }

        #endregion

        #region Wenn das Spiel beendet wird

        static void GameCompleted(PlayerInfo player)
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