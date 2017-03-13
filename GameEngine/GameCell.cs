namespace GameEngine
{
    public class GameCell
    {
        public CoOrdinate CoOrd { get; set; }

        public GameCell(int xCord, int yCord)
        {
            CoOrd = new CoOrdinate(xCord,yCord);
        }

        public bool IsBomb { get; set; }
        public bool hasBeenDug { get; set; }
        public int ProximityCount { get; set; }
        public bool IsExplosion { get; set; }
    }
}