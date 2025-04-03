import run from "aocrunner";

const parseInput = (rawInput) => rawInput.split("-").map(Number);

const isValidPassword = (num) => {
  const str = num.toString();
  let hasDouble = false;

  for (let i = 0; i < str.length - 1; i++) {
    if (str[i] > str[i + 1]) return false;
    if (str[i] === str[i + 1]) hasDouble = true;
  }

  return hasDouble;
};

const part1 = (rawInput) => {
  const [start, end] = parseInput(rawInput);
  let count = 0;

  for (let num = start; num <= end; num++) {
    if (isValidPassword(num)) count++;
  }

  return count;
};

const isValidPasswordPart2 = (num) => {
  const str = num.toString();
  let groups = {};

  for (let i = 0; i < str.length; i++) {
    groups[str[i]] = (groups[str[i]] || 0) + 1;
    if (i > 0 && str[i] < str[i - 1]) return false;
  }

  return Object.values(groups).includes(2);
};

const part2 = (rawInput) => {
  const [start, end] = parseInput(rawInput);
  let count = 0;

  for (let num = start; num <= end; num++) {
    if (isValidPasswordPart2(num)) count++;
  }

  return count;
};

run({
  part1: {
    tests: [
      { input: "111111-111111", expected: 1 },
      { input: "223450-223450", expected: 0 },
      { input: "123789-123789", expected: 0 },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      { input: "112233-112233", expected: 1 },
      { input: "123444-123444", expected: 0 },
      { input: "111122-111122", expected: 1 },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
