using System.ComponentModel.Design;
using EscapeRoom_Kevin_Game;
using EscapeRoom_Kevin_Menu;
using EscapeRoom_Kevin_Player;
using EscapeRoom_Kevin_Room;

namespace EscapeRoom_Kevin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu.WelcomeMessage();
            Menu.MainMenu();
        }
    }
}