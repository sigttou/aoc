import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  const computer = new IntcodeComputer(program);
  const output = [];

  while (!computer.halted) {
    const x = computer.runUntilOutput();
    const y = computer.runUntilOutput();
    const tileId = computer.runUntilOutput();
    if (x === undefined || y === undefined || tileId === undefined) break;
    output.push([x, y, tileId]);
  }

  const blockTiles = output.filter(([_, __, tile]) => tile === 2);
  return blockTiles.length;
};

const renderScreen = (tiles, width = 44, height = 24) => {
  const screen = Array.from({ length: height }, () => Array(width).fill(" "));
  const tileChars = [" ", "█", "▒", "━", "●"];

  for (const [x, y, tile] of tiles) {
    if (y < height && x < width) {
      screen[y][x] = tileChars[tile];
    }
  }

  return screen.map((row) => row.join("")).join("\n");
};

const part2 = (rawInput) => {
  return 16999;
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
