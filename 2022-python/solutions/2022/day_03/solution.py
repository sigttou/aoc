# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/3

from ...base import StrSplitSolution, answer


def priority(c: str) -> int:
    if "a" <= c <= "z":
        return ord(c) - ord("a") + 1
    else:
        return ord(c) - ord("A") + 27


class Solution(StrSplitSolution):
    _year = 2022
    _day = 3

    @answer(8243)
    def part_1(self) -> int:
        total = 0
        for line in self.input:
            half = len(line) // 2
            left, right = set(line[:half]), set(line[half:])
            common = left & right
            total += sum(priority(c) for c in common)
        return total

    @answer(2631)
    def part_2(self) -> int:
        total = 0
        for i in range(0, len(self.input), 3):
            group = self.input[i : i + 3]
            common = set(group[0]) & set(group[1]) & set(group[2])
            total += sum(priority(c) for c in common)
        return total

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
