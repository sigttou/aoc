using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day07 : BaseDay
    {
        private readonly string _input;

        public Day07()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var steps = ParseInstructions(_input);
            var order = GetStepOrder(steps);

            return new ValueTask<string>(order);
        }

        public override ValueTask<string> Solve_2()
        {
            var steps = ParseInstructions(_input);
            int totalTime = GetTotalTimeToComplete(steps, 5, 60);

            return new ValueTask<string>(totalTime.ToString());
        }

        private Dictionary<char, List<char>> ParseInstructions(string input)
        {
            var steps = new Dictionary<char, List<char>>();

            foreach (var line in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split(' ');
                var prerequisite = parts[1][0];
                var step = parts[7][0];

                if (!steps.ContainsKey(step))
                {
                    steps[step] = new List<char>();
                }
                if (!steps.ContainsKey(prerequisite))
                {
                    steps[prerequisite] = new List<char>();
                }

                steps[step].Add(prerequisite);
            }

            return steps;
        }

        private string GetStepOrder(Dictionary<char, List<char>> steps)
        {
            var order = new List<char>();
            var availableSteps = new SortedSet<char>(steps.Where(kv => kv.Value.Count == 0).Select(kv => kv.Key));

            while (availableSteps.Count > 0)
            {
                var currentStep = availableSteps.First();
                availableSteps.Remove(currentStep);
                order.Add(currentStep);

                foreach (var step in steps.Keys.Where(step => steps[step].Contains(currentStep)).ToList())
                {
                    steps[step].Remove(currentStep);
                    if (steps[step].Count == 0)
                    {
                        availableSteps.Add(step);
                    }
                }
            }

            return new string(order.ToArray());
        }

        private int GetTotalTimeToComplete(Dictionary<char, List<char>> steps, int workerCount, int baseDuration)
        {
            var stepDurations = steps.Keys.ToDictionary(step => step, step => baseDuration + (step - 'A' + 1));
            var availableSteps = new SortedSet<char>(steps.Where(kv => kv.Value.Count == 0).Select(kv => kv.Key));
            var workers = new (char step, int timeLeft)[workerCount];
            int currentTime = 0;

            while (availableSteps.Count > 0 || workers.Any(w => w.timeLeft > 0))
            {
                for (int i = 0; i < workers.Length; i++)
                {
                    if (workers[i].timeLeft == 0 && availableSteps.Count > 0)
                    {
                        var nextStep = availableSteps.First();
                        availableSteps.Remove(nextStep);
                        workers[i] = (nextStep, stepDurations[nextStep]);
                    }
                }

                int timeStep = workers.Where(w => w.timeLeft > 0).Min(w => w.timeLeft);
                currentTime += timeStep;

                for (int i = 0; i < workers.Length; i++)
                {
                    if (workers[i].timeLeft > 0)
                    {
                        workers[i].timeLeft -= timeStep;
                        if (workers[i].timeLeft == 0)
                        {
                            var completedStep = workers[i].step;
                            foreach (var step in steps.Keys.Where(step => steps[step].Contains(completedStep)).ToList())
                            {
                                steps[step].Remove(completedStep);
                                if (steps[step].Count == 0)
                                {
                                    availableSteps.Add(step);
                                }
                            }
                        }
                    }
                }
            }

            return currentTime;
        }
    }
}