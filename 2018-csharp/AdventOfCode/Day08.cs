using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day08 : BaseDay
    {
        private readonly string _input;

        public Day08()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var numbers = _input.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

            var (sum, _) = ParseNode(numbers, 0);
            return new ValueTask<string>(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var numbers = _input.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

            var (value, _) = ParseNodeForValue(numbers, 0);
            return new ValueTask<string>(value.ToString());
        }

        private (int sum, int nextIndex) ParseNode(int[] numbers, int startIndex)
        {
            int childCount = numbers[startIndex];
            int metadataCount = numbers[startIndex + 1];
            int currentIndex = startIndex + 2;

            int sum = 0;

            for (int i = 0; i < childCount; i++)
            {
                var (childSum, nextIndex) = ParseNode(numbers, currentIndex);
                sum += childSum;
                currentIndex = nextIndex;
            }

            for (int i = 0; i < metadataCount; i++)
            {
                sum += numbers[currentIndex];
                currentIndex++;
            }

            return (sum, currentIndex);
        }

        private (int value, int nextIndex) ParseNodeForValue(int[] numbers, int startIndex)
        {
            int childCount = numbers[startIndex];
            int metadataCount = numbers[startIndex + 1];
            int currentIndex = startIndex + 2;

            var childValues = new List<int>();

            for (int i = 0; i < childCount; i++)
            {
                var (childValue, nextIndex) = ParseNodeForValue(numbers, currentIndex);
                childValues.Add(childValue);
                currentIndex = nextIndex;
            }

            int value = 0;

            if (childCount == 0)
            {
                for (int i = 0; i < metadataCount; i++)
                {
                    value += numbers[currentIndex];
                    currentIndex++;
                }
            }
            else
            {
                for (int i = 0; i < metadataCount; i++)
                {
                    int metadata = numbers[currentIndex];
                    if (metadata > 0 && metadata <= childValues.Count)
                    {
                        value += childValues[metadata - 1];
                    }
                    currentIndex++;
                }
            }

            return (value, currentIndex);
        }
    }
}