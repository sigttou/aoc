using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day09 : BaseDay
    {
        private readonly string _input;

        public Day09()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var (players, lastMarble) = ParseInput(_input);
            var highScore = GetHighScore(players, lastMarble);

            return new ValueTask<string>(highScore.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var (players, lastMarble) = ParseInput(_input);
            var highScore = GetHighScore(players, lastMarble * 100);

            return new ValueTask<string>(highScore.ToString());
        }

        private (int players, int lastMarble) ParseInput(string input)
        {
            var parts = input.Split(new[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
            return (int.Parse(parts[0]), int.Parse(parts[6]));
        }

        private long GetHighScore(int players, int lastMarble)
        {
            var scores = new long[players];
            var circle = new LinkedList<int>();
            var currentMarble = circle.AddFirst(0);

            for (int marble = 1; marble <= lastMarble; marble++)
            {
                if (marble % 23 == 0)
                {
                    var player = marble % players;
                    scores[player] += marble;

                    for (int i = 0; i < 7; i++)
                    {
                        currentMarble = currentMarble.Previous ?? circle.Last;
                    }

                    scores[player] += currentMarble.Value;
                    var toRemove = currentMarble;
                    currentMarble = currentMarble.Next ?? circle.First;
                    circle.Remove(toRemove);
                }
                else
                {
                    currentMarble = currentMarble.Next ?? circle.First;
                    currentMarble = circle.AddAfter(currentMarble, marble);
                }
            }

            return scores.Max();
        }
    }
}