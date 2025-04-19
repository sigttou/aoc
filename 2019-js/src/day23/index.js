import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const NUM_COMPUTERS = 50;

const parseInput = (rawInput) => rawInput.trim().split(",").map(BigInt);

const runNetwork = (program, part) => {
  const computers = [];
  const queues = Array.from({ length: NUM_COMPUTERS }, () => []);
  const idle = Array(NUM_COMPUTERS).fill(false);
  let nat = null;
  let lastNatY = null;

  // Initialize computers with their address
  for (let i = 0; i < NUM_COMPUTERS; i++) {
    const computer = new IntcodeComputer(program);
    computer.input(i);
    computers.push(computer);
  }

  while (true) {
    let activity = false;

    for (let i = 0; i < NUM_COMPUTERS; i++) {
      const comp = computers[i];

      if (queues[i].length === 0) {
        comp.input(-1);
      } else {
        const [x, y] = queues[i].shift();
        comp.input(x);
        comp.input(y);
      }

      comp.runUntilOutput();

      while (comp.hasOutput()) {
        const dest = Number(comp.output());
        const x = comp.output();
        const y = comp.output();

        activity = true;

        if (dest === 255) {
          if (part === 1) {
            return y.toString();
          } else {
            nat = [x, y];
          }
        } else if (dest >= 0 && dest < NUM_COMPUTERS) {
          queues[dest].push([x, y]);
        }
      }
    }

    // Part 2: check if all queues are empty and no computers were active
    if (part === 2 && !activity && queues.every((q) => q.length === 0)) {
      if (nat) {
        const [x, y] = nat;
        queues[0].push([x, y]);
        if (y === lastNatY) {
          return y.toString();
        }
        lastNatY = y;
      }
    }
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
