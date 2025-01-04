# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/23

import re

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 23

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        self.instructions = list(
            re.findall(r"^(\w+) (\S+)(?:, (\S+))?$", self.input, re.M)
        )

    def run_code(self, registers):
        i = 0
        while i < len(self.instructions):
            instr, *args = self.instructions[i]
            if instr == "hlf":
                registers[args[0]] //= 2
            elif instr == "tpl":
                registers[args[0]] *= 3
            elif instr == "inc":
                registers[args[0]] += 1
            elif instr == "jmp":
                i += int(args[0]) - 1
            elif instr == "jie":
                if registers[args[0]] % 2 == 0:
                    i += int(args[1]) - 1
            elif instr == "jio" and registers[args[0]] == 1:
                i += int(args[1]) - 1
            i += 1
        return registers

    @answer(170)
    def part_1(self) -> int:
        return self.run_code({"a": 0, "b": 0})["b"]

    @answer(247)
    def part_2(self) -> int:
        return self.run_code({"a": 1, "b": 0})["b"]
