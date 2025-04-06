import run from "aocrunner";

const parseInput = (rawInput) => rawInput.trim().split(",").map(BigInt);

function runProgram(memory, inputs, pointer = 0n, relativeBase = 0n) {
  memory = [...memory];
  const output = [];
  let ip = pointer;
  let inputIndex = 0;
  let rb = relativeBase;
  const mem = new Map();
  memory.forEach((v, i) => mem.set(BigInt(i), v));

  const get = (addr) => mem.get(addr) ?? 0n;
  const set = (addr, val) => mem.set(addr, val);

  const getParam = (mode, param) => {
    if (mode === 0) return get(param); // position mode
    if (mode === 1) return param; // immediate mode
    if (mode === 2) return get(rb + param); // relative mode
    throw new Error("Unknown parameter mode: " + mode);
  };

  const getAddr = (mode, param) => {
    if (mode === 0) return param;
    if (mode === 2) return rb + param;
    throw new Error("Invalid address mode: " + mode);
  };

  while (get(ip) !== 99n) {
    const instruction = String(get(ip)).padStart(5, "0");
    const opcode = Number(instruction.slice(3));
    const mode1 = Number(instruction[2]);
    const mode2 = Number(instruction[1]);
    const mode3 = Number(instruction[0]);

    const param1 = get(ip + 1n);
    const param2 = get(ip + 2n);
    const param3 = get(ip + 3n);

    const val1 = getParam(mode1, param1);
    const val2 = getParam(mode2, param2);

    switch (opcode) {
      case 1:
        set(getAddr(mode3, param3), val1 + val2);
        ip += 4n;
        break;
      case 2:
        set(getAddr(mode3, param3), val1 * val2);
        ip += 4n;
        break;
      case 3:
        if (inputIndex >= inputs.length)
          return {
            output,
            memory: Array.from(mem.values()),
            ip,
            relativeBase: rb,
            halted: false,
          };
        set(getAddr(mode1, param1), inputs[inputIndex++]);
        ip += 2n;
        break;
      case 4:
        output.push(val1);
        ip += 2n;
        break;
      case 5:
        ip = val1 !== 0n ? val2 : ip + 3n;
        break;
      case 6:
        ip = val1 === 0n ? val2 : ip + 3n;
        break;
      case 7:
        set(getAddr(mode3, param3), val1 < val2 ? 1n : 0n);
        ip += 4n;
        break;
      case 8:
        set(getAddr(mode3, param3), val1 === val2 ? 1n : 0n);
        ip += 4n;
        break;
      case 9:
        rb += val1;
        ip += 2n;
        break;
      default:
        throw new Error(`Unknown opcode ${opcode} at position ${ip}`);
    }
  }

  return {
    output,
    memory: Array.from(mem.values()),
    ip,
    relativeBase: rb,
    halted: true,
  };
}

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  const { output } = runProgram(program, [1n]);
  return output[0];
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);
  const { output } = runProgram(program, [2n]);
  return output[0];
};

run({
  part1: {
    tests: [
      {
        input: `109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99`,
        expected: 109n,
      },
      {
        input: `1102,34915192,34915192,7,4,7,99,0`,
        expected: 1219070632396864n,
      },
      {
        input: `104,1125899906842624,99`,
        expected: 1125899906842624n,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99`,
        expected: 109n,
      },
      {
        input: `1102,34915192,34915192,7,4,7,99,0`,
        expected: 1219070632396864n,
      },
      {
        input: `104,1125899906842624,99`,
        expected: 1125899906842624n,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
