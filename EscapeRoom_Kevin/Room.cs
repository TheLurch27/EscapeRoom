using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscapeRoom_Kevin_Game;
using EscapeRoom_Kevin_Menu;
using EscapeRoom_Kevin_Player;
using EscapeRoom_Kevin;

namespace EscapeRoom_Kevin_Room
{
    internal class Room
    {
        public static int roomHeight, roomWidth, keyX, keyY, doorX, doorY, keyResetX, keyResetY, roomSize;
        public static string[,] map;
        public static bool hasKey, isGameFinished;


        #region Room Drawing

        public static void DrawRoom()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            for (int y = 0; y < roomHeight; y++)
            {
                Console.ForegroundColor = ConsoleColor.White;

                for (int x = 0; x < roomWidth; x++)
                {
                    // Überprüfe nur die Änderungen in der Karte
                    if (x == Player.playerX && y == Player.playerY)
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

        #region CustomMapCreation
        public static void CustomMapCreation()
        // Lässt den Benutzer die Raumgröße zwischen 5-30 Wählen.
        {
            int minRoomSize = 5;
            int maxRoomSize = 30;
            int roomSize;
            string input;

            while (true)
            {
                Console.Write($"Enter the room size (between {minRoomSize} and {maxRoomSize}): ");
                input = Console.ReadLine();

                if (int.TryParse(input, out roomSize) && roomSize >= minRoomSize && roomSize <= maxRoomSize)
                {
                    roomWidth = roomSize;
                    roomHeight = roomSize;
                    break;
                }
                else        // ANSONSTEN: wird dieser Fehler ausgegeben und auf erneute Eingabe verwiesen.
                {
                    Console.WriteLine("Oops... Something went wrong!");
                    Console.WriteLine($"Please enter a number between {minRoomSize} and {maxRoomSize}!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        #endregion

        #region PlaceKey
        public static void PlaceKey()
        // Platziert den Schlüssel zufällig im Raum.
        {
            Random rnd = new Random();
            keyX = rnd.Next(1, roomWidth - 1);
            keyY = rnd.Next(1, roomHeight - 1);

            keyResetX = keyX;
            keyResetY = keyY;
        }
        #endregion

        #region Collect Key
        public static void CollectKey()
        // legt fest, dass der Spieler den Schlüssel eingesammelt hat.
        {
            hasKey = true;
            Game.Beep();
            keyX = -1; // Schlüssel verschwindet
        }

        #endregion

        #region PlaceDoor
        public static void PlaceDoor()
        // Platziert die Tür zufällig an einer der vier Raumseiten.
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
        public static void OpenDoor()
        //Spielt einen Sound ab, sobald der Spieler mit dem aufgenommenen Schlüssel, durch die Tür geht.
        {
            isGameFinished = true;
            Game.Beep();
            Game.Beep();
            Game.Beep();
            Console.Clear();
            DrawRoom();
            Console.Clear();
            Console.ReadKey();
        }
        #endregion

    }
}
