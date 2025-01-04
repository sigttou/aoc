# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/20

import numpy as np

from ...base import IntSolution, answer


class Solution(IntSolution):
    _year = 2015
    _day = 20

    @answer((776160, 786240))
    def solve(self) -> tuple[int, int]:
        mx_houses = self.input // 10
        p1_houses = np.zeros(mx_houses + 1)
        p2_houses = np.zeros(mx_houses + 1)

        for i in range(1, mx_houses + 1):
            p1_houses[i:mx_houses:i] += 10 * i
            p2_houses[i : i * 50 : i] += 11 * i

        p1 = np.where(p1_houses >= self.input)[0][0]
        p2 = np.where(p2_houses >= self.input)[0][0]

        return int(p1), int(p2)
