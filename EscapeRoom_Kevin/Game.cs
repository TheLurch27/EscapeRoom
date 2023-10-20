using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscapeRoom_Kevin_Menu;
using EscapeRoom_Kevin;
using EscapeRoom_Kevin_Room;
using EscapeRoom_Kevin_Player;
using Newtonsoft.Json;

namespace EscapeRoom_Kevin_Game
{
    internal class Game
    {
        public static bool isBeeping = true;

        #region PlayGame
        public static void PlayGame()
        // Leitet das Spiel (Alle Komponenten kommen hier zusammen)
        {
            Player.NumberOfPlayers();
            Room.CustomMapCreation();
            InitializeRoom();
            Player.PlacePlayer();
            Room.PlaceKey();
            Room.PlaceDoor();

            for (int playerNumber = 1; playerNumber <= Player.numberOfPlayers; playerNumber++)
            {
                Console.Clear();
                Console.WriteLine($"Player {playerNumber}, it's your turn.");
                NameDeclaration(playerNumber);

                Player.PlayerInfo currentPlayer = new Player.PlayerInfo(Player.playerName);
                currentPlayer.StartTimer();

                GameCompleted(Player.currentPlayer);
                currentPlayer.StopTimer();
                currentPlayer.CalculateTime();

                WonMessage();
                Player.players.Add(currentPlayer);

                if (playerNumber < Player.numberOfPlayers)
                {
                    Console.WriteLine("Continue to the next player. Press any key to continue...");
                    Console.ReadKey();
                    ResetGame();
                }
            }

            Player.PlayerRanking(Player.players);
        }
        #endregion

        #region InitializeRoom
        private static void InitializeRoom()
        // Berechnet die Wände und den Boden.
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

        #region NameDeclaration
        private static void NameDeclaration(int playerNumber)
        // fordert den Spieler auf seinen Namen einzutragen.
        {
            Console.Write($"Player {playerNumber}, enter your name: ");
            playerName = Console.ReadLine();
            Console.Clear();
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

        #region Beep

        public static void Beep()
        {
            if (isBeeping == true)
            {
                Console.Beep();
            }
        }

        #endregion
    }
}
