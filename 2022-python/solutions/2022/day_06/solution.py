# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/6

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2022
    _day = 6

    def find_marker(self, size: int) -> int:
        for i in range(size, len(self.input) + 1):
            if len(set(self.input[i - size : i])) == size:
                return i
        return -1

    @answer(1109)
    def part_1(self) -> int:
        return self.find_marker(4)

    @answer(3965)
    def part_2(self) -> int:
        return self.find_marker(14)

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
