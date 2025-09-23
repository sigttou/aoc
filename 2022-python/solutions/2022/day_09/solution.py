# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/9

from ...base import StrSplitSolution, answer


DIRS = {
    "U": (0, 1),
    "D": (0, -1),
    "L": (-1, 0),
    "R": (1, 0),
}


def sign(x):
    return (x > 0) - (x < 0)


class Solution(StrSplitSolution):
    _year = 2022
    _day = 9

    def simulate(self, knots=2):
        rope = [(0, 0) for _ in range(knots)]
        visited = set()
        visited.add(rope[-1])
        for dir, steps in self.moves:
            dx, dy = DIRS[dir]
            for _ in range(steps):
                # Move head
                hx, hy = rope[0]
                rope[0] = (hx + dx, hy + dy)
                # Move each knot
                for i in range(1, knots):
                    px, py = rope[i - 1]
                    cx, cy = rope[i]
                    if abs(px - cx) > 1 or abs(py - cy) > 1:
                        cx += sign(px - cx)
                        cy += sign(py - cy)
                        rope[i] = (cx, cy)
                visited.add(rope[-1])
        return len(visited)

    def _parse(self):
        self.moves = []
        for line in self.input:
            d, n = line.split()
            self.moves.append((d, int(n)))

    @answer(6563)
    def part_1(self) -> int:
        self._parse()
        return self.simulate(knots=2)

    @answer(2653)
    def part_2(self) -> int:
        self._parse()
        return self.simulate(knots=10)

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
