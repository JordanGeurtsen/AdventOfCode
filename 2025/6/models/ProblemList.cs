namespace AdventOfCode._2025._6.Models {
    public class ProblemList
    {
        public List<Problem> Problems { get; set; }

        public ProblemList(List<Problem> problems)
        {
            Problems = problems;
        }

        public long SolveAll()
        {
            var results = new List<long>();
            foreach (var problem in Problems)
            {
                results.Add(problem.Solve());
            }
            return results.Sum();
        }
    }
}