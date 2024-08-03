using SuperTanks.Holders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks
{
    internal class Input
    {
        private AbstractHolder Holder;
        internal void setHolder(AbstractHolder holder)
        {
            Holder = holder;
        }

        public void Update()
        {
            ConsoleKey key = ConsoleKey.None;
            while (Console.KeyAvailable)
            {
                key = Console.ReadKey().Key;
            }
            
            Holder.OnPressKey(key);

        }
    }
}
