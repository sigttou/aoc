import run from "aocrunner";
import IntcodeComputer from "../utils/IntcodeComputer.js"; // assumes you saved it here

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

function runSpringscript(program, script) {
  const inputQueue = script
    .trim()
    .split("\n")
    .map((line) => [...line, "\n"])
    .flat()
    .map((char) => char.charCodeAt(0));
  const output = [];
  const computer = new IntcodeComputer(
    program,
    () => inputQueue.shift(),
    (value) => output.push(value),
  );
  computer.run();
  const last = output[output.length - 1];
  const ascii = output
    .slice(0, -1)
    .map((c) => String.fromCharCode(c))
    .join("");
  if (last > 127) {
    return last; // Hull damage
  } else {
    console.log(ascii); // Springdroid died
    return -1;
  }
}

const part1 = (rawInput) => {
  const program = parseInput(rawInput);

  const script = `
NOT A T
NOT B J
OR T J
NOT C T
OR T J
AND D J
WALK
`;

  return runSpringscript(program, script);
};

const part2 = (rawInput) => {
  const program = parseInput(rawInput);

  const script = `
NOT A T
NOT T T
AND B T
AND C T
NOT T J
AND D J
NOT H T
NOT T T
OR E T
AND T J
RUN
`;

  return runSpringscript(program, script);
};

run({
  part1: {
    solution: part1,
  },
  part2: {
    solution: part2,
  },
});
