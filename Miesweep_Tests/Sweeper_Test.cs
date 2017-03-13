using GameEngine;
using NUnit.Framework;

namespace Minesweeper_Tests
{
    [TestFixture]
    public class Sweeper_Test
    {
        private Sweeper _sweeper;

        private void SetUpThreeByThreeGrid()
        {
            SetUpGrid(3, 3);
        }


        private void SetUpGrid(int length, int height)
        {
            _sweeper = new Sweeper(length, height);
        }


        [Test]
        public void GetSurroundngHitCount_WhenInMiddleOfGird_WithNoHits_Returns0Hits()
        {
            //arrange
            SetUpThreeByThreeGrid();

            //act
            var hitCount = _sweeper.GetSurroundingHitCount(new CoOrdinate(1, 1));

            //act
            Assert.That(hitCount, Is.EqualTo(0));
        }

        [Test]
        public void Dig_WhenNoSurrounding_SetsProximityCountAsMinusOne()
        {
            //arrange
            SetUpThreeByThreeGrid();

            //act
            _sweeper.DigAndCheckIsBomb(new CoOrdinate(1, 1));

            //act
            Assert.That(_sweeper.Grid[1,1].ProximityCount, Is.EqualTo(-1));
        }



        [Test]
        public void GetSurroundngHitCount_WhenInMiddleOfGird_With1Hit_Return1Hit()
        {
            //arrange
            SetUpThreeByThreeGrid();
            _sweeper.Grid[2, 2].IsBomb = true;

            //act
            var hitCount = _sweeper.GetSurroundingHitCount(new CoOrdinate(1, 1));

            //act
            Assert.That(hitCount, Is.EqualTo(1));
        }



        [Test]
        public void GetSurroundngHitCount_WhenInMiddleOfGird_WithAllBombsSurrounding_Return8Hit()
        {
            //arrange
            SetUpThreeByThreeGrid();
            _sweeper.Grid[0, 0].IsBomb = true;
            _sweeper.Grid[0, 1].IsBomb = true;
            _sweeper.Grid[0, 2].IsBomb = true;
            _sweeper.Grid[1, 0].IsBomb = true;
            _sweeper.Grid[1, 1].IsBomb = true;
            _sweeper.Grid[1, 2].IsBomb = true;
            _sweeper.Grid[2, 0].IsBomb = true;
            _sweeper.Grid[2, 1].IsBomb = true;
            _sweeper.Grid[2, 2].IsBomb = true;

            //act
            var hitCount = _sweeper.GetSurroundingHitCount(new CoOrdinate(1, 1));

            //act
            Assert.That(hitCount, Is.EqualTo(8));
        }

        [Test]
        public void GetSurroundngHitCount_WhenInMiddleOfGird_WithAllBombsInCorners_Return4Hit()
        {
            //arrange
            SetUpThreeByThreeGrid();
            _sweeper.Grid[0, 0].IsBomb = true;
            _sweeper.Grid[0, 2].IsBomb = true;
            _sweeper.Grid[2, 0].IsBomb = true;
            _sweeper.Grid[2, 2].IsBomb = true;

            //act
            var hitCount = _sweeper.GetSurroundingHitCount(new CoOrdinate(1, 1));

            //act
            Assert.That(hitCount, Is.EqualTo(4));
        }


        [Test]
        public void Dig_WhenNoSurrounding_WhenInMiddleOfGird_WithAllBombsInCorners_SetsproximityCountAs4()
        {
            //arrange
            SetUpThreeByThreeGrid();
            _sweeper.Grid[0, 0].IsBomb = true;
            _sweeper.Grid[0, 2].IsBomb = true;
            _sweeper.Grid[2, 0].IsBomb = true;
            _sweeper.Grid[2, 2].IsBomb = true;

            //act
            _sweeper.DigAndCheckIsBomb(new CoOrdinate(1, 1));

            //act
            Assert.That(_sweeper.Grid[1, 1].ProximityCount, Is.EqualTo(4));
        }


