using SuperTanks.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Holders
{
    internal class TestHolder
    {
       /* public void st()
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PlayerTank pt = new PlayerTank();
            EnemyTank et = new EnemyTank();
            int index = 0;
            while (true)
            {

                index++;

                if (index == 2) index = 0;

                Console.ForegroundColor = ConsoleColor.Red;

                Console.SetCursorPosition(0, 0);
                string[] gfxdata = pt.getGFX();
                foreach (string strgf in gfxdata)
                    Console.WriteLine(strgf);
                pt.directionNextStep();


                Console.SetCursorPosition(0, 4);
                gfxdata = pt.getGFX();
                foreach (string strgf in gfxdata)
                    Console.WriteLine(strgf);
                pt.directionNextStep();


                pt.animNextStep();

                int x = 4;
                int y = 0;
                gfxdata = et.getGFX();
                foreach (string strgf in gfxdata)
                {
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine(strgf);
                    y++;
                }
                et.directionNextStep();

                x = 9;
                y = 0;
                gfxdata = et.getGFX();
                foreach (string strgf in gfxdata)
                {
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine(strgf);
                    y++;
                }
                et.directionNextStep();
                et.directionNextStep();
                et.directionNextStep();


                et.animNextStep();

                Console.CursorVisible = false;
                Thread.Sleep(250);
                // Console.Clear();


            }




        }
       */
    }
}
