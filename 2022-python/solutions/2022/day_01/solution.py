# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/1

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2022
    _day = 1

    def _parse_elves(self):
        # Split input by double newlines, then sum each elf's calories
        return [
            sum(map(int, elf.splitlines()))
            for elf in self.input.strip().split("\n\n")
        ]

    @answer(69501)
    def part_1(self) -> int:
        elves = self._parse_elves()
        return max(elves)

    @answer(202346)
    def part_2(self) -> int:
        elves = self._parse_elves()
        return sum(sorted(elves, reverse=True)[:3])

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
