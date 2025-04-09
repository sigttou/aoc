import fs from "fs/promises";
import path from "path";
import { fileURLToPath } from "url";

class IntcodeComputer {
  constructor(program) {
    this.memory = [...program];
    this.ip = 0;
    this.relativeBase = 0;
    this.input = [];
    this.output = [];
    this.halted = false;
  }

  getValue(mode, param) {
    if (mode === 0) return this.memory[param] || 0;
    if (mode === 1) return param;
    if (mode === 2) return this.memory[this.relativeBase + param] || 0;
  }

  setValue(mode, param, value) {
    const address = mode === 2 ? this.relativeBase + param : param;
    this.memory[address] = value;
  }

  run(inputProvider = () => 0) {
    while (!this.halted) {
      const opcode = this.memory[this.ip] % 100;
      const modes = Math.floor(this.memory[this.ip] / 100)
        .toString()
        .padStart(3, "0")
        .split("")
        .map(Number)
        .reverse();

      const param = (n) => this.memory[this.ip + n];
      const get = (n) => this.getValue(modes[n - 1], param(n));
      const set = (n, value) => this.setValue(modes[n - 1], param(n), value);

      switch (opcode) {
        case 1:
          set(3, get(1) + get(2));
          this.ip += 4;
          break;
        case 2:
          set(3, get(1) * get(2));
          this.ip += 4;
          break;
        case 3:
          const input = inputProvider();
          set(1, input);
          this.ip += 2;
          break;
        case 4:
          this.output.push(get(1));
          this.ip += 2;
          if (this.output.length === 3) return this.output.splice(0, 3);
          break;
        case 5:
          this.ip = get(1) !== 0 ? get(2) : this.ip + 3;
          break;
        case 6:
          this.ip = get(1) === 0 ? get(2) : this.ip + 3;
          break;
        case 7:
          set(3, get(1) < get(2) ? 1 : 0);
          this.ip += 4;
          break;
        case 8:
          set(3, get(1) === get(2) ? 1 : 0);
          this.ip += 4;
          break;
        case 9:
          this.relativeBase += get(1);
          this.ip += 2;
          break;
        case 99:
          this.halted = true;
          return null;
        default:
          throw new Error(`Unknown opcode: ${opcode}`);
      }
    }
  }
}

const renderScreen = (tiles, width = 44, height = 24) => {
  const screen = Array.from({ length: height }, () => Array(width).fill(" "));
  const tileChars = [" ", "█", "▒", "━", "●"];
  for (const [key, tile] of tiles) {
    const [x, y] = key.split(",").map(Number);
    if (y < height && x < width) {
      screen[y][x] = tileChars[tile];
    }
  }
  return screen.map((row) => row.join("")).join("\n");
};

const main = async () => {
  const __filename = fileURLToPath(import.meta.url);
  const __dirname = path.dirname(__filename);
  const inputPath = path.join(__dirname, "input.txt");

  const raw = await fs.readFile(inputPath, "utf8");
  const program = raw.trim().split(",").map(Number);
  program[0] = 2;

  const computer = new IntcodeComputer(program);
  const tiles = new Map();
  let ballX = 0;
  let paddleX = 0;
  let score = 0;

  const inputProvider = () => Math.sign(ballX - paddleX);

  while (!computer.halted) {
    const out = computer.run(inputProvider);
    if (!out) break;
    const [x, y, tile] = out;

    if (x === -1 && y === 0) {
      score = tile;
    } else {
      tiles.set(`${x},${y}`, tile);
      if (tile === 3) paddleX = x;
      if (tile === 4) ballX = x;
    }
  }

  console.log("Final score:", score);
  console.log(renderScreen(tiles));
};

main();
