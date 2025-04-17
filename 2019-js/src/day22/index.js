import run from "aocrunner";

const parseInput = (rawInput) => rawInput.trim().split("\n");

const dealIntoNewStack = (pos, size) => size - 1 - pos;
const cutN = (pos, size, n) => (pos - n + size) % size;
const dealWithIncrementN = (pos, size, n) => (pos * n) % size;

const parseInstructions = (lines) =>
  lines.map((line) => {
    if (line === "deal into new stack") {
      return (pos, size) => dealIntoNewStack(pos, size);
    } else if (line.startsWith("cut ")) {
      const n = parseInt(line.split(" ")[1], 10);
      return (pos, size) => cutN(pos, size, n);
    } else if (line.startsWith("deal with increment")) {
      const n = parseInt(line.split(" ").at(-1), 10);
      return (pos, size) => dealWithIncrementN(pos, size, n);
    }
  });

const mod = (a, m) => ((a % m) + m) % m;

const modInv = (a, m) => {
  // Extended Euclidean Algorithm for modular inverse using BigInts
  let [t, newT] = [0n, 1n];
  let [r, newR] = [m, a];
  while (newR !== 0n) {
    const q = r / newR;
    [t, newT] = [newT, t - q * newT];
    [r, newR] = [newR, r - q * newR];
  }
  if (r > 1n) throw new Error("a is not invertible");
  if (t < 0n) t += m;
  return t;
};

const modPow = (base, exp, modn) => {
  let result = 1n;
  base = base % modn;
  while (exp > 0n) {
    if (exp % 2n === 1n) result = (result * base) % modn;
    base = (base * base) % modn;
    exp /= 2n;
  }
  return result;
};

const part1 = (rawInput) => {
  const input = parseInput(rawInput);
  const instructions = parseInstructions(input);
  const deckSize = 10007;
  let pos = 2019;
  for (const fn of instructions) {
    pos = fn(pos, deckSize);
  }
  return pos;
};

const part2 = (rawInput) => {
  const input = parseInput(rawInput);
  const deckSize = 119315717514047n;
  const repeatCount = 101741582076661n;
  const targetPos = 2020n;

  let increment = 1n;
  let offset = 0n;

  for (const line of input) {
    if (line === "deal into new stack") {
      increment = -increment % deckSize;
      offset = (offset + increment) % deckSize;
    } else if (line.startsWith("cut ")) {
      const n = BigInt(line.split(" ")[1]);
      offset = (offset + n * increment) % deckSize;
    } else if (line.startsWith("deal with increment")) {
      const n = BigInt(line.split(" ").at(-1));
      const inv = modInv(n, deckSize);
      increment = (increment * inv) % deckSize;
    }
  }

  const finalIncrement = modPow(increment, repeatCount, deckSize);
  const oneMinusIncrement = (1n - increment + deckSize) % deckSize;
  const finalOffset =
    (offset * (1n - finalIncrement) * modInv(oneMinusIncrement, deckSize)) %
    deckSize;

  return mod(finalOffset + finalIncrement * targetPos, deckSize);
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
