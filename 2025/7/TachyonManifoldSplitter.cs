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
        private long totalTimelines = 0;
        private Dictionary<(int x, int y, string direction), long> cache = new Dictionary<(int x, int y, string direction), long>();

        public static void Main(string[] args)
        {
            var splitter = new TachyonManifoldSplitter();
            splitter.InitializeGrid(File.ReadAllLines("input"));
            splitter.CalculateSplitAmount();

            Console.WriteLine($"Total Quantum Timelines: {splitter.totalTimelines}");
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

        private long TraceBeamRecursive(int x, int y, string direction = "down")
        {
            var key = (x, y, direction);
            
            // Check cache to avoid recalculating same paths
            if (cache.ContainsKey(key))
                return cache[key];

            long timelineCount = 0;

            // Move beam in the specified direction until it hits a splitter or boundary
            while (true)
            {
                // Move to next position based on direction
                if (direction == "down")
                    y++;
                else if (direction == "left")
                    x--;
                else if (direction == "right")
                    x++;
                
                // Check bounds after movement
                if (y < 0 || y >= grid.Grid.Length || x < 0 || x >= grid.Grid[0].Length)
                {
                    // Beam exits the grid, timeline ends
                    timelineCount = 1;
                    break;
                }

                // Check if we hit a splitter '^'
                if (grid.CellEquals('^', x, y))
                {
                    Console.WriteLine($"Beam split at splitter ({x}, {y})");
                                       
                    // Each path creates its own set of timelines
                    long leftTimelines = TraceBeamRecursive(x, y, "left");
                    long rightTimelines = TraceBeamRecursive(x, y, "right");
                    
                    timelineCount = leftTimelines + rightTimelines;
                    break;
                }

                // For left/right movement, after one step, continue downward
                if (direction == "left" || direction == "right")
                {
                    direction = "down";
                }
            }

            // Cache the result for this position and direction so my computer doesn't explode
            cache[key] = timelineCount;
            return timelineCount;
        }

        private void CalculateSplitAmount()
        {
            totalTimelines = 0;
            cache.Clear();
            int y = 0;

            for (int x = 0; x < grid.Grid[y].Length; x++)
            {
                if (grid.CellEquals('S', x, y))
                {
                    Console.WriteLine($"Found source at ({x}, {y}), starting recursive beam tracing...");
                    totalTimelines += TraceBeamRecursive(x, y, "down");
                }
            }
        }
    }
}