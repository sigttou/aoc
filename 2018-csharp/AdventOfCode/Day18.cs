using AoCHelper;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day18 : BaseDay
{
    private readonly string[] _input;
    private readonly int _width;
    private readonly int _height;

    public Day18()
    {
        _input = File.ReadAllLines(InputFilePath);
        _height = _input.Length;
        _width = _input[0].Length;
    }

    private char[,] ParseInput()
    {
        char[,] grid = new char[_height, _width];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                grid[y, x] = _input[y][x];
            }
        }

        return grid;
    }

    private char[,] SimulateMinute(char[,] grid)
    {
        char[,] newGrid = new char[_height, _width];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                int trees = 0;
                int lumberyards = 0;

                for (int dy = -1; dy <= 1; dy++)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        int nx = x + dx;
                        int ny = y + dy;

                        if (nx >= 0 && nx < _width && ny >= 0 && ny < _height)
                        {
                            if (grid[ny, nx] == '|') trees++;
                            else if (grid[ny, nx] == '#') lumberyards++;
                        }
                    }
                }

                char currentAcre = grid[y, x];
                char newAcre = currentAcre;

                if (currentAcre == '.' && trees >= 3)
                {
                    newAcre = '|';
                }
                else if (currentAcre == '|' && lumberyards >= 3)
                {
                    newAcre = '#';
                }
                else if (currentAcre == '#' && !(lumberyards >= 1 && trees >= 1))
                {
                    newAcre = '.';
                }

                newGrid[y, x] = newAcre;
            }
        }

        return newGrid;
    }

    private int CalculateResourceValue(char[,] grid)
    {
        int trees = 0;
        int lumberyards = 0;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (grid[y, x] == '|') trees++;
                else if (grid[y, x] == '#') lumberyards++;
            }
        }

        return trees * lumberyards;
    }

    public override ValueTask<string> Solve_1()
    {
        char[,] grid = ParseInput();

        for (int i = 0; i < 10; i++)
        {
            grid = SimulateMinute(grid);
        }

        int resourceValue = CalculateResourceValue(grid);
        return new ValueTask<string>(resourceValue.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        char[,] grid = ParseInput();

        Dictionary<string, int> seenStates = new Dictionary<string, int>();

        for (int minute = 0; minute < 1000000000; minute++)
        {
            string currentState = GridToString(grid);

            if (seenStates.TryGetValue(currentState, out int previousMinute))
            {
                int cycleLength = minute - previousMinute;

                int remainingMinutes = 1000000000 - minute;
                int skipCycles = remainingMinutes / cycleLength;

                minute += skipCycles * cycleLength;

                for (int i = 0; i < remainingMinutes % cycleLength; i++)
                {
                    grid = SimulateMinute(grid);
                }

                break;
            }

            seenStates[currentState] = minute;
            grid = SimulateMinute(grid);
        }

        int resourceValue = CalculateResourceValue(grid);
        return new ValueTask<string>(resourceValue.ToString());
    }

    private string GridToString(char[,] grid)
    {
        char[] chars = new char[_height * _width];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                chars[y * _width + x] = grid[y, x];
            }
        }
        return new string(chars);
    }
}