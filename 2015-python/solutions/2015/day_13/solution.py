# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/13

import re
from collections import defaultdict
from itertools import permutations

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 13

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        self.mappings = defaultdict(dict)
        for a, action, n, b in re.findall(
            r"(\w+) .* (lose|gain) (\d+) .* (\w+)\.", self.input
        ):
            self.mappings[a][b] = (1 if action == "gain" else -1) * int(n)

    def solution(self):
        def happiness(combo):
            return sum(
                self.mappings[a][b] + self.mappings[b][a]
                for (a, b) in zip(combo, combo[1:] + combo[:1])
            )

        return max(happiness(x) for x in permutations(self.mappings))

    @answer(618)
    def part_1(self) -> int:
        return self.solution()

    @answer(601)
    def part_2(self) -> int:
        for k in list(self.mappings.keys()):
            self.mappings[k]["me"] = 0
            self.mappings["me"][k] = 0
        return self.solution()
