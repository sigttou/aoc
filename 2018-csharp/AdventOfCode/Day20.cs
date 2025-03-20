using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day20 : BaseDay
{
    private readonly string _input;
    private readonly Dictionary<(int x, int y), int> _distances = new();
    private readonly HashSet<(int x, int y, int x2, int y2)> _doors = new();

    public Day20()
    {
        _input = File.ReadAllText(InputFilePath).Trim();
    }

    private void BuildMap()
    {
        _distances.Clear();
        _doors.Clear();

        var positionStack = new Stack<(int x, int y)>();

        int x = 0, y = 0;
        _distances[(x, y)] = 0;

        for (int i = 1; i < _input.Length - 1; i++)
        {
            char c = _input[i];

            switch (c)
            {
                case 'N':
                    AddDoor(x, y, x, y - 1);
                    y -= 2;
                    UpdateDistance(x, y);
                    break;

                case 'S':
                    AddDoor(x, y, x, y + 1);
                    y += 2;
                    UpdateDistance(x, y);
                    break;

                case 'E':
                    AddDoor(x, y, x + 1, y);
                    x += 2;
                    UpdateDistance(x, y);
                    break;

                case 'W':
                    AddDoor(x, y, x - 1, y);
                    x -= 2;
                    UpdateDistance(x, y);
                    break;

                case '(':
                    positionStack.Push((x, y));
                    break;

                case '|':
                    (x, y) = positionStack.Peek();
                    break;

                case ')':
                    positionStack.Pop();
                    break;
            }
        }
    }

    private void AddDoor(int x1, int y1, int x2, int y2)
    {
        if (x1 > x2 || (x1 == x2 && y1 > y2))
        {
            (x1, y1, x2, y2) = (x2, y2, x1, y1);
        }

        _doors.Add((x1, y1, x2, y2));
    }

    private void UpdateDistance(int x, int y)
    {
        if (!_distances.ContainsKey((x, y)))
        {
            _distances[(x, y)] = int.MaxValue;
        }

        var queue = new Queue<(int x, int y, int distance)>();
        queue.Enqueue((0, 0, 0));

        var visited = new HashSet<(int x, int y)>();
        visited.Add((0, 0));

        while (queue.Count > 0)
        {
            var (currX, currY, distance) = queue.Dequeue();

            if (distance < _distances[(currX, currY)])
            {
                _distances[(currX, currY)] = distance;
            }

            TryMove(currX, currY, currX, currY - 2, distance + 1, queue, visited); // North
            TryMove(currX, currY, currX, currY + 2, distance + 1, queue, visited); // South
            TryMove(currX, currY, currX + 2, currY, distance + 1, queue, visited); // East
            TryMove(currX, currY, currX - 2, currY, distance + 1, queue, visited); // West
        }
    }

    private void TryMove(int x1, int y1, int x2, int y2, int newDistance,
                         Queue<(int x, int y, int distance)> queue,
                         HashSet<(int x, int y)> visited)
    {
        if (IsDoorBetween(x1, y1, x2, y2))
        {
            if (!visited.Contains((x2, y2)))
            {
                visited.Add((x2, y2));
                queue.Enqueue((x2, y2, newDistance));
            }
        }
    }

    private bool IsDoorBetween(int x1, int y1, int x2, int y2)
    {
        int doorX = (x1 + x2) / 2;
        int doorY = (y1 + y2) / 2;

        if (x1 > x2 || (x1 == x2 && y1 > y2))
        {
            (x1, y1, x2, y2) = (x2, y2, x1, y1);
        }

        return _doors.Contains((x1, y1, doorX, doorY)) || _doors.Contains((doorX, doorY, x2, y2));
    }

    public override ValueTask<string> Solve_1()
    {
        BuildMap();
        int maxDistance = _distances.Values.Max();
        return new ValueTask<string>(maxDistance.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        BuildMap();
        int roomsWithManyDoors = _distances.Values.Count(d => d >= 1000);
        return new ValueTask<string>(roomsWithManyDoors.ToString());
    }
}