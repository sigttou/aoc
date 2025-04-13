export class IntcodeComputer {
  constructor(program) {
    this.memory = [...program];
    this.ip = 0;
    this.relativeBase = 0;
    this.input = [];
    this.output = [];
    this.halted = false;
  }

  provideInput(value) {
    this.input.push(value);
  }

  run(inputs = []) {
    this.input.push(...inputs);
    this.output = [];

    while (!this.halted) {
      const opcode = this.memory[this.ip] % 100;
      const modes = Math.floor(this.memory[this.ip] / 100)
        .toString()
        .padStart(3, "0")
        .split("")
        .reverse()
        .map(Number);

      const getParam = (offset) => {
        const mode = modes[offset - 1];
        const val = this.memory[this.ip + offset] || 0;
        if (mode === 0) return this.memory[val] || 0;
        if (mode === 1) return val;
        if (mode === 2) return this.memory[this.relativeBase + val] || 0;
      };

      const setParam = (offset, value) => {
        const mode = modes[offset - 1];
        const addr =
          mode === 2
            ? this.relativeBase + (this.memory[this.ip + offset] || 0)
            : this.memory[this.ip + offset];
        this.memory[addr] = value;
      };

      switch (opcode) {
        case 1: // add
          setParam(3, getParam(1) + getParam(2));
          this.ip += 4;
          break;
        case 2: // multiply
          setParam(3, getParam(1) * getParam(2));
          this.ip += 4;
          break;
        case 3: // input
          if (this.input.length === 0) return this.output;
          setParam(1, this.input.shift());
          this.ip += 2;
          break;
        case 4: // output
          this.output.push(getParam(1));
          this.ip += 2;
          break;
        case 5: // jump-if-true
          this.ip = getParam(1) !== 0 ? getParam(2) : this.ip + 3;
          break;
        case 6: // jump-if-false
          this.ip = getParam(1) === 0 ? getParam(2) : this.ip + 3;
          break;
        case 7: // less than
          setParam(3, getParam(1) < getParam(2) ? 1 : 0);
          this.ip += 4;
          break;
        case 8: // equals
          setParam(3, getParam(1) === getParam(2) ? 1 : 0);
          this.ip += 4;
          break;
        case 9: // adjust relative base
          this.relativeBase += getParam(1);
          this.ip += 2;
          break;
        case 99: // halt
          this.halted = true;
          return this.output;
        default:
          throw new Error(`Unknown opcode ${opcode} at position ${this.ip}`);
      }
    }

    return this.output;
  }
}
