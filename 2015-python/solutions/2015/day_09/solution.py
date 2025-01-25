# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/9

from collections import defaultdict
from itertools import permutations

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 9

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        self.cities = defaultdict(dict)
        for line in self.input:
            a, _, b, _, dist = line.split()
            self.cities[a][b] = int(dist)
            self.cities[b][a] = int(dist)

    def get_distance(self, cities):
        return sum(self.cities[a][b] for a, b in zip(cities, cities[1:]))

    @answer((251, 898))
    def solve(self) -> tuple[int, int]:
        distances = [self.get_distance(x) for x in permutations(self.cities.keys())]
        return (min(distances), max(distances))
