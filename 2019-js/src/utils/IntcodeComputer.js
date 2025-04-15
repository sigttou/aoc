export default class IntcodeComputer {
  constructor(program) {
    this.memory = [...program];
    this.pointer = 0;
    this.relativeBase = 0;
    this.input = [];
    this.output = [];
    this.halted = false;
  }

  getValue(mode, index) {
    const addr =
      mode === 0
        ? this.memory[index] || 0
        : mode === 1
        ? index
        : this.relativeBase + (this.memory[index] || 0);
    return this.memory[addr] || 0;
  }

  setValue(mode, index, value) {
    const addr =
      mode === 0
        ? this.memory[index] || 0
        : this.relativeBase + (this.memory[index] || 0);
    this.memory[addr] = value;
  }

  run(input = []) {
    this.input.push(...input);
    while (this.memory[this.pointer] !== 99) {
      const instr = String(this.memory[this.pointer]).padStart(5, "0");
      const opcode = Number(instr.slice(3));
      const modes = instr.slice(0, 3).split("").reverse().map(Number);

      const [m1, m2, m3] = modes;

      switch (opcode) {
        case 1: {
          const val1 = this.getValue(m1, this.pointer + 1);
          const val2 = this.getValue(m2, this.pointer + 2);
          this.setValue(m3, this.pointer + 3, val1 + val2);
          this.pointer += 4;
          break;
        }
        case 2: {
          const val1 = this.getValue(m1, this.pointer + 1);
          const val2 = this.getValue(m2, this.pointer + 2);
          this.setValue(m3, this.pointer + 3, val1 * val2);
          this.pointer += 4;
          break;
        }
        case 3: {
          if (this.input.length === 0) return;
          const value = this.input.shift();
          this.setValue(m1, this.pointer + 1, value);
          this.pointer += 2;
          break;
        }
        case 4: {
          const value = this.getValue(m1, this.pointer + 1);
          this.output.push(value);
          this.pointer += 2;
          break;
        }
        case 5: {
          const val1 = this.getValue(m1, this.pointer + 1);
          const val2 = this.getValue(m2, this.pointer + 2);
          this.pointer = val1 !== 0 ? val2 : this.pointer + 3;
          break;
        }
        case 6: {
          const val1 = this.getValue(m1, this.pointer + 1);
          const val2 = this.getValue(m2, this.pointer + 2);
          this.pointer = val1 === 0 ? val2 : this.pointer + 3;
          break;
        }
        case 7: {
          const val1 = this.getValue(m1, this.pointer + 1);
          const val2 = this.getValue(m2, this.pointer + 2);
          this.setValue(m3, this.pointer + 3, val1 < val2 ? 1 : 0);
          this.pointer += 4;
          break;
        }
        case 8: {
          const val1 = this.getValue(m1, this.pointer + 1);
          const val2 = this.getValue(m2, this.pointer + 2);
          this.setValue(m3, this.pointer + 3, val1 === val2 ? 1 : 0);
          this.pointer += 4;
          break;
        }
        case 9: {
          const val = this.getValue(m1, this.pointer + 1);
          this.relativeBase += val;
          this.pointer += 2;
          break;
        }
        default:
          throw new Error(`Unknown opcode: ${opcode}`);
      }
    }

    this.halted = true;
    return this.output;
  }
}
