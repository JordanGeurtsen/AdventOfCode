namespace AdventOfCode._2025._8.Models {
    public class UnionFind
    {
        private int[] parent;
        private int[] size;

        public UnionFind(int n)
        {
            parent = new int[n];
            size = new int[n];
            for (int i = 0; i < n; i++)
            {
                parent[i] = i;
                size[i] = 1;
            }
        }

        public int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]); // Path compression
            }
            return parent[x];
        }

        public bool Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if (rootX == rootY)
            {
                return false; // Already connected
            }

            // Union by size
            if (size[rootX] < size[rootY])
            {
                (rootX, rootY) = (rootY, rootX);
            }

            parent[rootY] = rootX;
            size[rootX] += size[rootY];
            return true;
        }

        public List<int> GetCircuitSizes()
        {
            var circuits = new Dictionary<int, int>();
            for (int i = 0; i < parent.Length; i++)
            {
                int root = Find(i);
                circuits[root] = size[root];
            }
            return circuits.Values.ToList();
        }
    }
}