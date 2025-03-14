using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day12 : BaseDay
{
    private readonly string _initialState;
    private readonly Dictionary<string, char> _rules;

    public Day12()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        // Parse initial state
        _initialState = Regex.Match(lines[0], @"initial state: ([\.#]+)").Groups[1].Value;

        // Parse rules
        _rules = new Dictionary<string, char>();
        for (int i = 2; i < lines.Length; i++)
        {
            var matches = Regex.Match(lines[i], @"([\.#]{5}) => ([\.#])");
            if (matches.Success)
            {
                _rules[matches.Groups[1].Value] = matches.Groups[2].Value[0];
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var result = SimulateGenerations(20);
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(EfficientSimulateForBillions(50_000_000_000).ToString());
    }

    private long SimulateGenerations(int generations)
    {
        // Use HashSet for better performance with a potentially large number of pots
        var plants = new HashSet<int>();

        // Initialize with initial state
        for (int i = 0; i < _initialState.Length; i++)
        {
            if (_initialState[i] == '#')
            {
                plants.Add(i);
            }
        }

        for (int gen = 0; gen < generations; gen++)
        {
            var nextPlants = new HashSet<int>();

            // Calculate the range to check
            int minPot = plants.Count > 0 ? plants.Min() - 2 : -2;
            int maxPot = plants.Count > 0 ? plants.Max() + 2 : 2;

            for (int pot = minPot; pot <= maxPot; pot++)
            {
                // Build the pattern for this pot and its neighbors
                var pattern = new StringBuilder(5);
                for (int j = -2; j <= 2; j++)
                {
                    pattern.Append(plants.Contains(pot + j) ? '#' : '.');
                }

                // Apply the rule
                if (_rules.TryGetValue(pattern.ToString(), out char nextState) && nextState == '#')
                {
                    nextPlants.Add(pot);
                }
            }

            plants = nextPlants;
        }

        // Sum the indices of all pots with plants
        return plants.Sum(pot => (long)pot);
    }

    private long EfficientSimulateForBillions(long targetGeneration)
    {
        // Use HashSet for efficient lookups
        var plants = new HashSet<int>();

        // Initialize with initial state
        for (int i = 0; i < _initialState.Length; i++)
        {
            if (_initialState[i] == '#')
            {
                plants.Add(i);
            }
        }

        // We need to track when the pattern stabilizes (same shape, just shifted)
        var history = new Dictionary<string, (int generation, int minPot, long sum)>();

        int generation = 0;

        while (generation < targetGeneration)
        {
            // Store the current state pattern with its generation
            int minPot = plants.Count > 0 ? plants.Min() : 0;
            string statePattern = GetPatternSignature(plants, minPot);
            long currentSum = plants.Sum(pot => (long)pot);

            // Check if we've seen this pattern before
            if (history.TryGetValue(statePattern, out var previous))
            {
                // We've found a cycle!
                int cycleLength = generation - previous.generation;
                int potShift = minPot - previous.minPot;

                // Calculate how many complete cycles remain
                long remainingGenerations = targetGeneration - generation;
                long cycles = remainingGenerations / cycleLength;

                // Each cycle shifts all pots by 'potShift'
                // So each plant index increases by potShift * cycles
                return plants.Sum(pot => pot + (potShift * cycles));
            }

            // Record the current pattern
            history[statePattern] = (generation, minPot, currentSum);

            // Generate the next state
            var nextPlants = new HashSet<int>();

            // Calculate the range to check
            minPot = plants.Count > 0 ? plants.Min() - 2 : -2;
            int maxPot = plants.Count > 0 ? plants.Max() + 2 : 2;

            for (int pot = minPot; pot <= maxPot; pot++)
            {
                // Build the pattern for this pot and its neighbors
                var pattern = new StringBuilder(5);
                for (int j = -2; j <= 2; j++)
                {
                    pattern.Append(plants.Contains(pot + j) ? '#' : '.');
                }

                // Apply the rule
                if (_rules.TryGetValue(pattern.ToString(), out char nextState) && nextState == '#')
                {
                    nextPlants.Add(pot);
                }
            }

            plants = nextPlants;
            generation++;
        }

        // If we somehow reach this point (should not happen with 50 billion generations)
        return plants.Sum(pot => (long)pot);
    }

    private string GetPatternSignature(HashSet<int> plants, int minPot)
    {
        // Create a position-independent signature of the current plant pattern
        // by recording the relative distances between plants
        if (plants.Count == 0) return "empty";

        var sb = new StringBuilder();
        int previousPot = 0;
        bool first = true;

        foreach (int pot in plants.OrderBy(p => p))
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.Append(pot - previousPot);
                sb.Append(',');
            }
            previousPot = pot;
        }

        return sb.ToString();
    }
}