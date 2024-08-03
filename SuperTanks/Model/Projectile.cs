
namespace SuperTanks.Model
{
    public class Projectile
    {
        public Unit owner; //снаряд не может ранить только владельца. остальных можно френдли фаер возможен)
        public int w;
        public int h;

        public char gfx;
        public ConsoleColor color;

        public int directionX;
        public int directionY;

        public Projectile(Unit unit, int w, int h, int direction)
        {
            this.owner = unit;
            this.w = w;
            this.h = h;

            color = unit.color;

            switch (direction)
            {
                case 0:
                    directionX = -1;
                    gfx = '◀';
                    break;
                case 1:
                    directionY = 1;
                    gfx = '▼';
                    break;
                case 2:
                    directionX = +1;
                    gfx = '▶';
                    break;
                case 3:
                    directionY = -1;
                    gfx = '▲';
                    break;
            }
           
        }

    }
}