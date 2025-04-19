// src/lib/intcode.js
export default class IntcodeComputer {
  constructor(program) {
    this.memory = [...program];
    this.pointer = 0n;
    this.relativeBase = 0n;
    this.inputs = [];
    this.outputs = [];
    this.halted = false;
    this.waitingForInput = false;
  }

  cloneMemory() {
    const mem = {};
    for (let i = 0; i < this.memory.length; i++) {
      mem[BigInt(i)] = BigInt(this.memory[i]);
    }
    return mem;
  }

  getValue(address) {
    return this.memory[address] ?? 0n;
  }

  setValue(address, value) {
    this.memory[address] = value;
  }

  input(value) {
    this.inputs.push(BigInt(value));
    this.waitingForInput = false;
  }

  output() {
    return this.outputs.shift();
  }

  hasOutput() {
    return this.outputs.length > 0;
  }

  isHalted() {
    return this.halted;
  }

  runUntilOutput(limit = Infinity) {
    let outputCount = 0;

    while (!this.halted && outputCount < limit) {
      const instruction = this.getValue(this.pointer)
        .toString()
        .padStart(5, "0");
      const opcode = Number(instruction.slice(-2));
      const modes = instruction.slice(0, 3).split("").reverse().map(Number);

      const getParam = (i) => {
        const val = this.getValue(this.pointer + BigInt(i));
        if (modes[i - 1] === 0) return this.getValue(val); // position
        if (modes[i - 1] === 1) return val; // immediate
        if (modes[i - 1] === 2) return this.getValue(this.relativeBase + val); // relative
      };

      const getWriteAddress = (i) => {
        const val = this.getValue(this.pointer + BigInt(i));
        if (modes[i - 1] === 2) return this.relativeBase + val;
        return val;
      };

      switch (opcode) {
        case 1: {
          // add
          const a = getParam(1);
          const b = getParam(2);
          this.setValue(getWriteAddress(3), a + b);
          this.pointer += 4n;
          break;
        }
        case 2: {
          // multiply
          const a = getParam(1);
          const b = getParam(2);
          this.setValue(getWriteAddress(3), a * b);
          this.pointer += 4n;
          break;
        }
        case 3: {
          // input
          if (this.inputs.length === 0) {
            this.waitingForInput = true;
            return;
          }
          this.setValue(getWriteAddress(1), this.inputs.shift());
          this.pointer += 2n;
          break;
        }
        case 4: {
          // output
          this.outputs.push(getParam(1));
          this.pointer += 2n;
          outputCount++;
          break;
        }
        case 5: {
          // jump-if-true
          const a = getParam(1);
          const b = getParam(2);
          this.pointer = a !== 0n ? b : this.pointer + 3n;
          break;
        }
        case 6: {
          // jump-if-false
          const a = getParam(1);
          const b = getParam(2);
          this.pointer = a === 0n ? b : this.pointer + 3n;
          break;
        }
        case 7: {
          // less than
          const a = getParam(1);
          const b = getParam(2);
          this.setValue(getWriteAddress(3), a < b ? 1n : 0n);
          this.pointer += 4n;
          break;
        }
        case 8: {
          // equals
          const a = getParam(1);
          const b = getParam(2);
          this.setValue(getWriteAddress(3), a === b ? 1n : 0n);
          this.pointer += 4n;
          break;
        }
        case 9: {
          // adjust relative base
          this.relativeBase += getParam(1);
          this.pointer += 2n;
          break;
        }
        case 99: // halt
          this.halted = true;
          return;
        default:
          throw new Error(`Unknown opcode: ${opcode} at ${this.pointer}`);
      }
    }
  }
}
