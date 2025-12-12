import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const NUM_COMPUTERS = 50;

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const runNetwork = (program, part) => {
  const inputQueues = Array.from({ length: NUM_COMPUTERS }, (_, i) => [i]);
  const outputBuffers = Array.from({ length: NUM_COMPUTERS }, () => []);
  const sentMinus1 = Array(NUM_COMPUTERS).fill(false);
  let nat = null;
  let lastNatY = null;

  const computers = [];
  for (let i = 0; i < NUM_COMPUTERS; i++) {
    const idx = i;
    const computer = new IntcodeComputer(
      [...program],
      () => {
        if (inputQueues[idx].length > 0) {
          sentMinus1[idx] = false;
          return inputQueues[idx].shift();
        }
        if (!sentMinus1[idx]) {
          sentMinus1[idx] = true;
          return -1;
        }
        return undefined; // pause
      },
      (value) => {
        outputBuffers[idx].push(value);
        if (outputBuffers[idx].length === 3) {
          const [dest, x, y] = outputBuffers[idx];
          outputBuffers[idx].length = 0;
          if (dest === 255) {
            if (part === 1) {
              throw { result: y };
            } else {
              nat = [x, y];
            }
          } else if (dest >= 0 && dest < NUM_COMPUTERS) {
            inputQueues[dest].push(x, y);
          }
        }
      },
    );
    computers.push(computer);
  }

  try {
    while (true) {
      let anyActivity = false;

      // Reset flags and run each computer
      for (let i = 0; i < NUM_COMPUTERS; i++) {
        sentMinus1[i] = false;
      }

      for (let i = 0; i < NUM_COMPUTERS; i++) {
        const prevIp = computers[i].ip;
        const hadInput = inputQueues[i].length > 0;
        computers[i].run();
        if (computers[i].ip !== prevIp || hadInput) anyActivity = true;
      }

      // Check for idle state in part 2
      if (part === 2 && !anyActivity && nat) {
        const [x, y] = nat;
        inputQueues[0].push(x, y);
        if (y === lastNatY) {
          return y;
        }
        lastNatY = y;
        anyActivity = true;
      }
    }
  } catch (e) {
    if (e.result !== undefined) return e.result;
    throw e;
  }
};

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  return runNetwork(program, 1);
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);
  return runNetwork(program, 2);
};

run({
  part1: {
    tests: [],
    solution: part1,
  },
  part2: {
    tests: [],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
