using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperTanks.Model;
using static System.Net.Mime.MediaTypeNames;

namespace SuperTanks.Units
{
    public class PlayerTank : Unit
    {

        public PlayerTank()
        {
            gfxData = File.ReadAllLines(@".\Data\PlayerTank.txt", Encoding.UTF8);
            sizeUnit = 3;
            animationCount = 2;
            HP = 10;
            HPMax = 10;
            color = ConsoleColor.DarkRed;
        }

        public override void shoot()
        {
            if (shootCouldown > 7)
            {
                return;
            }
            projectiles.Add(new Projectile(this, w + 1, h + 1, Direction));//при стрельбе с танка 3х3 выстрел будет из центра. Для танка 4х4 реализована своя механика
            shootCouldown += 3;
        }
        public override void update()
        {
            shootCouldown = Math.Max(0, shootCouldown - 0.4f);
            base.update();
        }


    }
}
