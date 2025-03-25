using AoCHelper;

namespace AdventOfCode;
public class Day25 : BaseDay
{
    private readonly List<(int x, int y, int z, int w)> _points;

    public Day25()
    {
        _points = File.ReadAllLines(InputFilePath)
            .Select(line =>
            {
                var parts = line.Split(',').Select(int.Parse).ToArray();
                return (parts[0], parts[1], parts[2], parts[3]);
            })
            .ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int constellationCount = CountConstellations();
        return new(constellationCount.ToString());
    }

    public override ValueTask<string> Solve_2() => new("There is no part 2 for Day 25!");

    private int CountConstellations()
    {
        var unvisited = new HashSet<(int x, int y, int z, int w)>(_points);
        int count = 0;

        while (unvisited.Count > 0)
        {
            var toVisit = new Queue<(int x, int y, int z, int w)>();
            var current = unvisited.First();
            unvisited.Remove(current);
            toVisit.Enqueue(current);

            while (toVisit.Count > 0)
            {
                var point = toVisit.Dequeue();
                var connected = unvisited.Where(p => ManhattanDistance(p, point) <= 3).ToList();

                foreach (var p in connected)
                {
                    unvisited.Remove(p);
                    toVisit.Enqueue(p);
                }
            }

            count++;
        }

        return count;
    }

    private int ManhattanDistance((int x, int y, int z, int w) a, (int x, int y, int z, int w) b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) + Math.Abs(a.w - b.w);
    }
}
