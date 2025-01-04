# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/19

import re
from copy import copy
from random import shuffle

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 19

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)

        self.replacements = []
        for e in self.input[:-2]:
            self.replacements.append(re.findall(r"(\S+) => (\S+)", e)[0])

        self.start = self.input[-1]

    @answer(509)
    def part_1(self) -> int:
        solution = set()
        for a, b in self.replacements:
            for k in range(len(self.start)):
                if self.start[k : k + len(a)] == a:
                    solution.add(self.start[:k] + b + self.start[k + len(a) :])
        return len(solution)

    @answer(195)
    def part_2(self) -> int:
        ret = 0

        chk = copy(self.start)
        idx = 0
        while idx < len(self.replacements):
            a, b = self.replacements[idx]
            idx += 1
            pos = chk.find(b)
            if pos >= 0:
                ret += 1
                chk = chk[:pos] + a + chk[pos + len(b) :]
                idx = 0
                shuffle(self.replacements)
                if chk == "e":
                    return ret
        return -1
