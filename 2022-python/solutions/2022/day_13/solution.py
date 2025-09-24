# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/13

from ...base import TextSolution, answer
import functools


class Solution(TextSolution):
    _year = 2022
    _day = 13

    def compare(self, left, right):
        # Returns -1 if left < right, 1 if left > right, 0 if equal
        if isinstance(left, int) and isinstance(right, int):
            return (left > right) - (left < right)
        if isinstance(left, list) and isinstance(right, list):
            for l, r in zip(left, right):
                cmp = self.compare(l, r)
                if cmp != 0:
                    return cmp
            return (len(left) > len(right)) - (len(left) < len(right))
        if isinstance(left, int):
            return self.compare([left], right)
        if isinstance(right, int):
            return self.compare(left, [right])
        return 0

    def _parse(self):
        pairs = [
            tuple(map(eval, block.splitlines()))
            for block in self.input.strip().split("\n\n")
        ]
        return pairs

    @answer(5720)
    def part_1(self) -> int:
        pairs = self._parse()
        total = 0
        for idx, (left, right) in enumerate(pairs, 1):
            if self.compare(left, right) == -1:
                total += idx
        return total

    @answer(23504)
    def part_2(self) -> int:
        pairs = self._parse()
        packets = []
        for l, r in pairs:
            packets.append(l)
            packets.append(r)
        # Add divider packets
        div1 = [[2]]
        div2 = [[6]]
        packets.append(div1)
        packets.append(div2)
        packets.sort(key=functools.cmp_to_key(self.compare))
        i1 = packets.index(div1) + 1
        i2 = packets.index(div2) + 1
        return i1 * i2

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
