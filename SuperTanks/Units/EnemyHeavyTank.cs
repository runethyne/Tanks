using SuperTanks.Holders;
using SuperTanks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Units
{
    public class EnemyHeavyTank : EnemyTank
    {
        Random rnd = new Random();

        GameHolder gm;

        public EnemyHeavyTank(GameHolder _gm) : base(_gm)
        {
            gfxData = File.ReadAllLines(@".\Data\EnemyHeavyTank.txt", Encoding.UTF8);
            sizeUnit = 4;
            animationCount = 2;
            HP = 8;
            HPMax = 8;
            color = ConsoleColor.DarkMagenta;
            Direction = rnd.Next(0, 4);
            gm = _gm;
        }
        public override void shoot()
        {
            actionDelay = 24;
            projectiles.Add(new Projectile(this, w + 1, h + 1, Direction));
            projectiles.Add(new Projectile(this, w + 2, h + 2, Direction));
        }
    }
}
