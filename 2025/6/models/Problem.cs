namespace AdventOfCode._2025._6.Models {
    public class Problem
    {
        public int[] Numbers { get; set; }
        public string symbol { get; set; }
        
        public long Solve()
        {
            switch (symbol)
            {
                case MathSymbol.ADD:
                    return Numbers.Sum();
                case MathSymbol.MULTIPLY:
                    var product = 1;
                    foreach (var num in Numbers)
                    {
                        product *= num;
                    }
                    return product;
                default:
                    throw new InvalidOperationException($"Unknown symbol: {symbol}");
            }
        }
    }
}