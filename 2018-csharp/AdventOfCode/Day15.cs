using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day15 : BaseDay
{
    private readonly string[] _map;
    private const int DefaultHitPoints = 200;
    private const int DefaultAttackPower = 3;

    public Day15()
    {
        _map = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = SimulateCombat(DefaultAttackPower, DefaultAttackPower);
        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int elfAttackPower = 4;
        int outcome;

        while (true)
        {
            var result = SimulateCombat(elfAttackPower, DefaultAttackPower, true);
            if (result > 0)
            {
                outcome = result;
                break;
            }
            elfAttackPower++;
        }

        return new ValueTask<string>(outcome.ToString());
    }

    private int SimulateCombat(int elfAttackPower, int goblinAttackPower, bool requireAllElvesSurvive = false)
    {
        char[,] map = new char[_map.Length, _map[0].Length];
        List<Unit> units = new List<Unit>();

        for (int y = 0; y < _map.Length; y++)
        {
            for (int x = 0; x < _map[y].Length; x++)
            {
                char c = _map[y][x];
                map[y, x] = c;

                if (c == 'E')
                {
                    units.Add(new Unit { Type = UnitType.Elf, X = x, Y = y, HitPoints = DefaultHitPoints, AttackPower = elfAttackPower });
                    map[y, x] = '.';
                }
                else if (c == 'G')
                {
                    units.Add(new Unit { Type = UnitType.Goblin, X = x, Y = y, HitPoints = DefaultHitPoints, AttackPower = goblinAttackPower });
                    map[y, x] = '.';
                }
            }
        }

        int initialElfCount = units.Count(u => u.Type == UnitType.Elf);
        int rounds = 0;

        while (true)
        {
            units = units.OrderBy(u => u.Y).ThenBy(u => u.X).ToList();

            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                if (unit.HitPoints <= 0) continue;

                var targets = units.Where(u => u.Type != unit.Type && u.HitPoints > 0).ToList();
                if (targets.Count == 0)
                {
                    int remainingHitPoints = units.Where(u => u.HitPoints > 0).Sum(u => u.HitPoints);
                    return rounds * remainingHitPoints;
                }

                bool inRange = targets.Any(t => IsAdjacent(unit, t));

                if (!inRange)
                {
                    MoveTowardsTarget(unit, targets, map, units);
                    inRange = targets.Any(t => IsAdjacent(unit, t));
                }

                if (inRange)
                {
                    var adjacentTargets = targets.Where(t => IsAdjacent(unit, t))
                                               .OrderBy(t => t.HitPoints)
                                               .ThenBy(t => t.Y)
                                               .ThenBy(t => t.X)
                                               .ToList();

                    if (adjacentTargets.Any())
                    {
                        var target = adjacentTargets.First();
                        target.HitPoints -= unit.AttackPower;

                        if (target.HitPoints <= 0)
                        {
                            if (target.Type == UnitType.Elf && requireAllElvesSurvive)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }

            units = units.Where(u => u.HitPoints > 0).ToList();

            if (requireAllElvesSurvive && units.Count(u => u.Type == UnitType.Elf) < initialElfCount)
            {
                return 0;
            }

            rounds++;
        }
    }

    private void MoveTowardsTarget(Unit unit, List<Unit> targets, char[,] map, List<Unit> allUnits)
    {
        var targetSquares = new HashSet<(int X, int Y)>();
        foreach (var target in targets)
        {
            if (IsOpen(target.X, target.Y - 1, map, allUnits)) targetSquares.Add((target.X, target.Y - 1));
            if (IsOpen(target.X - 1, target.Y, map, allUnits)) targetSquares.Add((target.X - 1, target.Y));
            if (IsOpen(target.X + 1, target.Y, map, allUnits)) targetSquares.Add((target.X + 1, target.Y));
            if (IsOpen(target.X, target.Y + 1, map, allUnits)) targetSquares.Add((target.X, target.Y + 1));
        }

        if (targetSquares.Count == 0) return;

        var startPoints = new List<(int X, int Y)>();

        if (IsOpen(unit.X, unit.Y - 1, map, allUnits)) startPoints.Add((unit.X, unit.Y - 1));
        if (IsOpen(unit.X - 1, unit.Y, map, allUnits)) startPoints.Add((unit.X - 1, unit.Y));
        if (IsOpen(unit.X + 1, unit.Y, map, allUnits)) startPoints.Add((unit.X + 1, unit.Y));
        if (IsOpen(unit.X, unit.Y + 1, map, allUnits)) startPoints.Add((unit.X, unit.Y + 1));

        if (startPoints.Count == 0) return;

        startPoints = startPoints.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

        (int X, int Y)? bestTarget = null;
        (int X, int Y)? bestStart = null;
        int shortestDistance = int.MaxValue;

        foreach (var start in startPoints)
        {
            var queue = new Queue<(int X, int Y)>();
            var visited = new HashSet<(int X, int Y)>();
            var dist = new Dictionary<(int X, int Y), int>();

            queue.Enqueue(start);
            visited.Add(start);
            dist[start] = 1; // One step from the unit

            while (queue.Count > 0 && (bestTarget == null || dist[queue.Peek()] < shortestDistance))
            {
                var current = queue.Dequeue();

                if (targetSquares.Contains(current) && (bestTarget == null ||
                    dist[current] < shortestDistance ||
                    (dist[current] == shortestDistance &&
                     (current.Y < bestTarget.Value.Y || (current.Y == bestTarget.Value.Y && current.X < bestTarget.Value.X)))))
                {
                    shortestDistance = dist[current];
                    bestTarget = current;
                    bestStart = start;
                }

                var neighbors = new[]
                {
                    (X: current.X, Y: current.Y - 1),
                    (X: current.X - 1, Y: current.Y),
                    (X: current.X + 1, Y: current.Y),
                    (X: current.X, Y: current.Y + 1)
                };

                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor) && IsOpen(neighbor.X, neighbor.Y, map, allUnits))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        dist[neighbor] = dist[current] + 1;
                    }
                }
            }
        }

        if (bestTarget.HasValue && bestStart.HasValue)
        {
            unit.X = bestStart.Value.X;
            unit.Y = bestStart.Value.Y;
        }
    }

    private bool IsOpen(int x, int y, char[,] map, List<Unit> units)
    {
        if (x < 0 || y < 0 || y >= map.GetLength(0) || x >= map.GetLength(1))
            return false;

        if (map[y, x] != '.')
            return false;

        return !units.Any(u => u.HitPoints > 0 && u.X == x && u.Y == y);
    }

    private bool IsAdjacent(Unit unit1, Unit unit2)
    {
        return Math.Abs(unit1.X - unit2.X) + Math.Abs(unit1.Y - unit2.Y) == 1;
    }

    private enum UnitType
    {
        Elf,
        Goblin
    }

    private class Unit
    {
        public UnitType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int HitPoints { get; set; }
        public int AttackPower { get; set; }
    }
}