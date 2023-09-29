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
        static string playerName;


        #endregion

        #region Zeichen (Unwichtig)

        string upArrow = "\u2191";
        string downArrow = "\u2193";
        string rightArrow = "\u2192";
        string leftArrow = "\u2190";

        #endregion

        #region Ranking

        private static List<PlayerInfo> playerRanking = new List<PlayerInfo>();

        #endregion

        static void Main(string[] args)
        {

            WelcomeMessage();
            playerCounter();
            NameDeclaration();
            CustomMapCreation();
            InitializeRoom();
            PlacePlayer();
            PlaceKey();
            PlaceDoor();
            DateTime timeStart = DateAndTime.Now; // Spielzeitmessung gestartet
            GameCompleted();
            WinningMessage();
            PlayerRanking();
            DateTime timeStop = DateAndTime.Now; // Spielzeitmessung beenden

            TimeSpan tmp = CalculateElapsedTime(timeStart); // Berechnet die gespielte Zeit

            Console.WriteLine("Du hast " + tmp + " gebraucht.");  // Zeigt die gespielte Zeit an
            

            PlayerInfo currentPlayer = new PlayerInfo // Hier wird die Spielerinformation erstellt und zur Liste hinzugefügt
            {
                Name = playerName,
                Time = tmp // Die berechnete Spielzeit
            };
            playerRanking.Add(currentPlayer);

            playerRanking.Sort((x, y) => x.Time.CompareTo(y.Time)); // Spieler-Ranking nach Spielzeit sortieren

            // Zeige das Spieler-Ranking an
            Console.WriteLine("Sieh dir die Aktuelle Rangliste an: ");
            Console.WriteLine();
            Console.WriteLine("1 = " + playerRanking[0].Name + " mit einer Zeit von " + playerRanking[0].Time);
            Console.WriteLine("2 = " + playerRanking[1].Name + " mit einer Zeit von " + playerRanking[1].Time);
            Console.WriteLine("3 = " + playerRanking[2].Name + " mit einer Zeit von " + playerRanking[2].Time);
        }



        // Console.WriteLine("Rangliste:");
        //     for (int i = 0; i < playerRanking.Count; i++)
        //     {
        //         Console.WriteLine($"{i + 1}: {playerRanking[i].Name} - {playerRanking[i].Time}");
        //     }

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

        static void playerCounter()
        {
            Console.Write("Geben Sie die Anzahl der Spieler ein (max. 1-4): ");
            int numberOfPlayers = Convert.ToInt32(Console.ReadLine());

            if (numberOfPlayers < 1 || numberOfPlayers > 4)
            {
                Console.WriteLine("Ungültige Anzahl von Spielern. Das Spiel wird beendet.");
                return;
            }

            // Schleife für jeden Spieler
            for (int playerNumber = 1; playerNumber <= numberOfPlayers; playerNumber++)
            {
                // Frage nach dem Spielername
                Console.Write("Spieler " + playerNumber + ", gib deinen Namen ein: ");
                string playerName = Console.ReadLine();

                // Eingabe der Raumgröße
                CustomMapCreation();

                // Initialisierung des Raums
                InitializeRoom();

                // Platzieren des Spielers, Schlüssels und der Tür
                PlacePlayer();
                PlaceKey();
                PlaceDoor();

                // Spielzeitmessung starten
                DateTime timeStart = DateAndTime.Now;

                // Hauptspiel
                GameCompleted();

                // Spieler hat das Spiel beendet
                WinningMessage();

                // Spielzeitmessung beenden
                DateTime timeStop = DateAndTime.Now;

                // Berechne die gespielte Zeit
                TimeSpan tmp = CalculateElapsedTime(timeStart);

                // Zeige die gewonnene Zeit an
                Console.WriteLine("Spieler " + playerNumber + " (" + playerName + ") hat " + tmp + " gebraucht.");
                Console.ReadKey();
                Console.Clear();

                // Hier wird die Spielerinformation erstellt und zur Liste hinzugefügt
                PlayerInfo currentPlayer = new PlayerInfo
                {
                    Name = playerName,
                    Time = tmp // Die berechnete Spielzeit
                };
                playerRanking.Add(currentPlayer);
            }

        }

        #endregion

        #region Name eingeben

        static void NameDeclaration()
        {
            List<PlayerInfo> playerRanking = new List<PlayerInfo>();

            Console.Write("Gib deinen Namen ein: ");
            string playerName = Console.ReadLine();
        }

        #endregion

        #region Spielzeit berechnen

        private static TimeSpan CalculateElapsedTime(DateTime startTime)
        {
            DateTime endTime = DateAndTime.Now;
            TimeSpan elapsedTime = endTime - startTime;
            return elapsedTime;
        }

        #endregion
        #region zeigt spieler Ranking an

        static void PlayerRanking()
        {
            Console.WriteLine("Spieler-Ranking:");
            for (int i = 0; i < playerRanking.Count; i++)
            {
                Console.WriteLine((i + 1) + " = " + playerRanking[i].Name + " mit einer Zeit von " + playerRanking[i].Time);
            }
        }

        #endregion

        #region Spieler Info

        public class PlayerInfo
        {
            public string Name { get; set; }
            public TimeSpan Time { get; set; }
        }

        #endregion

        // #region Zeit anzeige
        // 
        // static void Timer()
        // {
        //     Console.WriteLine("Du hast " + tmp + " gebraucht.");
        //     Console.ReadKey();
        //     Console.Clear();
        // }

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

        static void WinningMessage()
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

        static void GameCompleted()
        {
            while (!isGameFinished)
            {
                DrawRoom();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                HandleInput(keyInfo);
            }
        }

        #endregion

    }

}