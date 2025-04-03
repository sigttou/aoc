import run from "aocrunner";

const parseInput = (rawInput) =>
  rawInput.split("\n").map((line) => line.split(","));

const getWirePoints = (instructions) => {
  const points = new Map();
  let x = 0,
    y = 0,
    steps = 0;

  const directions = {
    U: [0, 1],
    D: [0, -1],
    L: [-1, 0],
    R: [1, 0],
  };

  for (const instruction of instructions) {
    const dir = instruction[0];
    const length = parseInt(instruction.slice(1), 10);
    const [dx, dy] = directions[dir];

    for (let i = 0; i < length; i++) {
      x += dx;
      y += dy;
      steps++;
      const key = `${x},${y}`;
      if (!points.has(key)) {
        points.set(key, steps);
      }
    }
  }

  return points;
};

const part1 = (rawInput) => {
  const [wire1, wire2] = parseInput(rawInput);
  const points1 = getWirePoints(wire1);
  const points2 = getWirePoints(wire2);

  const intersections = [...points1.keys()].filter((point) =>
    points2.has(point),
  );

  return Math.min(
    ...intersections.map((point) => {
      const [x, y] = point.split(",").map(Number);
      return Math.abs(x) + Math.abs(y);
    }),
  );
};

const part2 = (rawInput) => {
  const [wire1, wire2] = parseInput(rawInput);
  const points1 = getWirePoints(wire1);
  const points2 = getWirePoints(wire2);

  const intersections = [...points1.keys()].filter((point) =>
    points2.has(point),
  );

  return Math.min(
    ...intersections.map((point) => points1.get(point) + points2.get(point)),
  );
};

run({
  part1: {
    tests: [
      {
        input:
          "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83",
        expected: 159,
      },
      {
        input:
          "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7",
        expected: 135,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input:
          "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83",
        expected: 610,
      },
      {
        input:
          "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7",
        expected: 410,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
