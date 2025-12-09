using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode._2025._9.Models;

namespace AdventOfCode._2025._9
{
    public class RedBoxCalculator
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Calculating largest red tile area...");
            var calculator = new RedBoxCalculator();
            var coords = calculator.ParseCoordinates();
            var tiles = calculator.CreateTiles(coords);
            Console.WriteLine($"Created {tiles.Count} tiles from input coordinates.");

            var grid = new RedTileGrid(tiles); 
            Console.WriteLine("Calculation complete.");
            Console.WriteLine($"Largest Red Tile Area: {grid.GetLargestArea()}");
        }

        private List<(int x, int y)> ParseCoordinates()
        {
            var lines = File.ReadAllLines("input");
            var coords = new List<(int x, int y)>();
            
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                {
                    coords.Add((x, y));
                }
            }
            
            return coords;
        }

        private List<Tile> CreateTiles(List<(int x, int y)> coords)
        {
            var tiles = new List<Tile>();
            
            for (int i = 0; i < coords.Count; i++)
            {
                for (int j = 0; j < coords.Count; j++)
                {
                    tiles.Add(new Tile(
                        coords[i].x, 
                        coords[i].y,
                        coords[j].x,
                        coords[j].y
                    ));
                }
            }
            
            return tiles;
        }
    }
}