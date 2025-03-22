using AoCHelper;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day21 : BaseDay
{
    private readonly string _input;
    private readonly int _coreConstant;

    public Day21()
    {
        _input = File.ReadAllText(InputFilePath);

        // Parse the core constant from: seti X 1 4
        var match = Regex.Match(_input, @"seti (\d+) 1 4");
        if (!match.Success)
            throw new InvalidDataException("Failed to parse core constant from input.");

        _coreConstant = int.Parse(match.Groups[1].Value);
    }

    public override ValueTask<string> Solve_1()
    {
        int result = RunPart1();
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int result = RunPart2();
        return new(result.ToString());
    }

    private int RunPart1()
    {
        int r4 = 0, r3;

        while (true)
        {
            r3 = r4 | 65536;
            r4 = _coreConstant;

            while (true)
            {
                r4 = (((r4 + (r3 & 255)) & 0xFFFFFF) * 65899) & 0xFFFFFF;
                if (r3 < 256)
                    break;
                r3 /= 256;
            }

            return r4;
        }
    }

    private int RunPart2()
    {
        int r4 = 0, r3;
        HashSet<int> seen = new();
        int lastUnique = 0;

        while (true)
        {
            r3 = r4 | 65536;
            r4 = _coreConstant;

            while (true)
            {
                r4 = (((r4 + (r3 & 255)) & 0xFFFFFF) * 65899) & 0xFFFFFF;
                if (r3 < 256)
                    break;
                r3 /= 256;
            }

            if (seen.Contains(r4))
            {
                return lastUnique;
            }

            seen.Add(r4);
            lastUnique = r4;
        }
    }
}
