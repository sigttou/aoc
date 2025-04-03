import run from "aocrunner";

const parseInput = (rawInput) => rawInput.split(",").map(Number);

const runIntcode = (program) => {
  const memory = [...program];
  let pointer = 0;

  while (memory[pointer] !== 99) {
    const [opcode, param1, param2, param3] = memory.slice(pointer, pointer + 4);
    if (opcode === 1) {
      memory[param3] = memory[param1] + memory[param2];
    } else if (opcode === 2) {
      memory[param3] = memory[param1] * memory[param2];
    } else {
      throw new Error(`Unknown opcode: ${opcode}`);
    }
    pointer += 4;
  }
  return memory;
};

const part1 = (rawInput) => {
  const input = parseInput(rawInput);
  if (input.length > 20) {
    input[1] = 12;
    input[2] = 2;
  }
  return runIntcode(input)[0];
};

const part2 = (rawInput) => {
  const input = parseInput(rawInput);
  for (let noun = 0; noun <= 99; noun++) {
    for (let verb = 0; verb <= 99; verb++) {
      const memory = [...input];
      memory[1] = noun;
      memory[2] = verb;
      if (runIntcode(memory)[0] === 19690720) {
        return 100 * noun + verb;
      }
    }
  }
};

run({
  part1: {
    tests: [
      {
        input: "1,0,0,0,99",
        expected: 2,
      },
      {
        input: "2,3,0,3,99",
        expected: 2,
      },
      {
        input: "2,4,4,5,99,0",
        expected: 2,
      },
      {
        input: "1,1,1,4,99,5,6,0,99",
        expected: 30,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
