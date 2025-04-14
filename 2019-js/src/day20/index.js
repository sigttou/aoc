import run from "aocrunner";

const parseInput = (rawInput) =>
  rawInput.split("\n").map((line) => line.split(""));

function findPortals(map) {
  const portals = new Map();
  const portalPositions = new Map();

  const height = map.length;
  const width = map[0].length;

  const isLetter = (c) => c >= "A" && c <= "Z";

  for (let y = 0; y < height - 1; y++) {
    for (let x = 0; x < width - 1; x++) {
      if (isLetter(map[y][x])) {
        for (const [dx, dy] of [
          [0, 1],
          [1, 0],
        ]) {
          const nx = x + dx;
          const ny = y + dy;

          if (!isLetter(map[ny][nx])) continue;

          const label = map[y][x] + map[ny][nx];

          let px = x - dx;
          let py = y - dy;

          if (
            px < 0 ||
            py < 0 ||
            px >= width ||
            py >= height ||
            map[py][px] !== "."
          ) {
            px = x + 2 * dx;
            py = y + 2 * dy;
          }

          if (map[py]?.[px] === ".") {
            if (!portals.has(label)) {
              portals.set(label, []);
            }
            portals.get(label).push([px, py]);
            portalPositions.set(`${px},${py}`, label);
          }
        }
      }
    }
  }

  return { portals, portalPositions };
}

function bfs(map, portals, portalPositions, recursive = false) {
  const height = map.length;
  const width = map[0].length;

  const isOuter = ([x, y]) =>
    x <= 2 || y <= 2 || x >= width - 3 || y >= height - 3;

  const [start] = portals.get("AA");
  const [end] = portals.get("ZZ");

  const queue = [[...start, 0, 0]]; // x, y, level, steps
  const visited = new Set();

  while (queue.length > 0) {
    const [x, y, level, steps] = queue.shift();
    const key = `${x},${y},${level}`;

    if (visited.has(key)) continue;
    visited.add(key);

    if (x === end[0] && y === end[1] && level === 0) return steps;

    for (const [dx, dy] of [
      [0, 1],
      [1, 0],
      [0, -1],
      [-1, 0],
    ]) {
      const nx = x + dx;
      const ny = y + dy;

      if (map[ny]?.[nx] === ".") {
        queue.push([nx, ny, level, steps + 1]);
      }
    }

    const portal = portalPositions.get(`${x},${y}`);
    if (portal && portal !== "AA" && portal !== "ZZ") {
      const [a, b] = portals.get(portal);
      const dest = a[0] === x && a[1] === y ? b : a;
      const goingOuter = isOuter([x, y]);

      let nextLevel = level;
      if (recursive) {
        nextLevel += goingOuter ? -1 : 1;
        if (nextLevel < 0) continue;
      }

      queue.push([dest[0], dest[1], nextLevel, steps + 1]);
    }
  }

  return -1;
}

const part1 = (rawInput) => {
  const map = parseInput(rawInput);
  const { portals, portalPositions } = findPortals(map);
  return bfs(map, portals, portalPositions, false);
};

const part2 = (rawInput) => {
  const map = parseInput(rawInput);
  const { portals, portalPositions } = findPortals(map);
  return bfs(map, portals, portalPositions, true);
};

run({
  part1: {
    tests: [
      {
        input: `         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       `,
        expected: 23,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `             Z L X W       C                 
             Z P Q B       K                 
  ###########.#.#.#.#######.###############  
  #...#.......#.#.......#.#.......#.#.#...#  
  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  
  #.#...#.#.#...#.#.#...#...#...#.#.......#  
  #.###.#######.###.###.#.###.###.#.#######  
  #...#.......#.#...#...#.............#...#  
  #.#########.#######.#.#######.#######.###  
  #...#.#    F       R I       Z    #.#.#.#  
  #.###.#    D       E C       H    #.#.#.#  
  #.#...#                           #...#.#  
  #.###.#                           #.###.#  
  #.#....OA                       WB..#.#..ZH
  #.###.#                           #.#.#.#  
CJ......#                           #.....#  
  #######                           #######  
  #.#....CK                         #......IC
  #.###.#                           #.###.#  
  #.....#                           #...#.#  
  ###.###                           #.#.#.#  
XF....#.#                         RF..#.#.#  
  #####.#                           #######  
  #......CJ                       NM..#...#  
  ###.#.#                           #.###.#  
RE....#.#                           #......RF
  ###.###        X   X       L      #.#.#.#  
  #.....#        F   Q       P      #.#.#.#  
  ###.###########.###.#######.#########.###  
  #.....#...#.....#.......#...#.....#.#...#  
  #####.#.###.#######.#######.###.###.#.#.#  
  #.......#.......#.#.#.#.#...#...#...#.#.#  
  #####.###.#####.#.#.#.#.###.###.#.###.###  
  #.......#.....#.#...#...............#...#  
  #############.#.#.###.###################  
               A O F   N                     
               A A D   M                     `,
        expected: 396,
      },
    ],
    solution: part2,
  },
  trimTestInputs: false,
  onlyTests: false,
});
