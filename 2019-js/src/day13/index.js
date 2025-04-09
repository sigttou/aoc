// day13/index.js

import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  const computer = new IntcodeComputer(program);
  const tiles = new Map();

  while (!computer.halted) {
    const out = computer.run();
    if (!out) break;
    const [x, y, tile] = out;
    tiles.set(`${x},${y}`, tile);
  }

  return [...tiles.values()].filter((tile) => tile === 2).length;
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);
  program[0] = 2; // Free play
  const computer = new IntcodeComputer(program);
  const tiles = new Map();
  let ballX = 0;
  let paddleX = 0;
  let score = 0;

  const inputProvider = () => Math.sign(ballX - paddleX);

  while (!computer.halted) {
    const out = computer.run(inputProvider);
    if (!out) break;
    const [x, y, tile] = out;

    if (x === -1 && y === 0) {
      score = tile;
    } else {
      tiles.set(`${x},${y}`, tile);
      if (tile === 3) paddleX = x;
      if (tile === 4) ballX = x;
    }
  }

  return score;
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
