namespace EscapeRoom_Kevin
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

        #endregion

        #region Zeichen (Unwichtig)

        string upArrow = "\u2191";
        string downArrow = "\u2193";
        string rightArrow = "\u2192";
        string leftArrow = "\u2190";

        #endregion

        static void Main(string[] args)
        {
            WelcomeMessage();
            CustomMapCreation();
            InitializeRoom();
            PlacePlayer();
            PlaceKey();
            PlaceDoor();
            GameCompleted();
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

        //
        //     
        //     {
        //         for (int x = 0; x < roomWidth; x++)
        //         {
        //             if (x == playerX && y == playerY)
        //             {
        //                 Console.ForegroundColor = ConsoleColor.Green;
        //                 Console.Write('P'); // Player
        //             }
        //             else if (x == keyX && y == keyY)
        //             {
        //                 if (!hasKey)
        //                 {
        //                     Console.ForegroundColor = ConsoleColor.Yellow;
        //                     Console.Write('K'); // Schlüssel
        //                 }
        //                 else
        //                 {
        //                     Console.Write('.'); // Schlüssel eingesammelt
        //                 }
        //             }
        //             else if (x == doorX && y == doorY)
        //             {
        //                 if (hasKey)
        //                 {
        //                     Console.ForegroundColor = ConsoleColor.Red;
        //                     Console.Write('D'); // Tür geoffnet
        //                 }
        //                 else
        //                 {
        //                     Console.Write('#'); // Verschlossene Tür
        //                 }
        //             }
        //             else
        //             {
        //                 Console.ForegroundColor = ConsoleColor.White;
        //                 Console.Write(map[x, y]); // Boden oder Wand
        //             }
        //         }
        //         Console.WriteLine();
        //     }
        // 
        //     Console.ForegroundColor = ConsoleColor.White;
        // }

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

            // return x >= 0 && x < roomWidth && y >= 0 && y < roomHeight && map[x, y] != "#";
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
            Console.WriteLine("Gluckwunsch! Du hast die Tür geöffnet und konntest den Raum verlassen!");
        }

        #endregion

        #region BEEEEEEEP

        static void Beep()
        {
            Console.Beep();
        }

        #endregion

        #region Funktion Wenn das Spiel abgeschlossen wurde.

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