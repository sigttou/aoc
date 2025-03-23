using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day23 : BaseDay
{
    private readonly string _input;
    private readonly List<Nanobot> _nanobots;

    public Day23()
    {
        _input = File.ReadAllText(InputFilePath);
        _nanobots = ParseInput(_input);
    }

    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    record Nanobot(int X, int Y, int Z, int Radius);

    private List<Nanobot> ParseInput(string input) =>
        input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
             .Select(line =>
             {
                 var m = Regex.Match(line, @"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)");
                 return new Nanobot(int.Parse(m.Groups[1].Value),
                                    int.Parse(m.Groups[2].Value),
                                    int.Parse(m.Groups[3].Value),
                                    int.Parse(m.Groups[4].Value));
             })
             .ToList();

    private int Manhattan(Nanobot a, Nanobot b) =>
        Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

    private int Manhattan(int x, int y, int z) =>
        Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

    private int Part1()
    {
        var strongest = _nanobots.OrderByDescending(bot => bot.Radius).First();

        return _nanobots.Count(bot => Manhattan(strongest, bot) <= strongest.Radius);
    }


    private int Part2()
    {
        var events = new List<(int Distance, int Type)>();

        foreach (var bot in _nanobots)
        {
            int distanceToOrigin = Math.Abs(bot.X) + Math.Abs(bot.Y) + Math.Abs(bot.Z);
            events.Add((distanceToOrigin - bot.Radius, +1));
            events.Add((distanceToOrigin + bot.Radius, -1));
        }

        events = events.OrderBy(e => e.Distance).ThenBy(e => -e.Type).ToList();

        int count = 0, maxCount = 0, bestDistance = 0;

        foreach (var (distance, type) in events)
        {
            count += type;
            if (count > maxCount)
            {
                maxCount = count;
                bestDistance = distance;
            }
        }

        return bestDistance + 1;
    }


}
