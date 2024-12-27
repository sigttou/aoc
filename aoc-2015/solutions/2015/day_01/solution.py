# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/1

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 1

    @answer(74)
    def part_1(self) -> int:
        return self.input.count("(") - self.input.count(")")

    @answer(1795)
    def part_2(self) -> int:
        chk = 0
        for i, c in enumerate(self.input):
            if c == "(":
                chk += 1
            if c == ")":
                chk -= 1
            if chk == -1:
                return i + 1
        return -1
