namespace AdventOfCode._2025._9.Models {
    public class RedTileGrid
    {
        public List<Tile> TilesList { get; set; }

        public RedTileGrid(List<Tile> tilesList)
        {            
            TilesList = tilesList;
        }

        public long GetLargestArea()
        {
            long largestArea = 0;

            foreach (var tile in TilesList)
            {
                long area = tile.Area();
                if (area > largestArea)
                {
                    largestArea = area;
                }
            }
            
            return largestArea;
        }
    }
}