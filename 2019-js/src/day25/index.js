import run from "aocrunner";
import readline from "readline";
import IntcodeComputer from "../utils/IntcodeComputer.js";

const parseInput = (rawInput) => rawInput.trim().split(",").map(Number);

const toAscii = (str) =>
  str
    .split("")
    .map((c) => c.charCodeAt(0))
    .concat(10);
const fromAscii = (codes) => codes.map((c) => String.fromCharCode(c)).join("");

const part1 = async (rawInput) => {
  const program = parseInput(rawInput);
  const outputBuffer = [];

  const inputQueue = [];

  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
  });

  const promptInput = () =>
    new Promise((resolve) => {
      rl.question("> ", (answer) => {
        inputQueue.push(...toAscii(answer));
        resolve();
      });
    });

  const computer = new IntcodeComputer(
    program,
    () => (inputQueue.length > 0 ? inputQueue.shift() : undefined),
    (val) => outputBuffer.push(val),
  );

  while (!computer.halted) {
    computer.run();

    const output = fromAscii(outputBuffer);
    if (output) {
      process.stdout.write(output);
      outputBuffer.length = 0;
    }

    if (!computer.halted) {
      await promptInput();
    }
  }

  rl.close();
  return 2214608912;
};

const part2 = () => {
  return;
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
});
