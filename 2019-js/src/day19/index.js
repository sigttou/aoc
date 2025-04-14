import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  let affected = 0;

  for (let y = 0; y < 50; y++) {
    for (let x = 0; x < 50; x++) {
      const computer = new IntcodeComputer(program);
      computer.addInput(x);
      computer.addInput(y);
      computer.run();
      affected += computer.getOutput()[0];
    }
  }

  return affected;
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);

  const isPulled = (x, y) => {
    const computer = new IntcodeComputer(program);
    computer.addInput(x);
    computer.addInput(y);
    computer.run();
    return computer.getOutput()[0] === 1;
  };

  let y = 100;
  let x = 0;

  while (true) {
    while (!isPulled(x, y)) {
      x++;
    }

    // Check top-right corner of 100x100 square
    if (isPulled(x + 99, y - 99)) {
      return x * 10000 + (y - 99);
    }

    y++;
  }
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
});
