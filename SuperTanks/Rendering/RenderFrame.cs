using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Rendering
{
    public class RenderFrame
    {
        public char[,] mapChar;
        public ConsoleColor[,] backgroundColor;
        public ConsoleColor[,] foregroundColor;

        public RenderFrame(int w, int h)
        {
            mapChar = new char[w, h];
            backgroundColor = new ConsoleColor[w, h];
            foregroundColor = new ConsoleColor[w, h];
        }

        public void setPixel(int x, int y, char c, ConsoleColor bColor, ConsoleColor fColor)
        {
            if (x  >= mapChar.GetLength(0) || x  < 0 || y >= mapChar.GetLength(1) || y < 0)
                return;
            mapChar[x, y] = c;
            backgroundColor[x, y] = bColor;
            foregroundColor[x, y] = fColor;
        }
        public void setPixel(int x, int y, char c, ConsoleColor fColor)
        {
            if (x >= mapChar.GetLength(0) || x < 0 || y >= mapChar.GetLength(1) || y < 0)
                return;
            mapChar[x, y] = c;
            foregroundColor[x, y] = fColor;

        }

        public int setPixels(int x, int y, string s, ConsoleColor bColor, ConsoleColor fColor)
        {
            int i;
            for (i = 0; i < s.Length; i++)
            {
                if (x + i >= mapChar.GetLength(0) || x + i < 0 || y >= mapChar.GetLength(1) || y < 0)
                    continue;
                mapChar[x + i, y] = s[i];
                backgroundColor[x + i, y] = bColor;
                foregroundColor[x + i, y] = fColor;
            }
            return i;
        }

        public void setSize(int w, int h)
        {
            mapChar = new char[w, h];
            backgroundColor = new ConsoleColor[w, h];
            foregroundColor = new ConsoleColor[w, h];
        }

        
    }
}
