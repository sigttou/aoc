import run from "aocrunner";

const parseInput = (rawInput) => rawInput.split("\n").map(Number);

const calculateFuel = (mass) => Math.floor(mass / 3) - 2;

const calculateTotalFuel = (mass) => {
  let totalFuel = 0;
  let fuel = calculateFuel(mass);
  while (fuel > 0) {
    totalFuel += fuel;
    fuel = calculateFuel(fuel);
  }
  return totalFuel;
};

const part1 = (rawInput) => {
  const input = parseInput(rawInput);
  return input.reduce((sum, mass) => sum + calculateFuel(mass), 0);
};

const part2 = (rawInput) => {
  const input = parseInput(rawInput);
  return input.reduce((sum, mass) => sum + calculateTotalFuel(mass), 0);
};

run({
  part1: {
    tests: [
      {
        input: "12\n14\n1969\n100756",
        expected: 2 + 2 + 654 + 33583,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: "14",
        expected: 2,
      },
      {
        input: "1969",
        expected: 966,
      },
      {
        input: "100756",
        expected: 50346,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
