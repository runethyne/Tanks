using SuperTanks.Holders;
using SuperTanks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Units
{
    public class EnemyLigthTank : EnemyTank
    {
        Random rnd = new Random();

        GameHolder gm;

        public EnemyLigthTank(GameHolder _gm) : base(_gm)
        {
            gfxData = File.ReadAllLines(@".\Data\EnemyLigthTank.txt", Encoding.UTF8);
            sizeUnit = 3;
            animationCount = 2;
            HP = 1;
            HPMax = 1;
            color = ConsoleColor.Magenta;
            Direction = rnd.Next(0, 4);
            gm = _gm;
        }

        public override void shoot()
        {
            actionDelay = 12;
            projectiles.Add(new Projectile(this, w + 1, h + 1, Direction));
        }
    }
}
