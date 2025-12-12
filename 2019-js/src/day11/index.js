import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const DIRECTIONS = [
  [0, -1], // up
  [1, 0], // right
  [0, 1], // down
  [-1, 0], // left
];

const key = (x, y) => `${x},${y}`;

const paintHull = (program, startColor = 0) => {
  let x = 0,
    y = 0,
    dir = 0;
  const panels = new Map();
  panels.set(key(x, y), startColor);

  const outputs = [];
  const computer = new IntcodeComputer(
    program,
    () => panels.get(key(x, y)) || 0,
    (value) => {
      outputs.push(value);
      if (outputs.length === 2) {
        const [paint, turn] = outputs;
        outputs.length = 0;
        panels.set(key(x, y), paint);
        dir = (dir + (turn === 0 ? 3 : 1)) % 4;
        x += DIRECTIONS[dir][0];
        y += DIRECTIONS[dir][1];
      }
    },
  );

  computer.run();
  return panels;
};

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  const panels = paintHull(program);
  return panels.size;
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);
  const panels = paintHull(program, 1);

  const painted = [...panels.entries()]
    .map(([coord, color]) => ({
      x: parseInt(coord.split(",")[0], 10),
      y: parseInt(coord.split(",")[1], 10),
      color,
    }))
    .filter((p) => p.color === 1);

  const minX = Math.min(...painted.map((p) => p.x));
  const maxX = Math.max(...painted.map((p) => p.x));
  const minY = Math.min(...painted.map((p) => p.y));
  const maxY = Math.max(...painted.map((p) => p.y));

  const grid = Array.from({ length: maxY - minY + 1 }, () =>
    Array(maxX - minX + 1).fill(" "),
  );

  painted.forEach(({ x, y }) => {
    grid[y - minY][x - minX] = "#";
  });

  return grid.map((row) => row.join("")).join("\n");
};

run({
  part1: {
    tests: [
      {
        input: `3,0,4,0,99`,
        expected: 1,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `99`, // robot halts immediately, paints only the starting white panel
        expected: "#",
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
