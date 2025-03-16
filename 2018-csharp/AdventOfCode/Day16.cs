using AoCHelper;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day16 : BaseDay
{
    private readonly List<Sample> _samples = new();
    private readonly List<Instruction> _testProgram = new();

    public Day16()
    {
        var input = File.ReadAllText(InputFilePath);
        var parts = input.Split("\n\n\n\n");

        if (parts.Length >= 1)
        {
            var sampleParts = parts[0].Split("\n\n");
            foreach (var samplePart in sampleParts)
            {
                var lines = samplePart.Split('\n');

                if (lines.Length >= 3)
                {
                    var beforeMatch = Regex.Match(lines[0], @"Before:\s+\[(\d+),\s*(\d+),\s*(\d+),\s*(\d+)\]");
                    var instructionMatch = Regex.Match(lines[1], @"(\d+)\s+(\d+)\s+(\d+)\s+(\d+)");
                    var afterMatch = Regex.Match(lines[2], @"After:\s+\[(\d+),\s*(\d+),\s*(\d+),\s*(\d+)\]");

                    if (beforeMatch.Success && instructionMatch.Success && afterMatch.Success)
                    {
                        var before = new int[]
                        {
                            int.Parse(beforeMatch.Groups[1].Value),
                            int.Parse(beforeMatch.Groups[2].Value),
                            int.Parse(beforeMatch.Groups[3].Value),
                            int.Parse(beforeMatch.Groups[4].Value)
                        };

                        var instruction = new Instruction(
                            int.Parse(instructionMatch.Groups[1].Value),
                            int.Parse(instructionMatch.Groups[2].Value),
                            int.Parse(instructionMatch.Groups[3].Value),
                            int.Parse(instructionMatch.Groups[4].Value)
                        );

                        var after = new int[]
                        {
                            int.Parse(afterMatch.Groups[1].Value),
                            int.Parse(afterMatch.Groups[2].Value),
                            int.Parse(afterMatch.Groups[3].Value),
                            int.Parse(afterMatch.Groups[4].Value)
                        };

                        _samples.Add(new Sample(before, instruction, after));
                    }
                }
            }
        }

        if (parts.Length >= 2)
        {
            var programLines = parts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in programLines)
            {
                var instructionMatch = Regex.Match(line, @"(\d+)\s+(\d+)\s+(\d+)\s+(\d+)");
                if (instructionMatch.Success)
                {
                    _testProgram.Add(new Instruction(
                        int.Parse(instructionMatch.Groups[1].Value),
                        int.Parse(instructionMatch.Groups[2].Value),
                        int.Parse(instructionMatch.Groups[3].Value),
                        int.Parse(instructionMatch.Groups[4].Value)
                    ));
                }
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int samplesWithThreeOrMoreOpcodes = 0;

        foreach (var sample in _samples)
        {
            var matchingOpcodes = GetMatchingOpcodes(sample);
            if (matchingOpcodes.Count >= 3)
            {
                samplesWithThreeOrMoreOpcodes++;
            }
        }

        return new ValueTask<string>(samplesWithThreeOrMoreOpcodes.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var opcodeMap = DetermineOpcodeMap();

        var registers = new int[4];

        foreach (var instruction in _testProgram)
        {
            ExecuteOpcode(opcodeMap[instruction.Opcode], instruction.A, instruction.B, instruction.C, registers);
        }

        return new ValueTask<string>(registers[0].ToString());
    }

    private Dictionary<int, int> DetermineOpcodeMap()
    {
        var possibleOpcodes = new Dictionary<int, HashSet<int>>();
        for (int i = 0; i < 16; i++)
        {
            possibleOpcodes[i] = new HashSet<int>(Enumerable.Range(0, 16));
        }

        foreach (var sample in _samples)
        {
            var matchingOpcodes = GetMatchingOpcodes(sample);
            var opcodeNumber = sample.Instruction.Opcode;

            possibleOpcodes[opcodeNumber].IntersectWith(matchingOpcodes);
        }

        var opcodeMap = new Dictionary<int, int>();
        while (possibleOpcodes.Any(kv => kv.Value.Count > 0))
        {
            var definite = possibleOpcodes.FirstOrDefault(kv => kv.Value.Count == 1);
            if (definite.Value == null)
                break;

            var opcodeNumber = definite.Key;
            var opcode = definite.Value.First();

            opcodeMap[opcodeNumber] = opcode;

            foreach (var key in possibleOpcodes.Keys)
            {
                if (key != opcodeNumber)
                {
                    possibleOpcodes[key].Remove(opcode);
                }
            }

            possibleOpcodes[opcodeNumber].Clear();
        }

        return opcodeMap;
    }

    private List<int> GetMatchingOpcodes(Sample sample)
    {
        var matchingOpcodes = new List<int>();

        for (int opcodeNumber = 0; opcodeNumber < 16; opcodeNumber++)
        {
            var registersCopy = new int[4];
            Array.Copy(sample.Before, registersCopy, 4);

            ExecuteOpcode(opcodeNumber, sample.Instruction.A, sample.Instruction.B, sample.Instruction.C, registersCopy);

            if (registersCopy.SequenceEqual(sample.After))
            {
                matchingOpcodes.Add(opcodeNumber);
            }
        }

        return matchingOpcodes;
    }

    private void ExecuteOpcode(int opcode, int a, int b, int c, int[] registers)
    {
        switch (opcode)
        {
            case 0: // addr
                registers[c] = registers[a] + registers[b];
                break;
            case 1: // addi
                registers[c] = registers[a] + b;
                break;
            case 2: // mulr
                registers[c] = registers[a] * registers[b];
                break;
            case 3: // muli
                registers[c] = registers[a] * b;
                break;
            case 4: // banr
                registers[c] = registers[a] & registers[b];
                break;
            case 5: // bani
                registers[c] = registers[a] & b;
                break;
            case 6: // borr
                registers[c] = registers[a] | registers[b];
                break;
            case 7: // bori
                registers[c] = registers[a] | b;
                break;
            case 8: // setr
                registers[c] = registers[a];
                break;
            case 9: // seti
                registers[c] = a;
                break;
            case 10: // gtir
                registers[c] = a > registers[b] ? 1 : 0;
                break;
            case 11: // gtri
                registers[c] = registers[a] > b ? 1 : 0;
                break;
            case 12: // gtrr
                registers[c] = registers[a] > registers[b] ? 1 : 0;
                break;
            case 13: // eqir
                registers[c] = a == registers[b] ? 1 : 0;
                break;
            case 14: // eqri
                registers[c] = registers[a] == b ? 1 : 0;
                break;
            case 15: // eqrr
                registers[c] = registers[a] == registers[b] ? 1 : 0;
                break;
        }
    }
}

public record Sample(int[] Before, Instruction Instruction, int[] After);

public record Instruction(int Opcode, int A, int B, int C);