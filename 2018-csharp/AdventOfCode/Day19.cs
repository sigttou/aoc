using AoCHelper;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day19 : BaseDay
{
    private readonly string[] _input;
    private readonly List<Instruction> _program = new();
    private readonly int _ipRegister;

    public Day19()
    {
        _input = File.ReadAllLines(InputFilePath);

        var ipBindingMatch = Regex.Match(_input[0], @"#ip (\d+)");
        _ipRegister = int.Parse(ipBindingMatch.Groups[1].Value);

        for (int i = 1; i < _input.Length; i++)
        {
            _program.Add(ParseInstruction(_input[i]));
        }
    }

    private Instruction ParseInstruction(string line)
    {
        var parts = line.Split(' ');
        return new Instruction
        {
            Opcode = parts[0],
            A = int.Parse(parts[1]),
            B = int.Parse(parts[2]),
            C = int.Parse(parts[3])
        };
    }

    private void ExecuteInstruction(Instruction instruction, int[] registers)
    {
        switch (instruction.Opcode)
        {
            case "addr":
                registers[instruction.C] = registers[instruction.A] + registers[instruction.B];
                break;

            case "addi":
                registers[instruction.C] = registers[instruction.A] + instruction.B;
                break;

            case "mulr":
                registers[instruction.C] = registers[instruction.A] * registers[instruction.B];
                break;

            case "muli":
                registers[instruction.C] = registers[instruction.A] * instruction.B;
                break;

            case "banr":
                registers[instruction.C] = registers[instruction.A] & registers[instruction.B];
                break;

            case "bani":
                registers[instruction.C] = registers[instruction.A] & instruction.B;
                break;

            case "borr":
                registers[instruction.C] = registers[instruction.A] | registers[instruction.B];
                break;

            case "bori":
                registers[instruction.C] = registers[instruction.A] | instruction.B;
                break;

            case "setr":
                registers[instruction.C] = registers[instruction.A];
                break;

            case "seti":
                registers[instruction.C] = instruction.A;
                break;

            case "gtir":
                registers[instruction.C] = instruction.A > registers[instruction.B] ? 1 : 0;
                break;

            case "gtri":
                registers[instruction.C] = registers[instruction.A] > instruction.B ? 1 : 0;
                break;

            case "gtrr":
                registers[instruction.C] = registers[instruction.A] > registers[instruction.B] ? 1 : 0;
                break;

            case "eqir":
                registers[instruction.C] = instruction.A == registers[instruction.B] ? 1 : 0;
                break;

            case "eqri":
                registers[instruction.C] = registers[instruction.A] == instruction.B ? 1 : 0;
                break;

            case "eqrr":
                registers[instruction.C] = registers[instruction.A] == registers[instruction.B] ? 1 : 0;
                break;
        }
    }

    private int RunProgram(int[] initialRegisters)
    {
        int[] registers = (int[])initialRegisters.Clone();
        int ip = 0;

        while (ip >= 0 && ip < _program.Count)
        {
            registers[_ipRegister] = ip;

            var instruction = _program[ip];
            ExecuteInstruction(instruction, registers);

            ip = registers[_ipRegister];
            ip++;
        }

        return registers[0];
    }

    private int AnalyzeProgram()
    {
        int targetNumber = 0;

        int[] registers = new int[6];
        registers[0] = 1; // Part 2 initialization
        int ip = 0;

        int instructionsRun = 0;
        int maxInstructions = 1000;

        while (ip >= 0 && ip < _program.Count && instructionsRun < maxInstructions)
        {
            registers[_ipRegister] = ip;
            var instruction = _program[ip];
            ExecuteInstruction(instruction, registers);
            ip = registers[_ipRegister];
            ip++;
            instructionsRun++;

            targetNumber = Math.Max(targetNumber, registers.Max());
        }

        int sum = 0;
        for (int i = 1; i <= targetNumber; i++)
        {
            if (targetNumber % i == 0)
            {
                sum += i;
            }
        }

        return sum;
    }

    public override ValueTask<string> Solve_1()
    {
        int[] registers = new int[6];
        int result = RunProgram(registers);
        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int result = AnalyzeProgram();
        return new ValueTask<string>(result.ToString());
    }

    private class Instruction
    {
        public string Opcode { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }
}