import run from "aocrunner";

const parseInput = (raw) =>
  raw
    .trim()
    .split("\n")
    .map((line) => line.trim().split(""));

const biodiversityRating = (grid) =>
  grid.flat().reduce((acc, cell, i) => acc + (cell === "#" ? 2 ** i : 0), 0);

const getNeighbors = (x, y) => [
  [x, y - 1],
  [x, y + 1],
  [x - 1, y],
  [x + 1, y],
];

const part1 = (rawInput) => {
  let grid = parseInput(rawInput);
  const seen = new Set();

  while (true) {
    const key = grid.map((row) => row.join("")).join("");
    if (seen.has(key)) return biodiversityRating(grid);
    seen.add(key);

    const next = Array.from({ length: 5 }, (_, y) =>
      Array.from({ length: 5 }, (_, x) => {
        const bugs = getNeighbors(x, y).filter(
          ([nx, ny]) => grid[ny]?.[nx] === "#",
        ).length;
        return grid[y][x] === "#"
          ? bugs === 1
            ? "#"
            : "."
          : bugs === 1 || bugs === 2
          ? "#"
          : ".";
      }),
    );

    grid = next;
  }
};

const emptyGrid = () => Array.from({ length: 5 }, () => Array(5).fill("."));

const countAdjacentBugs = (levels, z, x, y) => {
  let count = 0;
  const dirs = [
    [0, -1],
    [0, 1],
    [-1, 0],
    [1, 0],
  ];
  for (const [dx, dy] of dirs) {
    const nx = x + dx;
    const ny = y + dy;
    if (nx === 2 && ny === 2) {
      // into inner level
      const inner = levels.get(z + 1);
      if (!inner) continue;
      if (x === 2 && y === 1)
        for (let i = 0; i < 5; i++) if (inner[0][i] === "#") count++;
      if (x === 2 && y === 3)
        for (let i = 0; i < 5; i++) if (inner[4][i] === "#") count++;
      if (x === 1 && y === 2)
        for (let i = 0; i < 5; i++) if (inner[i][0] === "#") count++;
      if (x === 3 && y === 2)
        for (let i = 0; i < 5; i++) if (inner[i][4] === "#") count++;
    } else if (nx < 0 || nx > 4 || ny < 0 || ny > 4) {
      // into outer level
      const outer = levels.get(z - 1);
      if (!outer) continue;
      if (nx === -1 && outer[2][1] === "#") count++;
      if (nx === 5 && outer[2][3] === "#") count++;
      if (ny === -1 && outer[1][2] === "#") count++;
      if (ny === 5 && outer[3][2] === "#") count++;
    } else {
      if (levels.get(z)?.[ny]?.[nx] === "#") count++;
    }
  }
  return count;
};

const part2 = (rawInput) => {
  const base = parseInput(rawInput);
  base[2][2] = "?"; // middle cell represents recursion
  const levels = new Map();
  levels.set(
    0,
    base.map((row) => [...row]),
  );

  for (let minute = 0; minute < 200; minute++) {
    const newLevels = new Map();
    const min = Math.min(...levels.keys()) - 1;
    const max = Math.max(...levels.keys()) + 1;

    for (let z = min; z <= max; z++) {
      const oldGrid = levels.get(z) || emptyGrid();
      const newGrid = emptyGrid();
      for (let y = 0; y < 5; y++) {
        for (let x = 0; x < 5; x++) {
          if (x === 2 && y === 2) continue; // skip center
          const bugs = countAdjacentBugs(levels, z, x, y);
          if (oldGrid[y][x] === "#" && bugs === 1) newGrid[y][x] = "#";
          else if (oldGrid[y][x] === "." && (bugs === 1 || bugs === 2))
            newGrid[y][x] = "#";
        }
      }
      newLevels.set(z, newGrid);
    }

    levels.clear();
    for (const [z, g] of newLevels) levels.set(z, g);
  }

  let total = 0;
  for (const grid of levels.values()) {
    for (const row of grid) {
      for (const cell of row) {
        if (cell === "#") total++;
      }
    }
  }

  return total;
};

run({
  part1: {
    tests: [
      {
        input: `....#
#..#.
#..##
..#..
#....`,
        expected: 2129920,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `....#
#..#.
#..##
..#..
#....`,
        expected: 1922,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
});
