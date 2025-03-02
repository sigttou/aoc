using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day01 : BaseDay
    {
        private readonly string _input;

        public Day01()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            int resultingFrequency = _input
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Sum();

            return new ValueTask<string>(resultingFrequency.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var frequencyChanges = _input
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var seenFrequencies = new HashSet<int>();
            int currentFrequency = 0;
            int index = 0;

            while (true)
            {
                if (seenFrequencies.Contains(currentFrequency))
                {
                    return new ValueTask<string>(currentFrequency.ToString());
                }

                seenFrequencies.Add(currentFrequency);
                currentFrequency += frequencyChanges[index % frequencyChanges.Length];
                index++;
            }
        }
    }
}