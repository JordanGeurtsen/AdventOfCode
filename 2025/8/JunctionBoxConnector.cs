using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode._2025._8.Models;

namespace AdventOfCode._2025._8
{
    public class JunctionBoxConnector
    {
        public static void Main(string[] args)
        {
            var connector = new JunctionBoxConnector();
            long result = connector.ProcessJunctionBoxConnections();
            Console.WriteLine($"Final Result: {result}");
        }

        public long ProcessJunctionBoxConnections()
        {
            var junctionBoxes = new List<JunctionBox>();

            Console.WriteLine("Processing junction box connectors...");
            
            // Read input file
            string[] lines = File.ReadAllLines("input");
            
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] coords = line.Split(',');
                if (coords.Length == 3 && 
                    int.TryParse(coords[0], out int x) &&
                    int.TryParse(coords[1], out int y) &&
                    int.TryParse(coords[2], out int z))
                {
                    junctionBoxes.Add(new JunctionBox(x, y, z, i));
                }
            }

            Console.WriteLine($"Found {junctionBoxes.Count} junction boxes.");

            // Calculate all possible pairs and their distances
            var allPairs = new List<JunctionPair>();
            for (int i = 0; i < junctionBoxes.Count; i++)
            {
                for (int j = i + 1; j < junctionBoxes.Count; j++)
                {
                    double distance = Calculate3DDistance(junctionBoxes[i], junctionBoxes[j]);
                    allPairs.Add(new JunctionPair(junctionBoxes[i], junctionBoxes[j], distance));
                }
            }

            // Sort pairs by distance (closest first)
            allPairs.Sort((p1, p2) => p1.Distance.CompareTo(p2.Distance));
            Console.WriteLine($"Generated {allPairs.Count} possible pairs.");

            // Use Union-Find to connect pairs until all are in one circuit
            var unionFind = new UnionFind(junctionBoxes.Count);
            int connectionsUsed = 0;
            JunctionPair lastConnection = null;

            Console.WriteLine("Connecting closest pairs until all junction boxes are in one circuit...");

            foreach (var pair in allPairs)
            {
                bool connected = unionFind.Union(pair.Box1.Id, pair.Box2.Id);
                connectionsUsed++;
                
                if (connected)
                {
                    Console.WriteLine($"Connection {connectionsUsed}: Connected {pair.Box1} to {pair.Box2} (distance: {pair.Distance:F2})");
                    lastConnection = pair;
                    
                    // Check if all junction boxes are now in one circuit
                    var circuitSizes = unionFind.GetCircuitSizes();
                    if (circuitSizes.Count == 1)
                    {
                        Console.WriteLine($"All junction boxes are now connected in a single circuit!");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"Connection {connectionsUsed}: Skipped {pair.Box1} to {pair.Box2} - already connected");
                }
            }

            if (lastConnection == null)
            {
                Console.Error.WriteLine("Error: No connections were made.");
                return 0;
            }

            // Check final circuit state
            var finalCircuitSizes = unionFind.GetCircuitSizes();
            Console.WriteLine($"\nFinal state: {finalCircuitSizes.Count} circuits");
            
            if (finalCircuitSizes.Count == 1)
            {
                Console.WriteLine($"Success! All {junctionBoxes.Count} junction boxes are in one circuit.");
            }
            else
            {
                Console.WriteLine($"Warning: Still have {finalCircuitSizes.Count} separate circuits.");
                foreach (var size in finalCircuitSizes.OrderByDescending(s => s))
                {
                    Console.WriteLine($"  Circuit size: {size}");
                }
            }

            // Calculate result: multiply X coordinates of the last two connected junction boxes
            int x1 = lastConnection.Box1.X;
            int x2 = lastConnection.Box2.X;
            long result = (long)x1 * x2;
            
            Console.WriteLine($"\nLast connection: {lastConnection.Box1} to {lastConnection.Box2}");
            Console.WriteLine($"X coordinates: {x1} and {x2}");
            Console.WriteLine($"Result: {x1} × {x2} = {result}");

            return result;
        }

        private double Calculate3DDistance(JunctionBox box1, JunctionBox box2)
        {
            double dx = box2.X - box1.X;
            double dy = box2.Y - box1.Y;
            double dz = box2.Z - box1.Z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        } 
    }
}