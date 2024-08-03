using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperTanks.Holders;
using SuperTanks.Model;
using static System.Net.Mime.MediaTypeNames;

namespace SuperTanks.Units
{
    public class EnemyTank : Unit
    {
        Random rnd = new Random();

        GameHolder gm;

        public int actionDelay;
        int hiding;

        public EnemyTank(GameHolder _gm)
        {
            gfxData = File.ReadAllLines(@".\Data\EnemyMediumTank.txt", Encoding.UTF8);
            sizeUnit = 3;
            animationCount = 2;
            HP = 2;
            HPMax = 2;
            color = ConsoleColor.Magenta;
            Direction = rnd.Next(0, 4);
            gm = _gm;
        }
        public override void update()
        {
            base.update();

            //Наш ИИ, порядок по приоритетам
            // Если на линии стрельбы есть Игрок - стреляем. Макс дистанция = 10
            // Мы двигаемся в направлении, пока не упреся.
            // Если уперлись - меняем дирекцмю
            // Если оказались в кустах - можем спрятаться на N время в засаде и не двигаться

            //Здесь будет хардкон. уберите от экрана детей

            actionDelay = Math.Max(0, actionDelay-1);

            if (actionDelay == 0)
            {
                //проверяем есть ли цель

                if (!gm.inTree(gm.player))
                {

                    int firePointX = w + 1;
                    int firePointy = h + 1;

                    int startW = Math.Max(gm.player.w, 0);
                    int startH = Math.Max(gm.player.h, 0);
                    int endW = Math.Min(gm.player.w + 2, gm.map.Length - 1);
                    int endH = Math.Min(gm.player.h + 2, gm.map.Length - 1);

                    for (int x = startW; x <= endW; x++)
                    {
                        for (int y = startH; y <= endH; y++)
                        {
                            if (x == firePointX)
                            {
                                if (y > firePointy)
                                {
                                    Direction = 1;
                                    shoot();
                                    return;
                                }
                                if (y < firePointy)
                                {
                                    Direction = 3;
                                    shoot();
                                    return;
                                }
                            }
                            if (y == firePointy)
                            {
                                if (x > firePointX)
                                {
                                    Direction = 2;
                                    shoot();
                                    return;
                                }
                                if (x < firePointX)
                                {
                                    Direction = 0;
                                    shoot();
                                    return;
                                }
                            }
                        }
                    }
                }
                //чек на кусты
                //если мы в кустах - то с шансом в 20% решим спрятаться
                if (hiding <= 0 && gm.inTree(this) && rnd.Next(0,100) < 20)
                {
                    hiding = 40 + rnd.Next(0, 20);
                }

                if (hiding > 0)
                {
                    hiding -= 1;
                    return;
                }

                //Движение и смена направления
                int directionX = 0;
                int directionY = 0;

                switch (Direction)
                {
                    case 0:
                        directionX = -1;
                        break;
                    case 1:
                        directionY = 1;
                        break;
                    case 2:
                        directionX = +1;
                        break;
                    case 3:
                        directionY = -1;
                        break;
                }

                if (!gm.tryMove(this, directionX, directionY))
                {
                    actionDelay = 3;
                    Direction = rnd.Next(0, 4);
                }
                else
                {
                    actionDelay = 2;
                }
            }
        }
        public override void shoot()
        {
            actionDelay = 12;
            projectiles.Add(new Projectile(this, w + 1, h + 1, Direction));
        }

    }
}
