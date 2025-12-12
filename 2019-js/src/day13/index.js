// day13/index.js

import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  const tiles = new Map();
  const outputs = [];

  const computer = new IntcodeComputer(
    program,
    () => 0,
    (value) => {
      outputs.push(value);
      if (outputs.length === 3) {
        const [x, y, tile] = outputs;
        outputs.length = 0;
        tiles.set(`${x},${y}`, tile);
      }
    },
  );

  computer.run();
  return [...tiles.values()].filter((tile) => tile === 2).length;
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);
  program[0] = 2; // Free play
  const tiles = new Map();
  let ballX = 0;
  let paddleX = 0;
  let score = 0;
  const outputs = [];

  const computer = new IntcodeComputer(
    program,
    () => Math.sign(ballX - paddleX),
    (value) => {
      outputs.push(value);
      if (outputs.length === 3) {
        const [x, y, tile] = outputs;
        outputs.length = 0;
        if (x === -1 && y === 0) {
          score = tile;
        } else {
          tiles.set(`${x},${y}`, tile);
          if (tile === 3) paddleX = x;
          if (tile === 4) ballX = x;
        }
      }
    },
  );

  computer.run();
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
