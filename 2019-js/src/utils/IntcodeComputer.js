// utils/IntcodeComputer.js

export default class IntcodeComputer {
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
          return this.output.pop();
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
