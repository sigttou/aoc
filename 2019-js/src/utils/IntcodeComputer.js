// utils/IntcodeComputer.js

export default class IntcodeComputer {
  constructor(program) {
    this.memory = [...program];
    this.ip = 0; // instruction pointer
    this.relativeBase = 0;
    this.inputs = [];
    this.outputs = [];
    this.halted = false;
  }

  provideInput(value) {
    this.inputs.push(value);
  }

  getValue(mode, index) {
    const addr = this.getAddress(mode, index);
    return this.memory[addr] ?? 0;
  }

  setValue(mode, index, value) {
    const addr = this.getAddress(mode, index);
    this.memory[addr] = value;
  }

  getAddress(mode, index) {
    const val = this.memory[this.ip + index] ?? 0;
    if (mode === 0) return val;
    if (mode === 1) return this.ip + index;
    if (mode === 2) return this.relativeBase + val;
    throw new Error(`Unknown address mode: ${mode}`);
  }

  runUntilOutput() {
    while (this.memory[this.ip] !== 99) {
      const instruction = this.memory[this.ip];
      const [opcode, modes] = [
        instruction % 100,
        [
          Math.floor(instruction / 100) % 10,
          Math.floor(instruction / 1000) % 10,
          Math.floor(instruction / 10000) % 10,
        ],
      ];

      switch (opcode) {
        case 1: {
          const val = this.getValue(modes[0], 1) + this.getValue(modes[1], 2);
          this.setValue(modes[2], 3, val);
          this.ip += 4;
          break;
        }
        case 2: {
          const val = this.getValue(modes[0], 1) * this.getValue(modes[1], 2);
          this.setValue(modes[2], 3, val);
          this.ip += 4;
          break;
        }
        case 3: {
          if (this.inputs.length === 0) return undefined;
          const input = this.inputs.shift();
          this.setValue(modes[0], 1, input);
          this.ip += 2;
          break;
        }
        case 4: {
          const output = this.getValue(modes[0], 1);
          this.ip += 2;
          return output;
        }
        case 5: {
          if (this.getValue(modes[0], 1) !== 0) {
            this.ip = this.getValue(modes[1], 2);
          } else {
            this.ip += 3;
          }
          break;
        }
        case 6: {
          if (this.getValue(modes[0], 1) === 0) {
            this.ip = this.getValue(modes[1], 2);
          } else {
            this.ip += 3;
          }
          break;
        }
        case 7: {
          const val =
            this.getValue(modes[0], 1) < this.getValue(modes[1], 2) ? 1 : 0;
          this.setValue(modes[2], 3, val);
          this.ip += 4;
          break;
        }
        case 8: {
          const val =
            this.getValue(modes[0], 1) === this.getValue(modes[1], 2) ? 1 : 0;
          this.setValue(modes[2], 3, val);
          this.ip += 4;
          break;
        }
        case 9: {
          this.relativeBase += this.getValue(modes[0], 1);
          this.ip += 2;
          break;
        }
        default:
          throw new Error(`Unknown opcode: ${opcode}`);
      }
    }

    this.halted = true;
    return undefined;
  }
}
