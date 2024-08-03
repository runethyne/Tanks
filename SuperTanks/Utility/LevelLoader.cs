using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Utility
{
    public static class LevelLoader
    {
        public static int[][] Load(string lvlName)
        {
            String[] levelData = File.ReadAllLines(@".\Data\Levels\" + lvlName + ".txt", Encoding.UTF8);
            int[][] map = new int[levelData.Length][];
            for (int i = 0; i < levelData.Length; i++)
            {
                string lvlDataRaw = levelData[i].Replace("\t", "");

                map[i] = new int[lvlDataRaw.Length];
                for (int j = 0; j < lvlDataRaw.Length; j++)
                {
                    int typeCellMap = Int32.Parse(lvlDataRaw[j].ToString());

                    map[i][j] = typeCellMap;
                }
            }

            return map;

        }
    }
}
