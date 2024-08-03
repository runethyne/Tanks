using SuperTanks.Model;
using SuperTanks.Units;
using SuperTanks.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Holders
{
    public class GameHolder : AbstractHolder
    {
        public int[][] map;
        public Unit player;
        public List<Unit> units = new List<Unit>();

        private string[] _levels = new string[] { "Level_1", "Level_2", "Level_3", "Level_4", "Level_5" };
        private string[] _music = new string[] { "battletheme_01", "battletheme_01", "battletheme_01", "battletheme_01", "bossFigth" };
        private int level = 0;

        bool playerCheat = false;

        bool gameLose = false;
        bool gameWin = false;

        int gamePause = 0; //если > 0 то на данное количество кадров будет висеть пауза
        string[] ScreenText;

        public void LoadLevel()
        {
            LoadLevel(_levels[level]);
        }
       
        public void LoadLevel(string lvlName)
        {
            gameLose = false;
            gameWin = false;
            player = null;
            units.Clear();

            map = LevelLoader.Load(lvlName);
            for (int w= 0; w < map.Length; w++)
            {
                for (int h= 0; h < map[w].Length; h++)
                {
                    if (map[w][h] == 5 && player == null)
                    {
                        player = new PlayerTank();
                        player.setCoords(w,h);
                        units.Add(player);
                        break;
                    }
                    if (map[w][h] == 6)
                    {
                        Unit enemy = new EnemyTank(this);
                        enemy.setCoords(w, h);
                        units.Add(enemy);
                        break;
                    }
                    if (map[w][h] == 7)
                    {
                        Unit enemy = new EnemyLigthTank(this);
                        enemy.setCoords(w, h);
                        units.Add(enemy);
                        break;
                    }
                    if (map[w][h] == 8)
                    {
                        Unit enemy = new EnemyHeavyTank(this);
                        enemy.setCoords(w, h);
                        units.Add(enemy);
                        break;
                    }

                }
            }
            gamePause = 10;
            ScreenText = new string[] { "---------------", "READY TO BATTLE", "---------------" };
            MusicManager.stopBackgroundMusic();
            MusicManager.playBackgroundMusic(_music[level]);

        }

        public override string[] getScreenData()
        {
            if (gamePause > 0 && ScreenText.Length > 0)
                return ScreenText;

            return base.getScreenData();
        }

        public override int[][] getMapData()
        {
            return map;
        }
        public override Unit[] getUnits()
        {
            return units.ToArray();
        }
        public override string getStatusString()
        {
            string str = "ARMOR ";
            for (int i = 0; i < player.HP; i++)
                str += "▰";
            for (int i = 0; i < player.HPMax-player.HP; i++)
                str += "▱";
            
            str += " || GUN ";
            int loaded = 9 - (int) player.shootCouldown;

            for (int i = 0; i < loaded; i++)
                str += "▰";
            for (int i = 0; i < 9-loaded; i++)
                str += "▱";
            
            str += " || LVL "+(level+1);

            return str;
        }
        public override void Update()
        {
            cameraPositionX = player.w;
            cameraPositionY = player.h;

            updateGameState();

            gamePause = Math.Max(0, gamePause-1);
            if (gamePause > 0)
                return;

            foreach (Unit unit in units) {

                if (unit.isAlive())
                    unit.update();

                foreach (Projectile projectile in unit.projectiles.ToArray())
                {
                    projectile.w += projectile.directionX;
                    projectile.h += projectile.directionY;

                    if (projectile.w >= map.Length || projectile.w < 0 || projectile.h >= map.Length || projectile.h < 0)
                    {
                        unit.projectiles.Remove(projectile);
                        continue;
                    }

                    //чек колизии со стенами
                    switch (map[projectile.w][projectile.h]) 
                    { 
                        case 1:
                        case 2:
                            breakWall(projectile.w, projectile.h);
                            unit.projectiles.Remove(projectile);
                            continue;
                    }
                    //чек с Другими танками
                    foreach (Unit target in units) {
                        if (!target.isAlive())
                            continue;
                        if (target == projectile.owner)
                            continue;

                        bool hasTouch = false;

                        for (int i = 0; i < target.sizeUnit; i++)
                        {
                            for (int j = 0; j < target.sizeUnit; j++)
                            {
                                if (projectile.w == target.w+i && projectile.h == target.h+j)
                                {
                                    hasTouch = true;
                                }

                            }
                        }

                        if (hasTouch)
                        {
                            unit.projectiles.Remove(projectile);
                            if (playerCheat && target == player)
                                break;
                            target.takeDamage(1);
                            break;
                        }
                    }
                }
            }
        }

        private void updateGameState()
        {
            if (gameLose || gameWin)
            {
                if (gameWin && gamePause <= 0)
                {
                    level++;
                    if (level >= _levels.Length)
                    {
                        Program.setNewHolder(new MainMenuHolder());
                        return;
                    }
                    LoadLevel(_levels[level]);
                    return;
                }
                if (gameLose && gamePause <= 0)
                {

                    Program.setNewHolder(new MainMenuHolder());
                    return;
                }
                return;
            }
            //проверяем победу и поражение
            if (player.HP <= 0)
            {
                gamePause = 20;
                ScreenText = new string[] { "---------", "-YOU DIE-", "---------", };
                gameLose = true;
                MusicManager.stopBackgroundMusic();
                //MusicManager.playBackgroundMusic("lose.wav");

            }
            
            if (player.HP > 0 && units.Where(u => u.isAlive() && u != player).Count() == 0)
            {
                gamePause = 20;
                ScreenText = new string[] { "---------", "-YOU WIN-", "---------", };
                gameWin = true;
                MusicManager.stopBackgroundMusic();
                //MusicManager.playBackgroundMusic("win.wav");

            }
        }

        private void breakWall(int w, int h)
        {
            //взрыв 3х3
            int startW = Math.Max(w - 1,0);
            int startH = Math.Max(h - 1, 0); 
            int endW = Math.Min(w + 1, map.Length - 1);
            int endH = Math.Min(h + 1, map.Length - 1);

            for (int x = startW; x <= endW; x++)
            {
                for (int y = startH; y <= endH; y++)
                {
                    switch (map[x][y])
                    {
                        case 1:
                            map[x][y] = 0;
                            break;
                        case 2:
                            map[x][y] -= 1;
                            break;
                    }
                }
            }
        }

        public override void OnPressKey(ConsoleKey key)
        {
            if (gamePause > 0)
                return;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    tryMove(player, 0, -1);
                    player.setDirection(3);
                    break;
                case ConsoleKey.DownArrow:
                    tryMove(player, 0, +1);
                    player.setDirection(1);
                    break;
                case ConsoleKey.LeftArrow:
                    tryMove(player, -1, 0);
                    player.setDirection(0);
                    break;
                case ConsoleKey.RightArrow:
                    tryMove(player, +1, 0);
                    player.setDirection(2);
                    break;
                case ConsoleKey.C:
                    playerCheat = true;
                    break;
                case ConsoleKey.Spacebar:
                    player.shoot();
                    break;
            }
        }

        public bool tryMove(Unit unit, int w, int h)
        {
            bool cheak = true;
            int sizeUnit = unit.sizeUnit;

            for (int x = 0; x < sizeUnit; x++)
            {
                for (int y = 0; y < sizeUnit; y++) {

                    int checkCoordX = x + w + unit.w;
                    int checkCoordY = y + h + unit.h;

                    if (checkCoordX >= map.Length || checkCoordY >= map.Length || checkCoordX < 0 || checkCoordY < 0)
                    {
                        cheak = false;
                        return cheak;
                    }
                        
                    switch (map[checkCoordX][checkCoordY])
                    {
                        case 1:
                        case 2:
                        case 3:
                            cheak = false; ; break;
                        default:
                            break;
                    }

                }
            }
            foreach (Unit collisionUnit in units)
            {
                if (collisionUnit == unit || !collisionUnit.isAlive()) 
                    continue;
                //проверяем на наличие общих координат
                for (int x = 0; x < sizeUnit; x++)
                    for (int y = 0; y < sizeUnit; y++)
                        for (int colX = 0; colX < collisionUnit.sizeUnit; colX++)
                            for (int colY = 0; colY < collisionUnit.sizeUnit; colY++)
                                if (unit.w + x +w == collisionUnit.w + colX && unit.h + y + h == collisionUnit.h + colY)
                                {
                                    cheak = false;
                                    return cheak;
                                }
            }       
        


            if (cheak)
            {
                unit.w += w;
                unit.h += h;
            }

            return cheak;
        }

        public bool inTree(Unit unit)
        {
            bool cheak = true;
            int sizeUnit = unit.sizeUnit;

            for (int x = 0; x < sizeUnit; x++)
            {
                for (int y = 0; y < sizeUnit; y++)
                {

                    int checkCoordX = x +  unit.w;
                    int checkCoordY = y +  unit.h;

                    if (checkCoordX >= map.Length || checkCoordY >= map.Length || checkCoordX < 0 || checkCoordY < 0)
                    {
                        cheak = false;
                        return cheak;
                    }

                    switch (map[checkCoordX][checkCoordY])
                    {
                        case 0:
                            cheak = false; break;
                        default:
                            break;
                    }

                }
            }
            
            return cheak;
        }
    }
}
