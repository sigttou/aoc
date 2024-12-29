# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/10

from itertools import groupby

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 10

    @answer((329356, 4666278))
    def solve(self) -> tuple[int, int]:
        p1 = ""
        p2 = self.input

        for i in range(50):
            p2 = "".join(str(len(list(v))) + k for k, v in groupby(p2))
            if i == 39:
                p1 = p2

        return (len(p1), len(p2))
