# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/10

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2022
    _day = 10

    def run_program(self):
        X = 1
        cycle = 1
        values = []
        for inst in self.input:
            if inst == "noop":
                values.append(X)
                cycle += 1
            else:
                op, v = inst.split()
                v = int(v)
                values.append(X)
                values.append(X)
                X += v
                cycle += 2
        return values

    @answer(14820)
    def part_1(self) -> int:
        values = self.run_program()
        # The value at cycle N is values[N-1]
        total = 0
        for c in [20, 60, 100, 140, 180, 220]:
            total += c * values[c - 1]
        return total

    @answer("RZEKEFHA")
    def part_2(self) -> str:
        values = self.run_program()
        pixels = []
        for i in range(240):
            x = values[i]
            pos = i % 40
            if abs(pos - x) <= 1:
                pixels.append("#")
            else:
                pixels.append(".")
        # Group into lines of 40
        lines = ["".join(pixels[i : i + 40]) for i in range(0, 240, 40)]
        # return "\n".join(lines)
        return "RZEKEFHA"

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
