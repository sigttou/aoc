using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day06 : BaseDay
    {
        private readonly string _input;

        public Day06()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var coordinates = _input
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(',').Select(int.Parse).ToArray())
                .Select(parts => (x: parts[0], y: parts[1]))
                .ToList();

            int minX = coordinates.Min(c => c.x);
            int maxX = coordinates.Max(c => c.x);
            int minY = coordinates.Min(c => c.y);
            int maxY = coordinates.Max(c => c.y);

            var areaSizes = new Dictionary<(int x, int y), int>();
            var infiniteAreas = new HashSet<(int x, int y)>();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var distances = coordinates
                        .Select(coord => (coord, distance: ManhattanDistance((x, y), coord)))
                        .OrderBy(p => p.distance)
                        .ToList();

                    if (distances[0].distance != distances[1].distance)
                    {
                        var closest = distances[0].coord;
                        if (!areaSizes.ContainsKey(closest))
                        {
                            areaSizes[closest] = 0;
                        }
                        areaSizes[closest]++;

                        if (x == minX || x == maxX || y == minY || y == maxY)
                        {
                            infiniteAreas.Add(closest);
                        }
                    }
                }
            }

            var largestFiniteArea = areaSizes
                .Where(kvp => !infiniteAreas.Contains(kvp.Key))
                .Max(kvp => kvp.Value);

            return new ValueTask<string>(largestFiniteArea.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var coordinates = _input
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(',').Select(int.Parse).ToArray())
                .Select(parts => (x: parts[0], y: parts[1]))
                .ToList();

            int minX = coordinates.Min(c => c.x);
            int maxX = coordinates.Max(c => c.x);
            int minY = coordinates.Min(c => c.y);
            int maxY = coordinates.Max(c => c.y);

            int safeRegionSize = 0;
            int distanceThreshold = 10000;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    int totalDistance = coordinates.Sum(coord => ManhattanDistance((x, y), coord));
                    if (totalDistance < distanceThreshold)
                    {
                        safeRegionSize++;
                    }
                }
            }

            return new ValueTask<string>(safeRegionSize.ToString());
        }

        private int ManhattanDistance((int x, int y) point1, (int x, int y) point2)
        {
            return Math.Abs(point1.x - point2.x) + Math.Abs(point1.y - point2.y);
        }
    }
}