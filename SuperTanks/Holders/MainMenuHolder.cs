using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SuperTanks.Holders
{
    internal class MainMenuHolder : AbstractHolder
    {
        private string[] menuOption = new string[] { "START", "EXIT"};
        private int currentPosition = 0;

        public override string[][] getMenuData()
        {
            string[][] outMenu = new string[menuOption.Length][];
            for (int i = 0; i < menuOption.Length; i++)
            {
                outMenu[i] = new string[] {currentPosition==i ? "▶" : " " , menuOption[i] };
            }
            
            return outMenu;
        }

        public override void OnPressKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    currentPosition = Math.Max(currentPosition-1, 0); 
                    break;
                case ConsoleKey.DownArrow:
                    currentPosition = Math.Min(currentPosition+1, menuOption.Length-1); 
                    break;
                case ConsoleKey.Enter:
                    selectMenuOption();
                    break;

            }
        }
        private void selectMenuOption()
        {
            switch (currentPosition)
            {
                case 0:
                    GameHolder newHolder = new GameHolder();
                    newHolder.LoadLevel();
                    Program.setNewHolder(newHolder);
                    break;
                case 1:
                    Environment.Exit(0);
                    break;

            }
        }
    }
}
