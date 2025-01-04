# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/8

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 8

    @answer(1371)
    def part_1(self) -> int:
        return sum(len(line) - len(eval(line)) for line in self.input)

    @answer(2117)
    def part_2(self) -> int:
        return sum(2 + line.count("\\") + line.count('"') for line in self.input)
