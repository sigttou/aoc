import run from "aocrunner";

const parseInput = (rawInput) =>
  rawInput
    .trim()
    .split("\n")
    .map((line, y) =>
      [...line].map((c, x) => (c === "#" ? { x, y } : null)).filter(Boolean),
    )
    .flat();

function gcd(a, b) {
  while (b !== 0) {
    [a, b] = [b, a % b];
  }
  return Math.abs(a);
}

function angleFromUp(dx, dy) {
  const angle = Math.atan2(dx, -dy);
  return (angle + 2 * Math.PI) % (2 * Math.PI);
}

function getBestLocation(asteroids) {
  let maxVisible = 0;
  let best = null;

  for (const a of asteroids) {
    const directions = new Set();
    for (const b of asteroids) {
      if (a === b) continue;
      const dx = b.x - a.x;
      const dy = b.y - a.y;
      const div = gcd(dx, dy);
      directions.add(`${dx / div},${dy / div}`);
    }
    if (directions.size > maxVisible) {
      maxVisible = directions.size;
      best = a;
    }
  }

  return { location: best, visible: maxVisible };
}

const part1 = (rawInput) => {
  const asteroids = parseInput(rawInput);
  const { visible } = getBestLocation(asteroids);
  return visible;
};

const part2 = (rawInput) => {
  const asteroids = parseInput(rawInput);
  const { location: station } = getBestLocation(asteroids);

  const angles = new Map();
  for (const a of asteroids) {
    if (a === station) continue;
    const dx = a.x - station.x;
    const dy = a.y - station.y;
    const angle = angleFromUp(dx, dy);
    const dist = dx * dx + dy * dy;
    if (!angles.has(angle)) angles.set(angle, []);
    angles.get(angle).push({ ...a, dist });
  }

  const sortedAngles = [...angles.entries()]
    .map(([angle, list]) => {
      return [angle, list.sort((a, b) => a.dist - b.dist)];
    })
    .sort((a, b) => a[0] - b[0]);

  let vaporized = [];
  while (vaporized.length < 200) {
    for (const [, group] of sortedAngles) {
      if (group.length > 0) {
        vaporized.push(group.shift());
      }
    }
  }

  const target = vaporized[199];
  return target.x * 100 + target.y;
};

run({
  part1: {
    tests: [
      {
        input: `.#..#\n.....\n#####\n....#\n...##`,
        expected: 8,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `.#..##.###...#######\n##.############..##.\n.#.######.########.#\n.###.#######.####.#.\n#####.##.#.##.###.##\n..#####..#.#########\n####################\n#.####....###.#.#.##\n##.#################\n#####.##.###..####..\n..######..##.#######\n####.##.####...##..#\n.#####..#.######.###\n##...#.##########...\n#.##########.#######\n.####.#.###.###.#.##\n....##.##.###..#####\n.#.#.###########.###\n#.#.#.#####.####.###\n###.##.####.##.#..##`,
        expected: 802,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
