# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/25

import re

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 25

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        self.row, self.column = map(int, re.findall(r"\d+", self.input))

    @answer(19980801)
    def part_1(self) -> int:
        r = 20151125
        s = sum(range(self.column + 1))

        for e in range(self.row - 1):
            s += self.column + e

        for e in range(1, s):
            r = (r * 252533) % 33554393

        return r

    @answer("Click to get the star!")
    def part_2(self) -> str:
        return "Click to get the star!"
