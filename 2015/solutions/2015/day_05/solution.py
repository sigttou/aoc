# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/5

import re

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 5

    @answer(236)
    def part_1(self) -> int:
        return len(
            [
                e
                for e in self.input
                if re.search(r"([aeiou].*){3,}", e)
                and re.search(r"(.)\1", e)
                and not re.search(r"ab|cd|pq|xy", e)
            ]
        )

    @answer(51)
    def part_2(self) -> int:
        return len(
            [
                e
                for e in self.input
                if re.search(r"(..).*\1", e) and re.search(r"(.).\1", e)
            ]
        )
