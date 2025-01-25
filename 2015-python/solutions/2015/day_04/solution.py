# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/4

import itertools
from hashlib import md5

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 4

    @answer((254575, 1038736))
    def solve(self) -> tuple[int, int]:
        p1 = 0
        p2 = 0

        for i in itertools.count():
            if (
                md5((self.input + str(i)).encode("utf-8"))
                .hexdigest()
                .startswith("0" * 5)
                and p1 == 0
            ):
                p1 = i
            if (
                md5((self.input + str(i)).encode("utf-8"))
                .hexdigest()
                .startswith("0" * 6)
            ):
                p2 = i
                break

        return (p1, p2)
