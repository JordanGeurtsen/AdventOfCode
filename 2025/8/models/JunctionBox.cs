namespace AdventOfCode._2025._8.Models {
    public class JunctionBox
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Id { get; set; }

        public JunctionBox(int x, int y, int z, int id)
        {
            X = x;
            Y = y;
            Z = z;
            Id = id;
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }

        public override bool Equals(object obj)
        {
            if (obj is JunctionBox other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}