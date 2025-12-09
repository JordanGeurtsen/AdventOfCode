namespace AdventOfCode._2025._9.Models {
    public class Tile
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        
        public Tile(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public long Area()
        {
            return Math.Abs((X2 - X1 + 1) * (Y2 - Y1 + 1));
        }
    }
}