export default class IntcodeComputer {
  constructor(program) {
    this.memory = [...program];
    this.pointer = 0;
    this.inputs = [];
    this.outputs = [];
    this.relativeBase = 0;
    this.halted = false;
  }

  addInput(...values) {
    this.inputs.push(...values);
  }

  getOutput() {
    return this.outputs;
  }

  isHalted() {
    return this.halted;
  }

  run() {
    const get = (pos) => this.memory[pos] || 0;
    const set = (pos, val) => {
      this.memory[pos] = val;
    };

    const getParam = (mode, param) => {
      if (mode === 0) return get(param);
      if (mode === 1) return param;
      if (mode === 2) return get(this.relativeBase + param);
    };

    const getAddr = (mode, param) => {
      if (mode === 0) return param;
      if (mode === 2) return this.relativeBase + param;
    };

    while (true) {
      const instruction = get(this.pointer);
      const op = instruction % 100;
      const mode1 = Math.floor(instruction / 100) % 10;
      const mode2 = Math.floor(instruction / 1000) % 10;
      const mode3 = Math.floor(instruction / 10000) % 10;

      const param1 = get(this.pointer + 1);
      const param2 = get(this.pointer + 2);
      const param3 = get(this.pointer + 3);

      if (op === 1) {
        set(
          getAddr(mode3, param3),
          getParam(mode1, param1) + getParam(mode2, param2),
        );
        this.pointer += 4;
      } else if (op === 2) {
        set(
          getAddr(mode3, param3),
          getParam(mode1, param1) * getParam(mode2, param2),
        );
        this.pointer += 4;
      } else if (op === 3) {
        if (this.inputs.length === 0) return;
        set(getAddr(mode1, param1), this.inputs.shift());
        this.pointer += 2;
      } else if (op === 4) {
        this.outputs.push(getParam(mode1, param1));
        this.pointer += 2;
      } else if (op === 5) {
        if (getParam(mode1, param1) !== 0) {
          this.pointer = getParam(mode2, param2);
        } else {
          this.pointer += 3;
        }
      } else if (op === 6) {
        if (getParam(mode1, param1) === 0) {
          this.pointer = getParam(mode2, param2);
        } else {
          this.pointer += 3;
        }
      } else if (op === 7) {
        set(
          getAddr(mode3, param3),
          getParam(mode1, param1) < getParam(mode2, param2) ? 1 : 0,
        );
        this.pointer += 4;
      } else if (op === 8) {
        set(
          getAddr(mode3, param3),
          getParam(mode1, param1) === getParam(mode2, param2) ? 1 : 0,
        );
        this.pointer += 4;
      } else if (op === 9) {
        this.relativeBase += getParam(mode1, param1);
        this.pointer += 2;
      } else if (op === 99) {
        this.halted = true;
        return;
      } else {
        throw new Error(`Unknown opcode ${op} at position ${this.pointer}`);
      }
    }
  }
}
