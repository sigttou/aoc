using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly int _gridSerialNumber;
    private readonly int[,] _grid;
    private const int GridSize = 300;

    public Day11()
    {
        // Parse the grid serial number from input file
        _gridSerialNumber = int.Parse(File.ReadAllText(InputFilePath).Trim());

        // Initialize the power grid
        _grid = new int[GridSize + 1, GridSize + 1]; // +1 because grid is 1-indexed

        // Calculate power level for each cell
        for (int x = 1; x <= GridSize; x++)
        {
            for (int y = 1; y <= GridSize; y++)
            {
                _grid[x, y] = CalculatePowerLevel(x, y, _gridSerialNumber);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var (x, y, _) = FindLargestPowerSquare(3);
        return new($"{x},{y}");
    }

    public override ValueTask<string> Solve_2()
    {
        var (maxX, maxY, maxSize) = FindLargestPowerSquareAnySize();
        return new($"{maxX},{maxY},{maxSize}");
    }

    private int CalculatePowerLevel(int x, int y, int serialNumber)
    {
        // Find the fuel cell's rack ID, which is its X coordinate plus 10
        int rackId = x + 10;

        // Begin with a power level of the rack ID times the Y coordinate
        int powerLevel = rackId * y;

        // Increase the power level by the value of the grid serial number
        powerLevel += serialNumber;

        // Set the power level to itself multiplied by the rack ID
        powerLevel *= rackId;

        // Keep only the hundreds digit of the power level
        powerLevel = (powerLevel / 100) % 10;

        // Subtract 5 from the power level
        powerLevel -= 5;

        return powerLevel;
    }

    private (int x, int y, int power) FindLargestPowerSquare(int size)
    {
        int maxPower = int.MinValue;
        int maxX = 0, maxY = 0;

        // Calculate the summed-area table (integral image) for efficient square sum calculation
        int[,] sumTable = CalculateSummedAreaTable();

        // Find the square with the maximum power
        for (int x = 1; x <= GridSize - size + 1; x++)
        {
            for (int y = 1; y <= GridSize - size + 1; y++)
            {
                int power = CalculateSquarePower(sumTable, x, y, size);

                if (power > maxPower)
                {
                    maxPower = power;
                    maxX = x;
                    maxY = y;
                }
            }
        }

        return (maxX, maxY, maxPower);
    }

    private (int x, int y, int size) FindLargestPowerSquareAnySize()
    {
        int maxPower = int.MinValue;
        int maxX = 0, maxY = 0, maxSize = 0;

        // Calculate the summed-area table (integral image) for efficient square sum calculation
        int[,] sumTable = CalculateSummedAreaTable();

        // Try all possible square sizes
        for (int size = 1; size <= GridSize; size++)
        {
            var (x, y, power) = FindLargestPowerSquare(size);

            if (power > maxPower)
            {
                maxPower = power;
                maxX = x;
                maxY = y;
                maxSize = size;
            }
        }

        return (maxX, maxY, maxSize);
    }

    private int[,] CalculateSummedAreaTable()
    {
        // Compute summed-area table (integral image) for efficient 2D range queries
        int[,] sumTable = new int[GridSize + 1, GridSize + 1];

        for (int x = 1; x <= GridSize; x++)
        {
            for (int y = 1; y <= GridSize; y++)
            {
                sumTable[x, y] = _grid[x, y] +
                                sumTable[x - 1, y] +
                                sumTable[x, y - 1] -
                                sumTable[x - 1, y - 1];
            }
        }

        return sumTable;
    }

    private int CalculateSquarePower(int[,] sumTable, int x, int y, int size)
    {
        // Calculate the sum of a square using summed-area table in O(1) time
        return sumTable[x + size - 1, y + size - 1] -
               sumTable[x - 1, y + size - 1] -
               sumTable[x + size - 1, y - 1] +
               sumTable[x - 1, y - 1];
    }
}