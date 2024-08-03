using SuperTanks.Holders;
using SuperTanks.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperTanks
{
    internal class Program
    {
        private static Render render = new Render();
        private static Input input = new Input();
        private static AbstractHolder holder;

        public static void setNewHolder(AbstractHolder newHolder)
        {
            Console.ResetColor();
            Console.Clear();
            holder = newHolder;
            render.setHolder(newHolder);
            input.setHolder(newHolder);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            Console.SetWindowSize(60, 30);

            setNewHolder(new MainMenuHolder());

            while (true)
            {
                input.Update();
                holder.Update();
                MusicManager.update();
                render.Rendering();

                Thread.Sleep(15);
            }
        }
    }
}