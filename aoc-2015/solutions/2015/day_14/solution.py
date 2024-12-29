# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/14

import re
from collections import Counter
from itertools import accumulate

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 14

    @answer((2640, 1102))
    def solve(self) -> tuple[int, int]:
        p1, p2 = 0, 0

        race = {}
        for deer, speed, duration, rest in re.findall(
            r"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.",
            self.input,
        ):
            steps = [int(speed)] * int(duration) + [0] * int(rest)
            race[deer] = list(accumulate(steps[i % len(steps)] for i in range(2503)))

        p1 = max(e[-1] for e in race.values())
        p2 = max(
            Counter(
                [i for a in zip(*race.values()) for i, v in enumerate(a) if v == max(a)]
            ).values()
        )

        return (p1, p2)
