import run from "aocrunner";

const parseInput = (rawInput) => {
  const reactions = new Map();

  rawInput
    .trim()
    .split("\n")
    .forEach((line) => {
      const [inputs, output] = line.split(" => ");
      const [amount, chemical] = output.split(" ");
      reactions.set(chemical, {
        amount: Number(amount),
        inputs: inputs.split(", ").map((input) => {
          const [qty, name] = input.split(" ");
          return { name, qty: Number(qty) };
        }),
      });
    });

  return reactions;
};

const getOreRequirement = (reactions, fuelAmount = 1) => {
  const needs = { FUEL: fuelAmount };
  const leftovers = {};

  const produce = (chemical, amountNeeded) => {
    if (chemical === "ORE") {
      return amountNeeded;
    }

    const leftover = leftovers[chemical] || 0;
    const amountToProduce = Math.max(0, amountNeeded - leftover);
    const reaction = reactions.get(chemical);

    const batches = Math.ceil(amountToProduce / reaction.amount);
    const totalProduced = batches * reaction.amount;
    leftovers[chemical] =
      (leftovers[chemical] || 0) + totalProduced - amountNeeded;

    return reaction.inputs.reduce((oreSum, { name, qty }) => {
      return oreSum + produce(name, qty * batches);
    }, 0);
  };

  return produce("FUEL", fuelAmount);
};

const part1 = (rawInput) => {
  const reactions = parseInput(rawInput);
  return getOreRequirement(reactions);
};

const part2 = (rawInput) => {
  const reactions = parseInput(rawInput);
  const oreAvailable = 1_000_000_000_000;

  // Binary search to maximize FUEL
  let low = 1;
  let high = 1_000_000_000;

  while (low < high) {
    const mid = Math.ceil((low + high) / 2);
    const oreNeeded = getOreRequirement(reactions, mid);
    if (oreNeeded > oreAvailable) {
      high = mid - 1;
    } else {
      low = mid;
    }
  }

  return low;
};

run({
  part1: {
    tests: [
      {
        input: `
10 ORE => 10 A
1 ORE => 1 B
7 A, 1 B => 1 C
7 A, 1 C => 1 D
7 A, 1 D => 1 E
7 A, 1 E => 1 FUEL
        `,
        expected: 31,
      },
      {
        input: `
9 ORE => 2 A
8 ORE => 3 B
7 ORE => 5 C
3 A, 4 B => 1 AB
5 B, 7 C => 1 BC
4 C, 1 A => 1 CA
2 AB, 3 BC, 4 CA => 1 FUEL
        `,
        expected: 165,
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
