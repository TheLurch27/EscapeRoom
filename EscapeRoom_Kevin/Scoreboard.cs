using EscapeRoom_Kevin_Player;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static EscapeRoom_Kevin_Player.Player;

namespace EscapeRoom_Kevin_Scoreboard
{
    internal class Scoreboard
    {
        public class ScoreboardEntry
        // Deklaration
        {
            public string Name { get; set; }
            public TimeSpan Time { get; set; }
        }

        public static List<ScoreboardEntry> ConvertToScoreboardEntries(List<PlayerInfo> playerInfos)
        // 
        {
            List<ScoreboardEntry> scoreboardEntries = new List<ScoreboardEntry>();

            foreach (var playerInfo in playerInfos)
            {
                ScoreboardEntry entry = new ScoreboardEntry
                {
                    Name = playerInfo.PlayerName,
                    Time = playerInfo.ElapsedTime
                };
                scoreboardEntries.Add(entry);
            }

            return scoreboardEntries;
        }

        public static void SaveScoreboard(List<ScoreboardEntry> scoreboard)
        {
            string json = JsonConvert.SerializeObject(scoreboard, Formatting.Indented);
            File.WriteAllText("scoreboard.json", json);
        }

        public static List<ScoreboardEntry> LoadScoreboard()
        {
            try
            {
                string json = File.ReadAllText("scoreboard.json");
                var scoreboardEntries = JsonConvert.DeserializeObject<List<ScoreboardEntry>>(json);
                return scoreboardEntries;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the scoreboard: " + ex.Message);
                return new List<ScoreboardEntry>();
            }
        }
    }
}
