using SuperTanks.Holders;
using SuperTanks.Model;
using SuperTanks.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTanks.Rendering
{
    public class Render
    {
        private AbstractHolder Holder;
        private int windowWidth = Console.WindowWidth;
        private int windowHeight = Console.WindowHeight;

        private RenderFrame rFrame = new RenderFrame(120, 60);

        private string wallSymbols = "⊞╡╥╗╞═╔╦╨╝║╣╚╩╠╬";
        private char waterSymbol = '▒';
        private char treeSymbol = '◊';
        private char borderSymbol = ' ';

        private int EnvironmentAnimationState = 0;
        //Логика выбора сивола такова, что каждый символ имеет порядковый номер, который получается из наличия связей и направления этой связи в дыоичном формате
        // пример, Слева нет связи, внизу есть, справа есть, сверху нет. получится 0110 - wallSymbols[6] = ╔

        public void Rendering()
        {
            //Пайплайн такой:
            //1 - Чек смены разрешения. Если да - то клиним консоль
            //2 - Отрисовка игры, по слоям
            //  --Рисовка карты
            //  --Рисовка подвижных Объектов (Танки, снаряд, ш**гу, гер****ов)
            //  --Рисовка эффектов (следы, линия полета, прятки в кустах (кусты = невидимость, после такого игра должна попасть в PS Store))
            //  --Интерфейс
            //3 - Меню, если есть

            //1 Если мы поменяли разрешение, то зачищаем консоль
            if (windowWidth != Console.WindowWidth || windowHeight != Console.WindowHeight)
            {
                windowWidth = Console.WindowWidth;
                windowHeight = Console.WindowHeight;
                Console.SetBufferSize(windowWidth, windowHeight);
                Console.Clear();
                rFrame.setSize(windowWidth, windowHeight);
            }

            //2 Отрисовка карты и юнитов
            EnvironmentAnimationState++;
            if (EnvironmentAnimationState == 10) { EnvironmentAnimationState = 0; }
            
            int[][] mapData = Holder.getMapData();
            
            if (mapData != null) {
                int width = Math.Min(mapData[0].Length, windowWidth-1);
                int height = Math.Min(mapData.Length, windowHeight-1);

                //Оффсеты для того тчо бы "двигать карту" и держать по центру наш танк
                int cameraOffcetY = Holder.cameraPositionY+1 - (windowHeight/2);
                cameraOffcetY = Math.Max(cameraOffcetY, 0);
                cameraOffcetY = Math.Min(cameraOffcetY, mapData[0].Length - height);
               
                int cameraOffcetX = Holder.cameraPositionX+1 - (windowWidth / 2);
                cameraOffcetX = Math.Max(cameraOffcetX, 0);
                cameraOffcetX = Math.Min(cameraOffcetX, mapData.Length - width);


                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        switch (mapData[w + cameraOffcetX][h+ cameraOffcetY])
                        {
                            case 1:
                                rFrame.setPixel(w, h, getWallSymbol(mapData, w + cameraOffcetX, h + cameraOffcetY), ConsoleColor.Black, ConsoleColor.DarkGray);
                                break;
                            case 2:
                                rFrame.setPixel(w, h, getWallSymbol(mapData, w + cameraOffcetX, h + cameraOffcetY), ConsoleColor.Black, ConsoleColor.Gray);
                                break;
                            case 3:
                                rFrame.setPixel(w, h, waterSymbol, ConsoleColor.Cyan, EnvironmentAnimationState > 5 ? ConsoleColor.Blue : ConsoleColor.DarkBlue);
                                break;
                            default:
                                rFrame.setPixel(w, h, ' ', ConsoleColor.Black, ConsoleColor.White);
                                break;
                        }
                    }
                }
                //2 Рисовка подвижных Объектов
                Unit[] units = Holder.getUnits();
                if (units != null)
                {
                    for (int i = 0; i < units.Length; i++)
                    {
                        if (units[i].isAlive())
                        {
                            units[i].animNextStep();

                            string[] gfxData = units[i].getGFX();
                            int rowCount = 0;
                            foreach (string gfxRow in gfxData)
                            {
                                rFrame.setPixels(units[i].w - cameraOffcetX, units[i].h - cameraOffcetY + rowCount, gfxRow, ConsoleColor.Black, units[i].getColor());
                                rowCount++;
                            }
                        }
                        foreach (Projectile p in units[i].projectiles)
                        {
                            rFrame.setPixel(p.w - cameraOffcetX, p.h - cameraOffcetY, p.gfx, p.color);
                        }
                    }
                }

                //отрисовка леса
                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {

                        switch (mapData[w + cameraOffcetX][h + cameraOffcetY])
                        {
                            case 4:
                                rFrame.setPixel(w, h, treeSymbol, ConsoleColor.Black, EnvironmentAnimationState > 5 ? ConsoleColor.Green : ConsoleColor.DarkGreen);
                                break;
                        }


                        if (w == 0 || h == 0 || w+1 == width  || h+1 == height )
                        {
                            rFrame.setPixel(w, h, borderSymbol, ConsoleColor.White, ConsoleColor.White);

                            continue;
                        }
                    }
                }

                //отрисовка интерфейса

                string statusLine = Holder.getStatusString();
                rFrame.setPixels(0, rFrame.mapChar.GetLength(1)-1, statusLine, ConsoleColor.Black, ConsoleColor.Yellow);
            }
            

            //3 Отрисовка Меню, если есть
            string[][] interfaceData = Holder.getMenuData();

            int offsetX = windowWidth / 2;
            int offsetY = windowHeight / 2 - interfaceData.Length/2;
            
            int index = 0;
            
            
            foreach (string[] interfaceDataItem in interfaceData)
            {
                int posW = offsetX - interfaceDataItem[1].Length / 2;
                int powH = offsetY + index;
                
                rFrame.setPixels(posW, powH, interfaceDataItem[0]+ interfaceDataItem[1], ConsoleColor.Black, ConsoleColor.White);
                
                index++;
            }
            // отрисовка скрин инфы
            string[] screenData = Holder.getScreenData();
            offsetX = Math.Min(windowWidth / 2, rFrame.mapChar.GetLength(0));
            offsetY = windowHeight / 2 - screenData.Length / 2;
            foreach (string screenDataItem in screenData)
            {
                int posW = offsetX - screenDataItem.Length / 2;
                int powH = offsetY + index;

                rFrame.setPixels(posW-4, powH-2, screenDataItem, ConsoleColor.Black, ConsoleColor.White);

                index++;
            }


            //вывод кадра


            for (int w = 0; w < windowWidth; w++)
            {
                for(int h = 0; h < windowHeight; h++)
                {
                    //При смене ширины и высоты автоматом меняется буфер...
                    //так что просто избавимся от ошибок
                    if (w >= Console.WindowWidth || h >= Console.WindowHeight)
                        continue;
                  
                    try
                    {
                        Console.SetCursorPosition(w, h);
                        Console.BackgroundColor = rFrame.backgroundColor[w, h];
                        Console.ForegroundColor = rFrame.foregroundColor[w, h];
                        Console.Write(rFrame.mapChar[w, h]);
                    }
                    catch { }
                    
                }
            }
            Console.SetCursorPosition(0, 0);

        }

        public void setHolder(AbstractHolder holder)
        {
            Holder = holder;
            rFrame.setSize(windowWidth, windowHeight);
            Console.SetBufferSize(windowWidth, windowHeight);
            Console.Clear();
        }


        private char getWallSymbol(int[][] mapData, int w, int h)
        {
            int indexSymbol = 0;

            if (w > 0 && (mapData[w - 1][h] == 1 || mapData[w - 1][h] == 2))
            {
                indexSymbol += 1; //0001
            }
            if (h < mapData.Length - 1 && (mapData[w][h + 1] == 1 || mapData[w][h + 1] == 2))
            {
                indexSymbol += 2;//0010
            }
            if (w < mapData[0].Length-1 && (mapData[w + 1][h] == 1 || mapData[w + 1][h] == 2))
            {
                indexSymbol += 4;//0100
            }
            if (h > 0 && (mapData[w][h - 1] == 1 || mapData[w][h - 1] == 2))
            {
                indexSymbol += 8;//1000
            }

            return wallSymbols[indexSymbol];
            // ⊞ ╡ ╥ ╗ ╞ ═ ╔ ╦ ╨ ╝ ║  ╣  ╚  ╩  ╠  ╬ 
            // 0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15
        }

    }
}
