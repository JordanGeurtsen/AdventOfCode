namespace AdventOfCode._2025._6.Models {
    public class Problem
    {
        public int[] Numbers { get; set; }
        public MathSymbol symbol { get; set; }
        
        public int Solve()
        {
            switch (symbol)
            {
                case var s when s == MathSymbol.ADD:
                    return Numbers.Sum();
                case var s when s == MathSymbol.MULTIPLY:
                    return Numbers.Aggregate(1, (acc, val) => acc * val);
                default:
                    return 0;
            }
        }
    }
}