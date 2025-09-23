# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/8

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2022
    _day = 8

    def _parse(self):
        return [[int(c) for c in line] for line in self.input]

    @answer(1708)
    def part_1(self) -> int:
        grid = self._parse()
        rows, cols = len(grid), len(grid[0])
        visible = 0
        for r in range(rows):
            for c in range(cols):
                h = grid[r][c]
                if r == 0 or c == 0 or r == rows - 1 or c == cols - 1:
                    visible += 1
                else:
                    left = all(grid[r][i] < h for i in range(c))
                    right = all(grid[r][i] < h for i in range(c + 1, cols))
                    up = all(grid[i][c] < h for i in range(r))
                    down = all(grid[i][c] < h for i in range(r + 1, rows))
                    if left or right or up or down:
                        visible += 1
        return visible

    @answer(504000)
    def part_2(self) -> int:
        grid = self._parse()
        rows, cols = len(grid), len(grid[0])
        best = 0
        for r in range(rows):
            for c in range(cols):
                h = grid[r][c]
                # Look left
                l = 0
                for i in range(c - 1, -1, -1):
                    l += 1
                    if grid[r][i] >= h:
                        break
                # Look right
                ri = 0
                for i in range(c + 1, cols):
                    ri += 1
                    if grid[r][i] >= h:
                        break
                # Look up
                u = 0
                for i in range(r - 1, -1, -1):
                    u += 1
                    if grid[i][c] >= h:
                        break
                # Look down
                d = 0
                for i in range(r + 1, rows):
                    d += 1
                    if grid[i][c] >= h:
                        break
                score = l * ri * u * d
                if score > best:
                    best = score
        return best

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
