import run from "aocrunner";

const parseInput = (rawInput) => rawInput.split(",").map(Number);

function runProgram(memory, input) {
  let output = [];
  let ip = 0; // instruction pointer

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
      case 1: // Add
        memory[param3] = val1 + val2;
        ip += 4;
        break;
      case 2: // Multiply
        memory[param3] = val1 * val2;
        ip += 4;
        break;
      case 3: // Input
        memory[param1] = input;
        ip += 2;
        break;
      case 4: // Output
        output.push(val1);
        ip += 2;
        break;
      case 5: // Jump-if-true
        ip = val1 !== 0 ? val2 : ip + 3;
        break;
      case 6: // Jump-if-false
        ip = val1 === 0 ? val2 : ip + 3;
        break;
      case 7: // Less than
        memory[param3] = val1 < val2 ? 1 : 0;
        ip += 4;
        break;
      case 8: // Equals
        memory[param3] = val1 === val2 ? 1 : 0;
        ip += 4;
        break;
      default:
        throw new Error(`Unknown opcode ${opcode} at position ${ip}`);
    }
  }

  return output;
}

const part1 = (rawInput) => {
  const memory = parseInput(rawInput);
  const result = runProgram([...memory], 1); // system ID = 1
  return result[result.length - 1]; // Last output is the diagnostic code
};

const part2 = (rawInput) => {
  const memory = parseInput(rawInput);
  const result = runProgram([...memory], 5); // system ID = 5
  return result[result.length - 1]; // Last output is the diagnostic code
};

run({
  part1: {
    tests: [
      {
        input: "3,0,4,0,99",
        expected: 1,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input:
          "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31," +
          "1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104," +
          "999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99",
        expected: 999, // input < 8
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
