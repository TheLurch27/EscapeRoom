using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscapeRoom_Kevin_Game;
using EscapeRoom_Kevin;
using EscapeRoom_Kevin_Room;
using EscapeRoom_Kevin_Player;

namespace EscapeRoom_Kevin_Menu
{
    internal class Menu
    {
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Start Game");
                Console.WriteLine("2. Instructions");
                Console.WriteLine("3. Settings");
                Console.WriteLine("4. Scoreboard");
                Console.WriteLine("5. Exit Game");
                Console.Write("Select an option: ");

                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (choice)
                {
                    case '1':
                        Game.PlayGame();
                        break;
                    case '2':
                        ShowInstructions();
                        break;
                    case '3':
                        ShowSettingsMenu();
                        break;
                    case '4':
                        ShowScoreboard();
                        break;
                    case '5':
                        ExitGame();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        #region WelcomeMessage
        public static void WelcomeMessage()
        {
            // Welcome
            Console.WriteLine("Welcome!");
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

        #region ShowInstructions
        public static void ShowInstructions()
        {
            Console.Clear();
            Console.WriteLine("Instructions:");
            Console.WriteLine("Move the player (P) through the room, collect the key (K) to exit the room through the door (D).");
            Console.WriteLine();
            Console.WriteLine("Press 'B' to go back to the Main Menu.");
            Console.ReadKey();
        }
        #endregion

        #region ShowSettingsMenu
        public static void ShowSettingsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Settings Menu:");
                Console.WriteLine("1. Audio Settings");
                Console.WriteLine("2. Key Settings");
                Console.WriteLine("");
                Console.WriteLine("Press 'B' to go back to the Main Menu.");
                Console.Write("Select an option: ");

                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (choice)
                {
                    case '1':
                        ShowAudioSettings();
                        break;
                    case '2':
                        ShowKeySettings();
                        break;
                    case 'B':
                    case 'b':
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        #endregion

        #region ShowAudioSettings
        public static void ShowAudioSettings()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Audio Settings");
                Console.WriteLine("1. Enable Beep Sound");
                Console.WriteLine("2. Disable Beep Sound");
                Console.WriteLine("");
                Console.WriteLine("Press 'B' to go back to the Main Menu.");
                Console.Write("Select an option: ");
                char choice = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (choice)
                {
                    case '1':
                        Game.isBeeping = true;
                        Console.WriteLine("Änderung wurde übernommen.");
                        Console.ReadKey();
                        return;
                    case '2':
                        Game.isBeeping = false;
                        Console.WriteLine("Änderung wurde übernommen.");
                        Console.ReadKey();
                        return;
                    case 'B':
                    case 'b':
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        #endregion

        #region ShowKeySettings
        public static void ShowKeySettings()
        {
            Console.Clear();
            Console.WriteLine("Key Settings");
            Console.WriteLine("W|▲ = Up");
            Console.WriteLine("A|◄ = Left");
            Console.WriteLine("S|▼ = Down");
            Console.WriteLine("D|► = Right");
            Console.WriteLine("");
            Console.Write("Press 'B' to go back to the Main Menu.");
            Console.Write("Select an option: ");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (choice)
            {
                case 'B':
                case 'b':
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    Console.ReadKey();
                    ShowKeySettings();
                    break;
            }
        }
        #endregion

        #region ShowScoreboard
        public static void ShowScoreboard()
        {
            Console.Clear();
            Console.WriteLine("Scoreboard:");
            List<Player.PlayerInfo> players = LoadScoreboard("scoreboard.json"); // Lade die Daten aus der JSON-Datei
            players.Sort((player1, player2) => player1.ElapsedTime.CompareTo(player2.ElapsedTime));

            for (int i = 0; i < Math.Min(players.Count, 5); i++)
            {
                Console.WriteLine($"Position {i + 1}: {players[i].PlayerName} - Time: {players[i].ElapsedTime.ToString("mm':'ss'.'ff")}");
            }

            Console.WriteLine("Press 'B' to go back to the Main Menu.");
            while (Console.ReadKey().Key != ConsoleKey.B) ;
        }
        #endregion

        #region LoadScoreboard
        public static List<Player.PlayerInfo> LoadScoreboard(string fileName)
        {
            List<Player.PlayerInfo> players = new List<Player.PlayerInfo>();

            try
            {
                if (File.Exists(fileName))
                {
                    string json = File.ReadAllText(fileName);
                    players = JsonConvert.DeserializeObject<List<Player.PlayerInfo>>(json);
                }
            }
            catch (Exception)
            {
                // Wenn das Lesen der Scoreboard-Datei fehlschlägt, wird eine leere Liste verwendet
            }

            return players;
        }
        #endregion

        #region SaveScoreboard
        public static void SaveScoreboard(List<PlayerInfo> players)
        // Speichert "eigentlich" die Spielergebnisse in einer JSON-Datei (Bin aber zu doof dafür).
        {
            string json = JsonConvert.SerializeObject(players);
            File.WriteAllText("scoreboard.json", json);
        }
        #endregion

        #region ExitGame
        public static void ExitGame()
        {
            Console.Clear();
            Console.Write("Are you sure you want to exit the game? (J) Yes, (N) No: ");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (choice)
            {
                case 'J':
                case 'j':
                    SaveScoreboard(players); // Übergebe die players-Liste an die Methode
                    Environment.Exit(0);
                    break;
                case 'N':
                case 'n':
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    Console.ReadKey();
                    ExitGame();
                    break;
            }
        }

        #endregion

    }
}