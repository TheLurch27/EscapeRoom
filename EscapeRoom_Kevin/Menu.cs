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
using EscapeRoom_Kevin_Scoreboard;

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
                        Console.Clear();
                        var scoreboardEntries = Scoreboard.LoadScoreboard();
                        Console.WriteLine("Scoreboard:");

                        foreach (var entry in scoreboardEntries)
                        {
                            Console.WriteLine($"Name: {entry.Name}, Time: {entry.Time.ToString("mm':'ss'.'ff")}");
                        }

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
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
            Console.WriteLine(",------. ,---.   ,-----.  ,---.  ,------. ,------.    ,------.  ,-----.  ,-----. ,--.   ,--. ");
            Console.WriteLine("|  .---''   .-' '  .--./ /  O  \\ |  .--. '|  .---'    |  .--. ''  .-.  ''  .-.  '|   `.'   | ");
            Console.WriteLine("|  `--, `.  `-. |  |    |  .-.  ||  '--' ||  `--,     |  '--'.'|  | |  ||  | |  ||  |'.'|  | ");
            Console.WriteLine("|  `---..-'    |'  '--'\\|  | |  ||  | --' |  `---.    |  |\\  \\ '  '-'  ''  '-'  '|  |   |  | ");
            Console.WriteLine("`------'`-----'  `-----'`--' `--'`--'     `------'    `--' '--' `-----'  `-----' `--'   `--' ");
            Console.WriteLine("");
            Console.WriteLine("                                                                          ,--.   ,--.,--.      ,--.         ,------.   ,--.,--.  ,--.  ,--.                ");
            Console.WriteLine("                                                                          |  |   |  |`--' ,---.|  ,---.     |  .---' ,-|  |`--',-'  '-.`--' ,---. ,--,--,  ");
            Console.WriteLine("                                                                          |  |.'.|  |,--.(  .-'|  .-.  |    |  `--, ' .-. |,--.'-.  .-',--.| .-. ||      \\ ");
            Console.WriteLine("                                                                          |   ,'.   ||  |.-'  `)  | |  |    |  `---.\\ `-' ||  |  |  |  |  |' '-' '|  ||  | ");
            Console.WriteLine("                                                                          '--'   '--'`--'`----'`--' `--'    `------' `---' `--'  `--'  `--' `---' `--''--' ");
            Console.ReadKey();
            Console.Clear();
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


        #endregion

        #region LoadScoreboard


        #endregion

        #region SaveScoreboard

        public static void SaveScoreboard(List<Player.PlayerInfo> players)
        {
            var scoreboardEntries = Scoreboard.ConvertToScoreboardEntries(players);
            Scoreboard.SaveScoreboard(scoreboardEntries);
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
                    var players = Player.players;
                    var scoreboardEntries = players.Select(playerInfo => new Scoreboard.ScoreboardEntry
                    {
                        Name = playerInfo.PlayerName,
                        Time = playerInfo.ElapsedTime
                    }).ToList();
                    Scoreboard.SaveScoreboard(scoreboardEntries);
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