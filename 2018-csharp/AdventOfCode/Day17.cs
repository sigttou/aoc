using AoCHelper;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day17 : BaseDay
{
    private readonly HashSet<Complex> clay = new();
    private readonly HashSet<Complex> settled = new();
    private readonly HashSet<Complex> flowing = new();
    private readonly double ymin;
    private readonly double ymax;

    public Day17()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        foreach (string line in lines)
        {
            var matches = Regex.Matches(line, @"(\d+)");
            int a = int.Parse(matches[0].Value);
            int b = int.Parse(matches[1].Value);
            int c = int.Parse(matches[2].Value);

            if (line.StartsWith("x="))
            {
                for (int y = b; y <= c; y++)
                {
                    clay.Add(new Complex(a, y));
                }
            }
            else
            {
                for (int x = b; x <= c; x++)
                {
                    clay.Add(new Complex(x, a));
                }
            }
        }

        ymin = clay.Min(p => p.Imaginary);
        ymax = clay.Max(p => p.Imaginary);
    }

    private bool Fill(Complex position, Complex direction)
    {
        flowing.Add(position);
        Complex below = position + new Complex(0, 1);
        Complex left = position + new Complex(-1, 0);
        Complex right = position + new Complex(1, 0);

        if (!clay.Contains(below))
        {
            if (!flowing.Contains(below) && 1 <= below.Imaginary && below.Imaginary <= ymax)
            {
                Fill(below, new Complex(0, 1));
            }

            if (!settled.Contains(below))
            {
                return false;
            }
        }

        bool leftFilled = clay.Contains(left) || (!flowing.Contains(left) && Fill(left, new Complex(-1, 0)));
        bool rightFilled = clay.Contains(right) || (!flowing.Contains(right) && Fill(right, new Complex(1, 0)));

        if (direction.Imaginary == 1 && leftFilled && rightFilled)
        {
            settled.Add(position);

            var currentLeft = left;
            while (flowing.Contains(currentLeft))
            {
                settled.Add(currentLeft);
                currentLeft -= new Complex(1, 0);
            }

            var currentRight = right;
            while (flowing.Contains(currentRight))
            {
                settled.Add(currentRight);
                currentRight += new Complex(1, 0);
            }
        }

        return (direction.Real == -1 && (leftFilled || clay.Contains(left))) ||
               (direction.Real == 1 && (rightFilled || clay.Contains(right)));
    }

    public override ValueTask<string> Solve_1()
    {
        settled.Clear();
        flowing.Clear();

        Fill(new Complex(500, 0), new Complex(0, 1));

        var waterTiles = new HashSet<Complex>(flowing);
        foreach (var pos in settled)
        {
            waterTiles.Add(pos);
        }

        int result = waterTiles.Count(pt => ymin <= pt.Imaginary && pt.Imaginary <= ymax);

        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        if (flowing.Count == 0 && settled.Count == 0)
        {
            Fill(new Complex(500, 0), new Complex(0, 1));
        }

        int result = settled.Count(pt => ymin <= pt.Imaginary && pt.Imaginary <= ymax);

        return new ValueTask<string>(result.ToString());
    }
}
