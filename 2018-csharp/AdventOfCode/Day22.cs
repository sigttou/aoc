using AoCHelper;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace AdventOfCode;

public class Day22 : BaseDay
{
    private readonly int _depth;
    private readonly (int x, int y) _target;
    private readonly Dictionary<(int x, int y), long> _erosionLevelCache = new();

    public Day22()
    {
        var input = File.ReadAllLines(InputFilePath);
        _depth = int.Parse(input[0].Split(": ")[1]);
        var targetCoords = input[1].Split(": ")[1].Split(',');
        _target = (int.Parse(targetCoords[0]), int.Parse(targetCoords[1]));
    }

    public override ValueTask<string> Solve_1()
    {
        int riskLevel = 0;

        for (int y = 0; y <= _target.y; y++)
        {
            for (int x = 0; x <= _target.x; x++)
            {
                int regionType = CalculateRegionType(x, y);
                riskLevel += regionType;
            }
        }

        return new(riskLevel.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var shortestTime = FindShortestPath();
        return new(shortestTime.ToString());
    }

    private long CalculateGeologicIndex(int x, int y)
    {
        if (x == 0 && y == 0)
            return 0;

        if (x == _target.x && y == _target.y)
            return 0;

        if (y == 0)
            return x * 16807L;

        if (x == 0)
            return y * 48271L;

        long erosionLevel1 = CalculateErosionLevel(x - 1, y);
        long erosionLevel2 = CalculateErosionLevel(x, y - 1);

        return erosionLevel1 * erosionLevel2;
    }

    private long CalculateErosionLevel(int x, int y)
    {
        if (_erosionLevelCache.TryGetValue((x, y), out long cachedLevel))
            return cachedLevel;

        long geologicIndex = CalculateGeologicIndex(x, y);
        long erosionLevel = (geologicIndex + _depth) % 20183;

        _erosionLevelCache[(x, y)] = erosionLevel;
        return erosionLevel;
    }

    private int CalculateRegionType(int x, int y)
    {
        long erosionLevel = CalculateErosionLevel(x, y);
        return (int)(erosionLevel % 3);
    }

    private int FindShortestPath()
    {
        const int TORCH = 0;

        bool[,] allowed = new bool[3, 3]
        {
            { true, true, false },  // Rocky: Torch, Climbing Gear
            { false, true, true },  // Wet: Climbing Gear, Neither
            { true, false, true }   // Narrow: Torch, Neither
        };

        var queue = new PriorityQueue<(int x, int y, int tool, int time), int>();

        queue.Enqueue((0, 0, TORCH, 0), 0);

        var visited = new Dictionary<(int x, int y, int tool), int>();

        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };

        while (queue.Count > 0)
        {
            var (x, y, tool, time) = queue.Dequeue();

            if (x == _target.x && y == _target.y && tool == TORCH)
                return time;

            var state = (x, y, tool);
            if (visited.TryGetValue(state, out int visitedTime) && visitedTime <= time)
                continue;

            visited[state] = time;

            int currentRegion = CalculateRegionType(x, y);
            for (int newTool = 0; newTool < 3; newTool++)
            {
                if (newTool != tool && allowed[currentRegion, newTool])
                {
                    queue.Enqueue((x, y, newTool, time + 7), time + 7);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx < 0 || ny < 0)
                    continue;

                int nextRegion = CalculateRegionType(nx, ny);

                if (allowed[nextRegion, tool])
                {
                    queue.Enqueue((nx, ny, tool, time + 1), time + 1);
                }
            }
        }

        return -1;
    }
}