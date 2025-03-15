using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day14 : BaseDay
{
    private readonly int _puzzleInput;

    public Day14()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        _puzzleInput = int.Parse(lines[0]);
    }

    public override ValueTask<string> Solve_1()
    {
        var recipes = new List<int> { 3, 7 };

        int elf1Position = 0;
        int elf2Position = 1;

        while (recipes.Count < _puzzleInput + 10)
        {
            int sum = recipes[elf1Position] + recipes[elf2Position];

            if (sum >= 10)
            {
                recipes.Add(sum / 10);
                recipes.Add(sum % 10);
            }
            else
            {
                recipes.Add(sum);
            }

            elf2Position = (elf2Position + 1 + recipes[elf2Position]) % recipes.Count;
        }

        var result = new StringBuilder();
        for (int i = _puzzleInput; i < _puzzleInput + 10; i++)
        {
            result.Append(recipes[i]);
        }

        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var inputDigits = _puzzleInput.ToString().Select(c => int.Parse(c.ToString())).ToArray();

        var recipes = new List<int> { 3, 7 };

        int elf1Position = 0;
        int elf2Position = 1;

        while (true)
        {
            int sum = recipes[elf1Position] + recipes[elf2Position];

            if (sum >= 10)
            {
                recipes.Add(sum / 10);

                if (EndsWithSequence(recipes, inputDigits))
                {
                    return new ValueTask<string>((recipes.Count - inputDigits.Length).ToString());
                }

                recipes.Add(sum % 10);
            }
            else
            {
                recipes.Add(sum);
            }

            if (EndsWithSequence(recipes, inputDigits))
            {
                return new ValueTask<string>((recipes.Count - inputDigits.Length).ToString());
            }

            elf1Position = (elf1Position + 1 + recipes[elf1Position]) % recipes.Count;
            elf2Position = (elf2Position + 1 + recipes[elf2Position]) % recipes.Count;
        }
    }

    private bool EndsWithSequence(List<int> list, int[] sequence)
    {
        if (list.Count < sequence.Length)
            return false;

        for (int i = 0; i < sequence.Length; i++)
        {
            if (list[list.Count - sequence.Length + i] != sequence[i])
                return false;
        }

        return true;
    }
}