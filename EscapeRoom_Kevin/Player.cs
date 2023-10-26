using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscapeRoom_Kevin_Room;
using EscapeRoom_Kevin_Game;
using EscapeRoom_Kevin_Menu;
using EscapeRoom_Kevin;
using EscapeRoom_Kevin_Scoreboard;

namespace EscapeRoom_Kevin_Player
{
    internal class Player
    {
        public static int playerX, playerY, playerResetX, playerResetY;
        public static string playerName = "";
        public static List<PlayerInfo> players = new List<PlayerInfo>();
        public static int numberOfPlayers;
        #region PlayerInfo
        public class PlayerInfo
        // Verfolgt die Spielerinformationen, einschließlich des Spielerstartzeitpunkt, des Endzeitpunkts
        // und die für das Spiel benötigte Zeit.
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

        #region PlayerRanking
        public static void PlayerRanking(List<PlayerInfo> Player)
        // Sortiert die Spielergebnisse nach der Absolvierten Zeiten und zeigt "eigentlich" die Rangliste der Spieler an.
        {
            Console.WriteLine("Scoreboard:");

            //Sortiert die Spieler nach absolvierter Zeit.
            players.Sort((player1, player2) => player1.ElapsedTime.CompareTo(player2.ElapsedTime));

            // Zeigt die Ergebnisse jedes Spielers an.
            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine($"Position {i + 1}: {players[i].PlayerName} - Time: {players[i].ElapsedTime.ToString("mm':'ss'.'ff")}");
            }
        }
        #endregion

        #region PlacePlayer
        public static void PlacePlayer()
        // Platziert den Player zufällig/"Random" im Raum.
        {
            Random rnd = new Random();
            playerX = rnd.Next(1, Room.roomWidth - 2);
            playerY = rnd.Next(1, Room.roomHeight - 2);

            playerResetX = playerX;
            playerResetY = playerY;
        }
        #endregion

        #region NumberOfPlayers
        public static void NumberOfPlayers()
        // Fordert den Benutzer auf, die Anzahl der Spieler zwischen 1 und 4 auszuwählen.
        {
            int minPlayers = 1;
            int maxPlayers = 4;

            while (true)
            {
                Console.Clear();
                Console.Write($"Enter the number of players (between {minPlayers} and {maxPlayers}): ");
                string input = Console.ReadLine();
                int convertedString;
                Console.Clear();

                if (int.TryParse(input, out convertedString) && convertedString >= minPlayers && convertedString <= maxPlayers)
                {
                    numberOfPlayers = convertedString;
                    break;
                }
                else       // ANSONSTEN: wird dieser Fehler ausgegeben und auf erneute Eingabe verwiesen.
                {
                    Console.WriteLine("Oops... Something went wrong!");
                    Console.WriteLine("Please enter a number between 1 and 4!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        #endregion

        #region HandleInput
        public static void HandleInput(ConsoleKeyInfo keyInfo)
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
                    Game.isGameFinished = true;
                    return;
            }

            if (IsValidMove(newPlayerX, newPlayerY))
            {
                MovePlayer(newPlayerX, newPlayerY);

                if (newPlayerX == Room.keyX && newPlayerY == Room.keyY && !Room.hasKey)
                {
                    Room.CollectKey();
                }

                if (newPlayerX == Room.doorX && newPlayerY == Room.doorY && Room.hasKey)
                {
                    Room.OpenDoor();
                }
            }
        }

        public static bool IsValidMove(int x, int y)
        {
            if (x < 0 || x >= Room.roomWidth || y < 0 || y >= Room.roomHeight)
            {
                return false;
            }

            if (Room.map[x, y] == "██")
            {
                return false;
            }

            return true;
        }

        public static void MovePlayer(int x, int y)
        {
            Room.map[playerX, playerY] = "  ";
            playerX = x;
            playerY = y;
            Room.map[playerX, playerY] = ":)";
        }
        #endregion

        #region ConvertAndAddToScoreboard
        public static void ConvertAndAddToScoreboard(List<PlayerInfo> playerInfos)
        {
            List<Scoreboard.ScoreboardEntry> entries = new List<Scoreboard.ScoreboardEntry>();

            foreach (var playerInfo in playerInfos)
            {
                Scoreboard.ScoreboardEntry entry = new Scoreboard.ScoreboardEntry
                {
                    Name = playerInfo.PlayerName,
                    Time = playerInfo.ElapsedTime // Hier können Sie die Zeitkonvertierung anpassen
                };
                entries.Add(entry);
            }

            Scoreboard.SaveScoreboard(entries); // Speichern Sie die konvertierte Liste in der Scoreboard-Klasse
        }
        #endregion
    }
}
