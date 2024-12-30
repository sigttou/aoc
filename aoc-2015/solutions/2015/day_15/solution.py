# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/15

import collections
import functools
import operator

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 15

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        self.ingredients = {}
        for line in self.input:
            cookie, content = line.split(":")
            self.ingredients[cookie] = {
                k: int(v) for e in content.split(",") for k, v in [e.strip().split(" ")]
            }

    def get_total(self, combo, part):
        total = collections.Counter()
        for cookie, cnt in combo.items():
            for k, v in self.ingredients[cookie].items():
                total[k] += cnt * v

        for k, v in total.items():
            if v < 0:
                total[k] = 0

        if part == 2 and total["calories"] != 500:
            total["calories"] = 0

        total["calories"] = 1 if total["calories"] else 0

        return functools.reduce(operator.mul, total.values())

    def partition(self, N, d):
        if d == 1:
            yield [N]
        else:
            yield from (
                [H] + T for H in range(N + 1) for T in self.partition(N - H, d - 1)
            )

    @answer((222870, 117936))
    def solve(self) -> tuple[int, int]:
        parts = {1: 0, 2: 0}
        for combo in (
            dict(zip(self.ingredients, combo)) for combo in self.partition(100, 4)
        ):
            for p, v in parts.items():
                total = self.get_total(combo, p)
                parts[p] = max(total, v)

        return (parts[1], parts[2])
