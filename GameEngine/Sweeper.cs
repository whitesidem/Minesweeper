using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class Sweeper
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public GameCell[,] Grid { get; set; }

        public Sweeper(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new GameCell[width, height];
            for (var xCord = 0; xCord < Width; xCord++)
            {
                for (var yCord = 0; yCord < Height; yCord++)
                {
                    Grid[xCord, yCord] = new GameCell(xCord, yCord);
                }
            }
        }

        public int GetSurroundingHitCount(CoOrdinate coOrd)
        {
            var hitCount = 0;
            foreach(var surround in AllVaidBorderCoordinates(coOrd))
            {
                if (Grid[surround.XCord, surround.YCord].IsBomb == true)
                {
                    hitCount++;
                }                    
            }
            return hitCount;
        }

        private static bool IsCurrentPosition(CoOrdinate currCoOrd, CoOrdinate targetCoOrd)
        {
            return currCoOrd.XCord == targetCoOrd.XCord && currCoOrd.YCord == targetCoOrd.YCord;
        }

        private IEnumerable<GameCell> AllGameCells()
        {
            for (var xCord = 0; xCord < Width; xCord++)
            {
                for (var yCord = 0; yCord < Height; yCord++)
                {
                    yield return Grid[xCord, yCord];
                }
            }
        }

        public void ExecutePerGameCell(Action<GameCell> action)
        {
            for (var xCord = 0; xCord < Width; xCord++)
            {
                for (var yCord = 0; yCord < Height; yCord++)
                {
                    action(Grid[xCord, yCord]);
                }
            }
        }

        private IEnumerable<CoOrdinate> AllVaidBorderCoordinates(CoOrdinate coOrd)
        {
            for (var xCord = coOrd.XCord - 1; xCord <= coOrd.XCord + 1; xCord++)
            {
                for (var yCord = coOrd.YCord - 1; yCord <= coOrd.YCord + 1; yCord++)
                {
                    var surroundCoord = new CoOrdinate(xCord, yCord);
                    if (IsCurrentPosition(coOrd, surroundCoord))
                    {
                        continue;
                    }
                    if (IsInsideGrid(surroundCoord) == false)
                    {
                        continue;
                    }
                    yield return surroundCoord;
                }
            }
        }

        private bool IsInsideGrid(CoOrdinate coOrd)
        {
            return (coOrd.XCord >= 0 && coOrd.XCord < Width && coOrd.YCord >= 0 && coOrd.YCord < Height);
        }

        public void AddBombsToGrid(int totalBombs)
        {
            var random = new Random();

            int totalAdded = 0;
            while(totalAdded < totalBombs)
            {
                var randomNumber = random.Next(0, (Width * Height) -1);
                var ypos = randomNumber/Width;
                var xpos = randomNumber - Width*ypos;
                if (Grid[xpos, ypos].IsBomb == false)
                {
                    Grid[xpos, ypos].IsBomb = true;
                    totalAdded++;
                } 
            }
        }

        public int GetTtotalBombs()
        {
            return AllGameCells().Count(cell => cell.IsBomb);
        }

        private void SetAsDug(CoOrdinate coOrd, int hitCount)
        {
            var cell = Grid[coOrd.XCord, coOrd.YCord];
            cell.hasBeenDug = true;
            cell.ProximityCount = SetProximityCount(hitCount);
        }

        private static int SetProximityCount(int hitCount)
        {
            return hitCount > 0 ? hitCount : -1;
        }

        public bool DigAndCheckIsBomb(CoOrdinate coOrd)
        {
            var gridCell = Grid[coOrd.XCord, coOrd.YCord];
            if (gridCell.IsBomb)
            {
                gridCell.IsExplosion = true;
                return true;
            }
            SetRecursiveSurroundingHitCount(coOrd);
            return false;
        }

        public void SetRecursiveSurroundingHitCount(CoOrdinate coOrd)
        {
//            var gridCell = Grid[coOrd.XCord, coOrd.YCord];
//            if (gridCell.IsBomb)
//            {
//                return;
//            }
            var hitCount = GetSurroundingHitCount(coOrd);
            SetAsDug(coOrd, hitCount);
            if (hitCount > 0)
            {
                return;
            }

            foreach (var surround in AllVaidBorderCoordinates(coOrd))
            {
                var surroundGridCell = Grid[surround.XCord, surround.YCord];

                if (surroundGridCell.IsBomb==false && surroundGridCell.hasBeenDug == false && surroundGridCell.ProximityCount == 0)
                {
                    var subHitcount = GetSurroundingHitCount(surroundGridCell.CoOrd);
                    SetAsDug(surroundGridCell.CoOrd, subHitcount);
                    if (subHitcount == 0)
                    {
                        SetRecursiveSurroundingHitCount(surroundGridCell.CoOrd);
                    }
                }

            }
        }
    }
}