using System;
using GameEngine;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {

            var sweeper = SetupBoardBasedOnUserInput();
            var gameView = new GameView(sweeper);
            gameView.SeupWindowPanelDimension();

            do
            {
                gameView.DrawGrid();
                var coOrd = GetCoordinateInputOrQuitCommand(gameView);
                if (coOrd==null) break;

                if (sweeper.DigAndCheckIsBomb(coOrd))
                {
                    gameView.YouHitABombMessage(coOrd);
                    break;
                }


            } while (true);

        }


        private static Sweeper SetupBoardBasedOnUserInput()
        {
            Console.Clear();
            Console.WriteLine("Enter Length:");
            int length = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter Height:");
            int height = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter Total Bombs:");
            int totalBombs = Int32.Parse(Console.ReadLine());

            var sweeper = new Sweeper(length, height);
            sweeper.AddBombsToGrid(totalBombs);
            return sweeper;
        }

        private static CoOrdinate GetCoordinateInputOrQuitCommand(GameView gameView)
        {
            var x = gameView.AskXPos();
            if(x==-1) return null;
            var y = gameView.AskYPos();
            return new CoOrdinate(x,y);
        }

    }
}
