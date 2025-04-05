import run from "aocrunner";

const parseInput = (rawInput) =>
  rawInput.split("\n").map((line) => line.trim());

function runProgram(memory, input) {
  let output = [];
  let ip = 0; // instruction pointer

  const getParam = (mode, param) => (mode === 0 ? memory[param] : param);

  while (memory[ip] !== 99) {
    const instruction = String(memory[ip]).padStart(5, "0");
    const opcode = Number(instruction.slice(3));
    const mode1 = Number(instruction[2]);
    const mode2 = Number(instruction[1]);

    const param1 = memory[ip + 1];
    const param2 = memory[ip + 2];
    const param3 = memory[ip + 3];

    const val1 = getParam(mode1, param1);
    const val2 = getParam(mode2, param2);

    switch (opcode) {
      case 1:
        memory[param3] = val1 + val2;
        ip += 4;
        break;
      case 2:
        memory[param3] = val1 * val2;
        ip += 4;
        break;
      case 3:
        memory[param1] = input;
        ip += 2;
        break;
      case 4:
        output.push(val1);
        ip += 2;
        break;
      case 5:
        ip = val1 !== 0 ? val2 : ip + 3;
        break;
      case 6:
        ip = val1 === 0 ? val2 : ip + 3;
        break;
      case 7:
        memory[param3] = val1 < val2 ? 1 : 0;
        ip += 4;
        break;
      case 8:
        memory[param3] = val1 === val2 ? 1 : 0;
        ip += 4;
        break;
      default:
        throw new Error(`Unknown opcode ${opcode} at position ${ip}`);
    }
  }

  return output;
}

const part1 = (rawInput) => {
  const orbits = parseInput(rawInput);
  const map = new Map();

  for (const orbit of orbits) {
    const [center, orbiter] = orbit.split(")");
    map.set(orbiter, center);
  }

  let total = 0;
  for (const node of map.keys()) {
    let current = node;
    while (map.has(current)) {
      current = map.get(current);
      total++;
    }
  }

  return total;
};

const part2 = (rawInput) => {
  const orbits = parseInput(rawInput);
  const map = new Map();

  for (const orbit of orbits) {
    const [center, orbiter] = orbit.split(")");
    map.set(orbiter, center);
  }

  const getPathToCOM = (start) => {
    const path = [];
    let current = start;
    while (map.has(current)) {
      current = map.get(current);
      path.push(current);
    }
    return path;
  };

  const pathYou = getPathToCOM("YOU");
  const pathSan = getPathToCOM("SAN");

  const setYou = new Set(pathYou);
  const firstCommon = pathSan.find((planet) => setYou.has(planet));

  return pathYou.indexOf(firstCommon) + pathSan.indexOf(firstCommon);
};

run({
  part1: {
    tests: [
      {
        input: `COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L`,
        expected: 42,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\nK)YOU\nI)SAN`,
        expected: 4,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
