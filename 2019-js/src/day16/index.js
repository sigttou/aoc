// src/day16.js
import run from "aocrunner";

const parseInput = (rawInput) => rawInput.trim().split("").map(Number);

const basePattern = [0, 1, 0, -1];

const applyPhase = (signal) => {
  const output = [];
  const len = signal.length;

  for (let i = 0; i < len; i++) {
    let sum = 0;
    for (let j = 0; j < len; j++) {
      const patternIndex = Math.floor((j + 1) / (i + 1)) % basePattern.length;
      sum += signal[j] * basePattern[patternIndex];
    }
    output.push(Math.abs(sum) % 10);
  }
  return output;
};

const part1 = (rawInput) => {
  let signal = parseInput(rawInput);
  for (let i = 0; i < 100; i++) {
    signal = applyPhase(signal);
  }
  return signal.slice(0, 8).join("");
};

const part2 = (rawInput) => {
  const input = parseInput(rawInput);
  const offset = parseInt(rawInput.slice(0, 7), 10);
  const totalLength = input.length * 10000;
  const size = totalLength - offset;

  // Only works because offset is in the second half
  let signal = new Array(size);
  for (let i = 0; i < size; i++) {
    signal[i] = input[(offset + i) % input.length];
  }

  for (let phase = 0; phase < 100; phase++) {
    for (let i = size - 2; i >= 0; i--) {
      signal[i] = (signal[i] + signal[i + 1]) % 10;
    }
  }

  return signal.slice(0, 8).join("");
};

run({
  part1: {
    tests: [
      { input: "80871224585914546619083218645595", expected: "24176176" },
      { input: "19617804207202209144916044189917", expected: "73745418" },
      { input: "69317163492948606335995924319873", expected: "52432133" },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      { input: "03036732577212944063491565474664", expected: "84462026" },
      { input: "02935109699940807407585447034323", expected: "78725270" },
      { input: "03081770884921959731165446850517", expected: "53553731" },
    ],
    solution: part2,
  },
  onlyTests: false,
});
