using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Model
{
    public abstract class Unit
    {
        public int w = 0;
        public int h = 0;

        public int HPMax = 0;
        public int HP = 0;
        private bool isHitting = false;

        public String[] gfxData;
        public int sizeUnit;
        protected int animationCount;

        public ConsoleColor color;
        private protected ConsoleColor onHitcolor = ConsoleColor.White;
       
        protected int AnimateState = 0;
        protected int Direction = 0;

        public List<Projectile> projectiles = new List<Projectile>();
        public float shootCouldown;

        public ConsoleColor getColor()
        {
            if (isHitting)
            {
                return onHitcolor;
            }
            return color;
        }

        public void setCoords(int _w, int _h)
        {
            w = _w;
            h = _h;
        }
        public virtual void shoot()
        {
            projectiles.Add(new Projectile(this, w + 1, h + 1, Direction));//при стрельбе с танка 3х3 выстрел будет из центра. Для танка 4х4 реализована своя механика
        }
        public virtual void update() 
        {
            isHitting = false;
        }

        public void animNextStep()
        {
            AnimateState++;
            if (AnimateState == animationCount)
            {
                AnimateState = 0;
            }
        }

        public void setDirection(int _direction)
        {
            Direction = _direction;
        }

        public string[] getGFX()
        {

            int animStateOffset = AnimateState * sizeUnit;
            int directionOffset = Direction * sizeUnit;

            string[] gfx = new String[sizeUnit];

            for (int i = 0; i < sizeUnit; i++)
            {
                gfx[i] = gfxData[animStateOffset + i].Substring(directionOffset, sizeUnit);
            }

            return gfx;
        }

        public void takeDamage(int _damage)
        {
            HP -= _damage;
            isHitting = true;
        }

        public bool isAlive()
        {
            return HP > 0;
        }
    }
}
