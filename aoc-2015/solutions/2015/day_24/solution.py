# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/24

from functools import reduce
from itertools import combinations
from operator import mul

from ...base import IntSplitSolution, answer


class Solution(IntSplitSolution):
    _year = 2015
    _day = 24

    def get_entanglements(self, num_groups):
        grp_size = sum(self.input) // num_groups
        for i in range(len(self.input)):
            entanglements = [
                reduce(mul, combo)
                for combo in combinations(self.input, i)
                if sum(combo) == grp_size
            ]
            if entanglements:
                return min(entanglements)
        return -1

    @answer((11266889531, 77387711))
    def solve(self) -> tuple[int, int]:
        return self.get_entanglements(3), self.get_entanglements(4)
