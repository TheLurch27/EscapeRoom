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
using EscapeRoom_Kevin_Scoreboard;

namespace EscapeRoom_Kevin_Game
{
    internal class Game
    {
        public static bool isGameFinished;
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

                Player.players.Add(currentPlayer);
                GameCompleted(currentPlayer);

                WonMessage();

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
            Room.map = new string[Room.roomWidth, Room.roomHeight];

            for (int x = 0; x < Room.roomWidth; x++)
            {
                for (int y = 0; y < Room.roomHeight; y++)
                {
                    if (x == 0 || x == Room.roomWidth - 1 || y == 0 || y == Room.roomHeight - 1)
                    {
                        Room.map[x, y] = "██";
                    }
                    else
                    {
                        Room.map[x, y] = "  ";
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
            Player.playerName = Console.ReadLine();
            Console.Clear();
        }
        #endregion

        #region GameCompleted
        private static void GameCompleted(Player.PlayerInfo player)
        {
            while (!isGameFinished)
            {
                Room.DrawRoom();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Player.HandleInput(keyInfo);

                if (isGameFinished)
                {
                    player.StopTimer();
                    player.CalculateTime();

                    var scoreboardEntries = Scoreboard.ConvertToScoreboardEntries(Player.players);
                    Scoreboard.SaveScoreboard(scoreboardEntries);
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
            Player.playerX = Player.playerResetX;
            Player.playerY = Player.playerResetY;
            Room.keyX = Room.keyResetX;
            Room.keyY = Room.keyResetY;
            Room.hasKey = false;
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
