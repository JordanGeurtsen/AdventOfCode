namespace AdventOfCode._2025._7.Models {
    public class TachyonManifold
    {
        public char[][] Grid { get; set; }

        public TachyonManifold(char[][] grid)
        {
            Grid = grid;
        }

        public char GetCell(int x, int y)
        {
            if (y < 0 || y >= Grid.Length || x < 0 || x >= Grid[y].Length)
                return ' ';
            return Grid[y][x];
        }

        public bool CellEquals(char value, int x, int y)
        {
            return GetCell(x, y) == value;
        }
    }
}