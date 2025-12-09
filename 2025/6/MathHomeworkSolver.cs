using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode._2025._6.Models;
    
namespace AdventOfCode._2025._6
{
    public class MathHomeworkSolver
    {
        public static MathHomeworkSolver solver {get; set;}
        public static ProblemList problemList {get; set;}


        public static void Main(string[] args)
        {
            solver = new MathHomeworkSolver();
            problemList = new ProblemList(new List<Problem>());

            solver.InitializeGrid(File.ReadAllLines("input"));

            Console.WriteLine($"Total sum of problem answers: {problemList.SolveAll()}");
        }

        private void InitializeGrid(string[] lines)
        {
            var problems = new List<Problem>();
            
            if (lines.Length == 0) return;
            
            // The last line contains the operation symbols
            string symbolLine = lines[lines.Length - 1];
            int maxWidth = lines.Max(line => line.Length);
            List<int> problemColumns = new List<int>();
            
            // Find problem column boundaries by looking for non-space characters in symbol line
            for (int col = 0; col < symbolLine.Length; col++)
            {
                if (symbolLine[col] == '+' || symbolLine[col] == '*')
                {
                    problemColumns.Add(col);
                }
            }
                       
            // For each problem column, extract the numbers above the symbol
            foreach (int symbolCol in problemColumns)
            {
                var numbers = new List<int>();
                char symbol = symbolLine[symbolCol];
                
                // Look upward from the symbol to find the column of numbers
                // Find the start and end of this column by looking for spaces
                int columnStart = symbolCol;
                int columnEnd = symbolCol;
                
                // Expand the column boundaries to capture the full numbers
                for (int row = 0; row < lines.Length - 1; row++)
                {
                    if (row < lines.Length && symbolCol < lines[row].Length)
                    {
                        // Find the start of the number in this row
                        int numStart = symbolCol;
                        while (numStart > 0 && lines[row][numStart - 1] != ' ')
                            numStart--;
                        
                        // Find the end of the number in this row  
                        int numEnd = symbolCol;
                        while (numEnd < lines[row].Length - 1 && lines[row][numEnd + 1] != ' ')
                            numEnd++;
                            
                        columnStart = Math.Min(columnStart, numStart);
                        columnEnd = Math.Max(columnEnd, numEnd);
                    }
                }
                
                // Extract numbers from this column
                for (int row = 0; row < lines.Length - 1; row++)
                {
                    if (row < lines.Length)
                    {
                        string rowText = lines[row];
                        if (columnStart < rowText.Length)
                        {
                            // Extract the number from this column range
                            string columnText = "";
                            for (int col = columnStart; col <= columnEnd && col < rowText.Length; col++)
                            {
                                columnText += rowText[col];
                            }
                            
                            // Parse the number from this column
                            columnText = columnText.Trim();
                            if (!string.IsNullOrEmpty(columnText) && int.TryParse(columnText, out int number))
                            {
                                numbers.Add(number);
                            }
                        }
                    }
                }
                
                if (numbers.Count > 0)
                {
                    problems.Add(new Problem{
                        Numbers = numbers.ToArray(),
                        symbol = symbol == '+' ? MathSymbol.ADD : MathSymbol.MULTIPLY
                    });
                    
                    // Console.WriteLine($"Problem {problems.Count}: {string.Join(" " + symbol + " ", numbers)} = {(symbol == '+' ? numbers.Sum() : numbers.Aggregate(1, (acc, val) => acc * val))}");
                }
            }
            
            problemList = new ProblemList(problems);
            Console.WriteLine($"Initialized {problems.Count} problems from input.");
        }
    }
}