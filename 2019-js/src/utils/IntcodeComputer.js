export default class IntcodeComputer {
  constructor(program, inputFn = () => 0, outputFn = () => {}) {
    this.memory = [...program];
    this.inputFn = inputFn;
    this.outputFn = outputFn;
    this.ip = 0;
    this.relativeBase = 0;
    this.halted = false;
  }

  get(addr) {
    return this.memory[addr] ?? 0;
  }

  set(addr, value) {
    this.memory[addr] = value;
  }

  getParam(mode, offset) {
    const val = this.get(this.ip + offset);
    if (mode === 0) return this.get(val);
    if (mode === 1) return val;
    if (mode === 2) return this.get(this.relativeBase + val);
    throw new Error(`Unknown mode ${mode}`);
  }

  getWriteAddr(mode, offset) {
    const val = this.get(this.ip + offset);
    if (mode === 0) return val;
    if (mode === 2) return this.relativeBase + val;
    throw new Error(`Unknown write mode ${mode}`);
  }

  run() {
    while (!this.halted) {
      const opcode = this.get(this.ip) % 100;
      const modes = Math.floor(this.get(this.ip) / 100)
        .toString()
        .padStart(3, "0")
        .split("")
        .reverse()
        .map(Number);

      switch (opcode) {
        case 1: {
          // add
          const a = this.getParam(modes[0], 1);
          const b = this.getParam(modes[1], 2);
          const addr = this.getWriteAddr(modes[2], 3);
          this.set(addr, a + b);
          this.ip += 4;
          break;
        }
        case 2: {
          // multiply
          const a = this.getParam(modes[0], 1);
          const b = this.getParam(modes[1], 2);
          const addr = this.getWriteAddr(modes[2], 3);
          this.set(addr, a * b);
          this.ip += 4;
          break;
        }
        case 3: {
          // input
          const addr = this.getWriteAddr(modes[0], 1);
          const input = this.inputFn();
          if (input === undefined || input === null) return; // pause execution
          this.set(addr, input);
          this.ip += 2;
          break;
        }
        case 4: {
          // output
          const value = this.getParam(modes[0], 1);
          this.outputFn(value);
          this.ip += 2;
          break;
        }
        case 5: {
          // jump-if-true
          const a = this.getParam(modes[0], 1);
          const b = this.getParam(modes[1], 2);
          this.ip = a !== 0 ? b : this.ip + 3;
          break;
        }
        case 6: {
          // jump-if-false
          const a = this.getParam(modes[0], 1);
          const b = this.getParam(modes[1], 2);
          this.ip = a === 0 ? b : this.ip + 3;
          break;
        }
        case 7: {
          // less than
          const a = this.getParam(modes[0], 1);
          const b = this.getParam(modes[1], 2);
          const addr = this.getWriteAddr(modes[2], 3);
          this.set(addr, a < b ? 1 : 0);
          this.ip += 4;
          break;
        }
        case 8: {
          // equals
          const a = this.getParam(modes[0], 1);
          const b = this.getParam(modes[1], 2);
          const addr = this.getWriteAddr(modes[2], 3);
          this.set(addr, a === b ? 1 : 0);
          this.ip += 4;
          break;
        }
        case 9: {
          // adjust relative base
          const a = this.getParam(modes[0], 1);
          this.relativeBase += a;
          this.ip += 2;
          break;
        }
        case 99: // halt
          this.halted = true;
          return;
        default:
          throw new Error(`Unknown opcode ${opcode} at ${this.ip}`);
      }
    }
  }
}
