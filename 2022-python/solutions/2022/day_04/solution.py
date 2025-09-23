# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/4

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2022
    _day = 4

    def _parse(self):
        # Returns list of ((a1, a2), (b1, b2)) tuples
        pairs = []
        for line in self.input:
            left, right = line.split(",")
            a1, a2 = map(int, left.split("-"))
            b1, b2 = map(int, right.split("-"))
            pairs.append(((a1, a2), (b1, b2)))
        return pairs

    @answer(413)
    def part_1(self) -> int:
        # Count pairs where one range fully contains the other
        count = 0
        for (a1, a2), (b1, b2) in self._parse():
            if (a1 <= b1 and a2 >= b2) or (b1 <= a1 and b2 >= a2):
                count += 1
        return count  # Count pairs where one range fully contains the other

    @answer(806)
    def part_2(self) -> int:
        # Count pairs where ranges overlap at all
        count = 0
        for (a1, a2), (b1, b2) in self._parse():
            if a2 >= b1 and b2 >= a1:
                count += 1
        return count

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
