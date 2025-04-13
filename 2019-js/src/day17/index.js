import run from "aocrunner";
import { IntcodeComputer } from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) =>
  rawInput.split(",").map((n) => Number(n.trim()));

const toAsciiGrid = (output) => {
  const chars = output.map((n) => String.fromCharCode(n));
  return chars
    .join("")
    .trim()
    .split("\n")
    .map((line) => line.split(""));
};

const isScaffold = (grid, x, y) =>
  grid[y]?.[x] === "#" &&
  grid[y - 1]?.[x] === "#" &&
  grid[y + 1]?.[x] === "#" &&
  grid[y]?.[x - 1] === "#" &&
  grid[y]?.[x + 1] === "#";

const part1 = (rawInput) => {
  const input = parseInput(rawInput);
  const computer = new IntcodeComputer(input);
  const output = computer.run();
  const grid = toAsciiGrid(output);

  let sum = 0;
  for (let y = 1; y < grid.length - 1; y++) {
    for (let x = 1; x < grid[y].length - 1; x++) {
      if (isScaffold(grid, x, y)) {
        sum += x * y;
      }
    }
  }

  return sum;
};

const encodeAscii = (lines) =>
  lines
    .join("\n")
    .split("")
    .map((c) => c.charCodeAt(0))
    .concat(10); // Final newline

const part2 = (rawInput) => {
  const input = parseInput(rawInput);
  input[0] = 2; // Wake up the vacuum robot

  const movementProgram = [
    "A,B,A,C,B,C,A,C,B,C", // main routine
    "L,8,R,10,L,10", // function A
    "R,10,L,8,L,8,L,10", // function B
    "L,4,L,6,L,8,L,8", // function C
    "n", // no video feed
  ];

  const movementAscii = movementProgram.flatMap((line) => [
    ...line.split("").map((c) => c.charCodeAt(0)),
    10,
  ]);

  const computer = new IntcodeComputer(input);
  const output = computer.run(movementAscii);

  return output.at(-1);
};

run({
  part1: {
    tests: [
      {
        input: `104,35,104,46,104,10,104,35,104,46,104,10,104,35,104,35,104,10,99`,
        expected: 0,
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
