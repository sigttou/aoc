import run from "aocrunner";
import PriorityQueue from "js-priority-queue";

const parseInput = (rawInput) =>
  rawInput
    .trim()
    .split("\n")
    .map((line) => line.split(""));

const isKey = (c) => c >= "a" && c <= "z";
const isDoor = (c) => c >= "A" && c <= "Z";
const keyBit = (k) => 1 << (k.charCodeAt(0) - 97);
const allKeysMask = (map) => {
  let mask = 0;
  for (const row of map) {
    for (const ch of row) {
      if (isKey(ch)) mask |= keyBit(ch);
    }
  }
  return mask;
};

const neighbors = [
  [0, 1],
  [1, 0],
  [0, -1],
  [-1, 0],
];

const bfs = (map, starts) => {
  const keyMask = allKeysMask(map);
  const seen = new Set();
  const queue = new PriorityQueue({ comparator: (a, b) => a.steps - b.steps });

  queue.queue({ steps: 0, positions: starts, keys: 0 });

  while (queue.length) {
    const { steps, positions, keys } = queue.dequeue();
    const keyStr = `${positions.map((p) => p.join(",")).join("|")}|${keys}`;
    if (seen.has(keyStr)) continue;
    seen.add(keyStr);

    if (keys === keyMask) return steps;

    for (let i = 0; i < positions.length; i++) {
      const [sx, sy] = positions[i];
      const visited = new Set([[sx, sy].join(",")]);
      const localQueue = [[sx, sy, 0]];

      while (localQueue.length) {
        const [x, y, dist] = localQueue.shift();

        for (const [dx, dy] of neighbors) {
          const nx = x + dx,
            ny = y + dy;
          const key = `${nx},${ny}`;
          if (visited.has(key)) continue;
          visited.add(key);

          const cell = map[ny]?.[nx];
          if (!cell || cell === "#") continue;
          if (isDoor(cell) && !(keys & keyBit(cell.toLowerCase()))) continue;

          if (isKey(cell) && !(keys & keyBit(cell))) {
            const newKeys = keys | keyBit(cell);
            const newPositions = [...positions];
            newPositions[i] = [nx, ny];
            queue.queue({
              steps: steps + dist + 1,
              positions: newPositions,
              keys: newKeys,
            });
          } else {
            localQueue.push([nx, ny, dist + 1]);
          }
        }
      }
    }
  }
};

const part1 = (rawInput) => {
  const map = parseInput(rawInput);
  const start = [];

  for (let y = 0; y < map.length; y++) {
    for (let x = 0; x < map[0].length; x++) {
      if (map[y][x] === "@") {
        start.push([x, y]);
        break;
      }
    }
  }

  return bfs(map, start);
};

const part2 = (rawInput) => {
  const map = parseInput(rawInput);
  let sx, sy;

  for (let y = 0; y < map.length; y++) {
    for (let x = 0; x < map[0].length; x++) {
      if (map[y][x] === "@") {
        sx = x;
        sy = y;
        break;
      }
    }
  }

  // Replace center with 4 bots
  map[sy][sx] = "#";
  map[sy - 1][sx] = "#";
  map[sy + 1][sx] = "#";
  map[sy][sx - 1] = "#";
  map[sy][sx + 1] = "#";

  map[sy - 1][sx - 1] = "@";
  map[sy - 1][sx + 1] = "@";
  map[sy + 1][sx - 1] = "@";
  map[sy + 1][sx + 1] = "@";

  const starts = [
    [sx - 1, sy - 1],
    [sx + 1, sy - 1],
    [sx - 1, sy + 1],
    [sx + 1, sy + 1],
  ];

  return bfs(map, starts);
};

run({
  part1: {
    tests: [
      {
        input: `#########
#b.A.@.a#
#########`,
        expected: 8,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `#######
#a.#Cd#
##...##
##.@.##
##...##
#cB#Ab#
#######`,
        expected: 8,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
});
