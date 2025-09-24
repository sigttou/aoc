# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/20

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2022
    _day = 20

    def mix(self, numbers, times=1):
        # Keep track of original indices to handle duplicates
        mixed = list(enumerate(numbers))
        n = len(mixed)
        for _ in range(times):
            for idx, value in enumerate(numbers):
                # Find current position of this original index
                pos = next(
                    i for i, (orig_idx, _) in enumerate(mixed) if orig_idx == idx
                )
                item = mixed.pop(pos)
                new_pos = (pos + value) % (n - 1)
                mixed.insert(new_pos, item)
        return [v for _, v in mixed]

    def grove_coords(self, mixed):
        n = len(mixed)
        zero_idx = mixed.index(0)
        return sum(mixed[(zero_idx + offset) % n] for offset in [1000, 2000, 3000])

    @answer(4578)
    def part_1(self) -> int:
        numbers = [int(x) for x in self.input]
        mixed = self.mix(numbers, times=1)
        return self.grove_coords(mixed)

    @answer(2159638736133)
    def part_2(self) -> int:
        DECRYPTION_KEY = 811589153
        numbers = [int(x) * DECRYPTION_KEY for x in self.input]
        mixed = self.mix(numbers, times=10)
        return self.grove_coords(mixed)
