import run from "aocrunner";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

function runProgram(memory, inputs, pointer = 0) {
  memory = [...memory];
  let output = [];
  let ip = pointer;
  let inputIndex = 0;

  const getParam = (mode, param) => (mode === 0 ? memory[param] : param);

  while (memory[ip] !== 99) {
    const instruction = String(memory[ip]).padStart(5, "0");
    const opcode = Number(instruction.slice(3));
    const mode1 = Number(instruction[2]);
    const mode2 = Number(instruction[1]);

    const param1 = memory[ip + 1];
    const param2 = memory[ip + 2];
    const param3 = memory[ip + 3];

    const val1 = getParam(mode1, param1);
    const val2 = getParam(mode2, param2);

    switch (opcode) {
      case 1:
        memory[param3] = val1 + val2;
        ip += 4;
        break;
      case 2:
        memory[param3] = val1 * val2;
        ip += 4;
        break;
      case 3:
        if (inputIndex >= inputs.length)
          return { output, memory, ip, halted: false };
        memory[param1] = inputs[inputIndex++];
        ip += 2;
        break;
      case 4:
        output.push(val1);
        ip += 2;
        break;
      case 5:
        ip = val1 !== 0 ? val2 : ip + 3;
        break;
      case 6:
        ip = val1 === 0 ? val2 : ip + 3;
        break;
      case 7:
        memory[param3] = val1 < val2 ? 1 : 0;
        ip += 4;
        break;
      case 8:
        memory[param3] = val1 === val2 ? 1 : 0;
        ip += 4;
        break;
      default:
        throw new Error(`Unknown opcode ${opcode} at position ${ip}`);
    }
  }

  return { output, memory, ip, halted: true };
}

const permutations = (arr) => {
  if (arr.length === 0) return [[]];
  return arr.flatMap((val, idx) => {
    const rest = [...arr.slice(0, idx), ...arr.slice(idx + 1)];
    return permutations(rest).map((perm) => [val, ...perm]);
  });
};

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  let maxSignal = 0;

  for (const phases of permutations([0, 1, 2, 3, 4])) {
    let inputSignal = 0;
    for (const phase of phases) {
      const { output } = runProgram(program, [phase, inputSignal]);
      inputSignal = output[0];
    }
    maxSignal = Math.max(maxSignal, inputSignal);
  }

  return maxSignal;
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);
  let maxSignal = 0;

  for (const phases of permutations([5, 6, 7, 8, 9])) {
    const amps = phases.map(() => ({
      memory: [...program],
      ip: 0,
      halted: false,
    }));
    let inputSignal = 0;
    let firstInput = true;

    while (!amps[4].halted) {
      for (let i = 0; i < 5; i++) {
        const amp = amps[i];
        const input = firstInput ? [phases[i], inputSignal] : [inputSignal];
        const result = runProgram(amp.memory, input, amp.ip);
        amp.memory = result.memory;
        amp.ip = result.ip;
        amp.halted = result.halted;
        inputSignal = result.output[result.output.length - 1] ?? inputSignal;
      }
      firstInput = false;
    }

    maxSignal = Math.max(maxSignal, inputSignal);
  }

  return maxSignal;
};

run({
  part1: {
    tests: [
      {
        input: `3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0`,
        expected: 43210,
      },
      {
        input: `3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0`,
        expected: 54321,
      },
      {
        input: `3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0`,
        expected: 65210,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5`,
        expected: 139629729,
      },
      {
        input: `3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10`,
        expected: 18216,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
