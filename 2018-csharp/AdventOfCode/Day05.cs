using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day05 : BaseDay
    {
        private readonly string _input;

        public Day05()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            string polymer = _input.Trim();
            string reactedPolymer = ReactPolymer(polymer);

            return new ValueTask<string>(reactedPolymer.Length.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            string polymer = _input.Trim();
            int shortestLength = GetShortestPolymerLengthByRemovingUnits(polymer);

            return new ValueTask<string>(shortestLength.ToString());
        }

        private string ReactPolymer(string polymer)
        {
            var stack = new Stack<char>();

            foreach (var unit in polymer)
            {
                if (stack.Count > 0 && AreOppositePolarities(stack.Peek(), unit))
                {
                    stack.Pop();
                }
                else
                {
                    stack.Push(unit);
                }
            }

            return new string(stack.Reverse().ToArray());
        }

        private bool AreOppositePolarities(char unit1, char unit2)
        {
            return char.ToLower(unit1) == char.ToLower(unit2) && unit1 != unit2;
        }

        private int GetShortestPolymerLengthByRemovingUnits(string polymer)
        {
            var unitTypes = polymer.ToLower().Distinct();
            int shortestLength = polymer.Length;

            foreach (var unitType in unitTypes)
            {
                string modifiedPolymer = new string(polymer.Where(c => char.ToLower(c) != unitType).ToArray());
                string reactedPolymer = ReactPolymer(modifiedPolymer);
                if (reactedPolymer.Length < shortestLength)
                {
                    shortestLength = reactedPolymer.Length;
                }
            }

            return shortestLength;
        }
    }
}