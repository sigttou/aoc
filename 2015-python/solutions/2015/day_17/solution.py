# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/17

from itertools import combinations

from ...base import IntSplitSolution, answer


class Solution(IntSplitSolution):
    _year = 2015
    _day = 17

    @answer((1638, 17))
    def solve(self) -> tuple[int, int]:
        combos = [
            c
            for i in range(1, len(self.input) + 1)
            for c in combinations(self.input, i)
            if sum(c) == 150
        ]
        return (
            len(combos),
            len([c for c in combos if len(c) == len(min(combos, key=len))]),
        )
