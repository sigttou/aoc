using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly List<Star> _stars;

    public Day10()
    {
        _stars = File.ReadAllLines(InputFilePath)
            .Select(line => new Star(line))
            .ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var (message, _) = FindMessage();
        string outputPath = Path.Combine("Outputs", "day10_message.txt");
        Directory.CreateDirectory("Outputs"); // Ensure the directory exists
        File.WriteAllText(outputPath, message);
        return new(outputPath);
    }

    public override ValueTask<string> Solve_2()
    {
        var (_, seconds) = FindMessage();
        return new(seconds.ToString());
    }

    private (string message, int seconds) FindMessage()
    {
        var stars = _stars.Select(s => s.Clone()).ToList();

        int seconds = 0;
        long previousArea = long.MaxValue;
        long currentArea = CalculateArea(stars);

        // Keep moving stars until they start spreading out again
        while (currentArea < previousArea)
        {
            previousArea = currentArea;
            seconds++;

            // Move all stars
            foreach (var star in stars)
            {
                star.Move();
            }

            currentArea = CalculateArea(stars);
        }

        // Go back one second to the configuration with minimum area
        seconds--;
        foreach (var star in stars)
        {
            star.MoveBack();
        }

        // Generate the message visualization
        string message = VisualizeMessage(stars);
        return (message, seconds);
    }

    private long CalculateArea(List<Star> stars)
    {
        int minX = stars.Min(s => s.X);
        int maxX = stars.Max(s => s.X);
        int minY = stars.Min(s => s.Y);
        int maxY = stars.Max(s => s.Y);

        return (long)(maxX - minX) * (maxY - minY);
    }

    private string VisualizeMessage(List<Star> stars)
    {
        int minX = stars.Min(s => s.X);
        int maxX = stars.Max(s => s.X);
        int minY = stars.Min(s => s.Y);
        int maxY = stars.Max(s => s.Y);

        // Create a grid representation of the stars
        char[,] grid = new char[maxY - minY + 1, maxX - minX + 1];
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                grid[y, x] = '.';
            }
        }

        // Mark the positions of the stars
        foreach (var star in stars)
        {
            grid[star.Y - minY, star.X - minX] = '#';
        }

        // Convert grid to string
        var sb = new StringBuilder();
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                sb.Append(grid[y, x]);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private class Star
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        private readonly int _velocityX;
        private readonly int _velocityY;

        public Star(string input)
        {
            // Parse input format: "position=< 9,  1> velocity=< 0,  2>"
            var parts = input.Split("velocity=");

            string positionPart = parts[0].Substring(parts[0].IndexOf('<') + 1, parts[0].LastIndexOf('>') - parts[0].IndexOf('<') - 1);
            string velocityPart = parts[1].Substring(parts[1].IndexOf('<') + 1, parts[1].LastIndexOf('>') - parts[1].IndexOf('<') - 1);

            var position = positionPart.Split(',').Select(p => int.Parse(p.Trim())).ToArray();
            var velocity = velocityPart.Split(',').Select(p => int.Parse(p.Trim())).ToArray();

            X = position[0];
            Y = position[1];
            _velocityX = velocity[0];
            _velocityY = velocity[1];
        }

        private Star(int x, int y, int velocityX, int velocityY)
        {
            X = x;
            Y = y;
            _velocityX = velocityX;
            _velocityY = velocityY;
        }

        public void Move()
        {
            X += _velocityX;
            Y += _velocityY;
        }

        public void MoveBack()
        {
            X -= _velocityX;
            Y -= _velocityY;
        }

        public Star Clone()
        {
            return new Star(X, Y, _velocityX, _velocityY);
        }
    }
}