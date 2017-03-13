using System;
using GameEngine;

namespace Game
{
    public class GameView
    {
        private readonly Sweeper _sweeper;
        private const int GridLeftPos = 3;
        private const int GridTopPos = 3;
        const int CellXOffset = 4;
        const int CellYOffset = 4;
        private static bool SHOW_BOMB = true;

        public GameView(Sweeper sweeper)
        {
            _sweeper = sweeper;
        }

        public void DrawGrid()
        {
            DrawGridLines();
            _sweeper.ExecutePerGameCell(DrawCell);
        }

        private void DrawGridLines()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            OffSetGridPosition(new CoOrdinate(0, 0), 0, 0, false);
            Console.Write('+');
            _sweeper.ExecutePerGameCell(DrawLines);
            OffSetGridPosition(new CoOrdinate(_sweeper.Width, _sweeper.Height), 0, 0, false);
            Console.Write('+');
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DrawLines(GameCell gameCell)
        {
            var coOrd = gameCell.CoOrd; 

            int pos;
            for (pos = 0; pos < 3; pos++)
            {
                OffSetGridPosition(coOrd, pos+1,0, false);
                Console.Write("-");
            }
            OffSetGridPosition(coOrd, pos+1,0, false);
            Console.Write("+");
            if (coOrd.XCord == _sweeper.Width - 1)
            {
                for (pos = 0; pos < 3; pos++)
                {
                    OffSetGridPosition(new CoOrdinate(coOrd.XCord+1, coOrd.YCord), 0, pos + 1, false);
                    Console.Write("|");
                }
            }
            if (coOrd.XCord == 0)
            {
                OffSetGridPosition(new CoOrdinate(coOrd.XCord, coOrd.YCord), -1, 2, false);
                Console.Write(coOrd.YCord.ToString());
            }


            for (pos = 0; pos < 3; pos++)
            {
                OffSetGridPosition(coOrd, 0,pos+1, false);
                Console.Write("|");
            }
            OffSetGridPosition(coOrd, 0,pos+1, false);
            Console.Write("+");
            if (coOrd.YCord == _sweeper.Height - 1)
            {
                for (pos = 0; pos < 3; pos++)
                {
                    OffSetGridPosition(new CoOrdinate(coOrd.XCord, coOrd.YCord+1), pos + 1, 0, false);
                    Console.Write("-");
                }
            }
            if (coOrd.YCord == 0)
            {
                OffSetGridPosition(new CoOrdinate(coOrd.XCord, coOrd.YCord), 2, -1, false);
                Console.Write(coOrd.XCord.ToString());
            }
        }



        private void DrawCell(GameCell gameCell)
        {
            Console.ForegroundColor = ConsoleColor.White;
            OffSetGridPosition(gameCell.CoOrd, 1, 1);
            if (gameCell.IsExplosion)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("B");
            }
            else if (gameCell.IsBomb && SHOW_BOMB)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("B");
            }
            else if (gameCell.hasBeenDug)
            {
                if (gameCell.ProximityCount == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (gameCell.ProximityCount == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (gameCell.ProximityCount > 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(gameCell.ProximityCount == -1 ? " " : gameCell.ProximityCount.ToString());
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("?");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public int AskYPos()
        {
            OffSetGridPosition(new CoOrdinate(0, _sweeper.Height), 0, 5);
            Console.Write("Enter Y Co-ordinate: ");
            var value = Console.ReadLine();
            OffSetGridPosition(new CoOrdinate(0, _sweeper.Height), 0, 5);
            Console.Write("                                                                    ");
            return Int32.Parse(value);
        }

        public int AskXPos()
        {
            OffSetGridPosition(new CoOrdinate(0, _sweeper.Height), 0, 5);
            Console.Write("Enter X Co-ordinate to dig OR enter q to quit: ");
            var value = Console.ReadLine();
            if (value == "q")
            {
                return -1;
            }
            OffSetGridPosition(new CoOrdinate(0, _sweeper.Height), 0, 5);
            Console.Write("                                                                    ");
            return Int32.Parse(value);
        }

        private static void OffSetGridPosition(CoOrdinate coOrd, int xpos, int ypos, bool includeGridStart = true)
        {
            var x = GridLeftPos +  (includeGridStart ? 1 : 0) + (coOrd.XCord*CellXOffset) + xpos;
            var y = GridTopPos + (includeGridStart ? 1 : 0) + (coOrd.YCord * CellYOffset) + ypos;
            Console.SetCursorPosition(x, y);
        }


        public void SeupWindowPanelDimension()
        {
            Console.Clear();
            var calcWidth = (GridLeftPos*2) + (_sweeper.Width*CellXOffset);
            Console.WindowWidth = Math.Max(calcWidth, 60) ;
            Console.WindowHeight = (GridTopPos*4) + (_sweeper.Height * CellYOffset) ;
        }

        public void YouHitABombMessage(CoOrdinate coOrd)
        {
            DrawGrid();
            OffSetGridPosition(new CoOrdinate(0, _sweeper.Height), 0, 5);
            Console.WriteLine("Oh NO it's a BOMB, hash tag Minesweep Fail!");
        }
    }
}