        [TestCase(0,0)]
        [TestCase(3,0)]
        [TestCase(0,3)]
        [TestCase(3,3)]
        public void GetSurroundngHitCount_WhenInCornersOfGird_WithAllBombsSurroundingCorners_Return3Hit(int xpos, int ypos)
        {
            //arrange
            SetUpGrid(4, 4);
            //surround top left corner 
            _sweeper.Grid[0, 1].IsBomb = true;
            _sweeper.Grid[1, 0].IsBomb = true;
            _sweeper.Grid[1, 1].IsBomb = true;

            //surround bottom left corner 
            _sweeper.Grid[2, 0].IsBomb = true;
            _sweeper.Grid[2, 1].IsBomb = true;
            _sweeper.Grid[3, 1].IsBomb = true;

            //surround top right corner 
            _sweeper.Grid[0, 2].IsBomb = true;
            _sweeper.Grid[1, 2].IsBomb = true;
            _sweeper.Grid[1, 3].IsBomb = true;

            //surround bottom right corner 
            _sweeper.Grid[2, 2].IsBomb = true;
            _sweeper.Grid[2, 3].IsBomb = true;
            _sweeper.Grid[3, 2].IsBomb = true;


            //act
            var hitCount = _sweeper.GetSurroundingHitCount(new CoOrdinate(xpos, ypos));

            //act
            Assert.That(hitCount, Is.EqualTo(3));
        }

        [TestCase(3,3,1)]
        [TestCase(5,2,3)]
        [TestCase(2,5,3)]
        [TestCase(5,5,18)]
        public void AddBombsToGrid_AddsCorrectNumberOfBombs(int width, int height, int totalBombs)
        {
            //arrange
            var sweeper = new Sweeper(width, height);

            //act
            sweeper.AddBombsToGrid(totalBombs);

            //assert
            var actualBombs = sweeper.GetTtotalBombs();
            Assert.That(actualBombs, Is.EqualTo(totalBombs));
        }


        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        public void GetRecursiveSurroundngHitCount_WithControlledGrid_WhenHitaroundSameArea_SetSameCorrectHitNumbers(int xpos, int ypos)
        {
            //arrange
            SetUpGrid(5, 5);
            _sweeper.Grid[4, 1].IsBomb = true;
            _sweeper.Grid[4, 2].IsBomb = true;
            _sweeper.Grid[0, 3].IsBomb = true;
            _sweeper.Grid[1, 3].IsBomb = true;
            _sweeper.Grid[2, 3].IsBomb = true;
            _sweeper.Grid[4, 3].IsBomb = true;
            _sweeper.Grid[1, 4].IsBomb = true;

            /*
             * Expected:
             * 
             *  0 0 0 1 ?
             *  0 0 0 2 B
             *  2 3 2 3 B
             *  B B B ? B
             *  ? B ? ? ?
             * 
            */

            //act
            _sweeper.DigAndCheckIsBomb(new CoOrdinate(xpos, ypos)) ;

            //act
            AssertGridCell(0, 0,-1);
            AssertGridCell(0,0,-1);
            AssertGridCell(1,0,-1);
            AssertGridCell(2,0,-1);
            AssertGridCell(3,0,1);
            AssertGridCell(4,0,0);
            AssertGridCell(0,1,-1);
            AssertGridCell(1,1,-1);
            AssertGridCell(2,1,-1);
            AssertGridCell(3,1,2);
            AssertGridCell(4,1,0);
            AssertGridCell(0,2,2);
            AssertGridCell(1,2,3);
            AssertGridCell(2,2,2);
            AssertGridCell(3,2,4);
            AssertGridCell(4,2,0);
            AssertGridCell(0,3,0);
            AssertGridCell(1,3,0);
            AssertGridCell(2,3,0);
            AssertGridCell(3,3,0);
            AssertGridCell(4,3,0);
            AssertGridCell(0,4,0);
            AssertGridCell(1,4,0);
            AssertGridCell(2,4,0);
            AssertGridCell(3,4,0);
            AssertGridCell(4,4,0);


        }

        private void AssertGridCell(int x, int y, int expected)
        {
            Assert.That(_sweeper.Grid[x, y].ProximityCount, Is.EqualTo(expected),"Expected {0},{1} to be {2}", x,y,expected);
        }
    }
}
