// src/day15.js
import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const directions = [
  { dx: 0, dy: -1, command: 1 }, // north
  { dx: 0, dy: 1, command: 2 }, // south
  { dx: -1, dy: 0, command: 3 }, // west
  { dx: 1, dy: 0, command: 4 }, // east
];

const key = (x, y) => `${x},${y}`;

const explore = (program) => {
  const visited = new Map();
  const start = { x: 0, y: 0 };
  const queue = [
    { ...start, path: [], computer: new IntcodeComputer(program) },
  ];

  visited.set(key(0, 0), ".");
  let oxygenSystem = null;

  while (queue.length) {
    const { x, y, path, computer } = queue.shift();

    for (const { dx, dy, command } of directions) {
      const nx = x + dx;
      const ny = y + dy;
      const posKey = key(nx, ny);
      if (visited.has(posKey)) continue;

      const newComputer = new IntcodeComputer([...computer.memory]);
      newComputer.ip = computer.ip;
      newComputer.relativeBase = computer.relativeBase;
      newComputer.input = [...computer.input, command];

      const output = newComputer.run(() => newComputer.input.shift());

      if (output === 0) {
        visited.set(posKey, "#");
      } else {
        visited.set(posKey, output === 2 ? "O" : ".");
        const newPath = [...path, command];
        queue.push({ x: nx, y: ny, path: newPath, computer: newComputer });
        if (output === 2) {
          oxygenSystem = { x: nx, y: ny, path: newPath };
        }
      }
    }
  }

  return { visited, oxygenSystem };
};

const part1 = (rawInput) => {
  const program = parseInput(rawInput);
  const { oxygenSystem } = explore(program);
  return oxygenSystem.path.length;
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);
  const { visited, oxygenSystem } = explore(program);

  const filled = new Set([key(oxygenSystem.x, oxygenSystem.y)]);
  let minutes = 0;

  while (true) {
    const toFill = [];
    for (const pos of filled) {
      const [x, y] = pos.split(",").map(Number);
      for (const { dx, dy } of directions) {
        const nx = x + dx;
        const ny = y + dy;
        const nkey = key(nx, ny);
        if (visited.get(nkey) === "." && !filled.has(nkey)) {
          toFill.push(nkey);
        }
      }
    }
    if (toFill.length === 0) break;
    toFill.forEach((pos) => filled.add(pos));
    minutes++;
  }

  return minutes;
};

run({
  part1: {
    tests: [
      // no example tests for part 1
    ],
    solution: part1,
  },
  part2: {
    tests: [],
    solution: part2,
  },
  onlyTests: false,
});
