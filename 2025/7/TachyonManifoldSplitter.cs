using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode._2025._7.Models;

namespace AdventOfCode._2025._7
{
    public class TachyonManifoldSplitter
    {
        public TachyonManifold grid {get; set;}
        public (int x, int y)[] beamSplitCoords { get; set; }

        public static void Main(string[] args)
        {
            var splitter = new TachyonManifoldSplitter();
            splitter.InitializeGrid(File.ReadAllLines("input"));
            splitter.CalculateSplitAmount();

            Console.WriteLine($"Total Splits: {splitter.splitCount}");
            Console.WriteLine($"Unique Splitters Hit: {splitter.beamSplitCoords.Length}");
        }

        private void InitializeGrid(string[] lines)
        {
            int height = lines.Length;
            int width = lines.Max(line => line.Length);
            char[][] gridArray = new char[height][];

            for (int y = 0; y < height; y++)
            {
                gridArray[y] = lines[y].PadRight(width).ToCharArray();
            }

            grid = new TachyonManifold(gridArray);
        }

        private int CountAllSplitters(TachyonManifold grid)
        {
            int count = 0;
            for (int y = 0; y < grid.Grid.Length; y++)
            {
                for (int x = 0; x < grid.Grid[y].Length; x++)
                {
                    if (grid.CellEquals('^', x, y))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private int splitCount = 0;
        private HashSet<(int x, int y)> visitedSplitters = new HashSet<(int x, int y)>();

        private void TraceBeamRecursive(int x, int y, string direction = "down")
        {
            // Move beam in the specified direction
            while (y >= 0 && y < grid.Grid.Length && x >= 0 && x < grid.Grid[0].Length)
            {
                // Move to next position based on direction
                if (direction == "down")
                {
                    y++;
                }
                else if (direction == "left")
                {
                    x--;
                    direction = "down"; // After left, continue down
                }
                else if (direction == "right")
                {
                    x++;
                    direction = "down"; // After right, continue down
                }
                
                // Check bounds again after movement
                if (y < 0 || y >= grid.Grid.Length || x < 0 || x >= grid.Grid[0].Length)
                    break;

                // Check if we hit a splitter '^'
                if (grid.CellEquals('^', x, y))
                {
                    // Avoid infinite loops by checking if we've already processed this splitter
                    if (visitedSplitters.Contains((x, y)))
                        break;
                    
                    visitedSplitters.Add((x, y));
                    splitCount++;
                    
                    Console.WriteLine($"Beam split at splitter ({x}, {y}) - Split #{splitCount}");
                    
                    // Recursively trace left and right paths from this splitter
                    TraceBeamRecursive(x, y, "left");
                    TraceBeamRecursive(x, y, "right");
                    
                    // Stop this beam path since it has split
                    break;
                }
            }
        }

        private void CalculateSplitAmount()
        {
            splitCount = 0;
            visitedSplitters.Clear();

            // Find all sources 'S' and trace beams recursively
            for (int y = 0; y < grid.Grid.Length; y++)
            {
                for (int x = 0; x < grid.Grid[y].Length; x++)
                {
                    if (grid.CellEquals('S', x, y))
                    {
                        Console.WriteLine($"Found source at ({x}, {y}), starting recursive beam tracing...");
                        TraceBeamRecursive(x, y, "down");
                    }
                }
            }

            beamSplitCoords = visitedSplitters.ToArray();
            Console.WriteLine($"Total splits counted: {splitCount}");
        }
    }
}