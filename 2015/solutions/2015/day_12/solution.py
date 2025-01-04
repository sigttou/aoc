# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/12


import json
import re

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 12

    def solution(self, x):
        return sum(map(int, re.findall(r"-?\d+", x)))

    @answer(119433)
    def part_1(self) -> int:
        return self.solution(self.input)

    @answer(68466)
    def part_2(self) -> int:
        def hook(obj):
            if "red" in obj.values():
                return {}
            return obj

        return self.solution(str(json.loads(self.input, object_hook=hook)))
