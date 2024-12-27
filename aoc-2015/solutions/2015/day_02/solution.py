# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/2

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 2

    @answer((1598415, 3812909))
    def solve(self) -> tuple[int, int]:
        p1 = 0
        p2 = 0

        for line in self.input:
            l, w, h = map(int, line.split("x"))
            sides = [l * w, w * h, h * l]
            p2sides = [l, w, h]
            p2sides.sort()
            p2 += p2sides[0] * 2 + p2sides[1] * 2 + l * w * h
            sides.sort()
            p1 += sum(sides) * 2 + sides[0]

        return (p1, p2)
