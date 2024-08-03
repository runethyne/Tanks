using SuperTanks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Holders
{
    public abstract class AbstractHolder
    {
        public int cameraPositionX = 0;
        public int cameraPositionY = 0;

        public virtual void Update() { }
        public virtual string[][] getMenuData() { return new string[][]{ }; }
        public virtual int[][] getMapData() { return null; }
        public virtual Unit[] getUnits() { return null;  }
        public virtual void OnPressKey(ConsoleKey key) { }
        public virtual string[] getScreenData() { return new string[] { }; }
        public virtual string getStatusString() { return ""; }
    }
}
