# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/18

from copy import deepcopy

from ...base import StrSplitSolution, answer
from ...utils.grid import neighbors_all


class Solution(StrSplitSolution):
    _year = 2015
    _day = 18

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        self.grid = [list(line) for line in self.input]

    @answer(821)
    def part_1(self) -> int:
        grid = deepcopy(self.grid)

        for _ in range(100):
            tmp_grid = deepcopy(grid)
            for y, line in enumerate(grid):
                for x, e in enumerate(line):
                    cnt = 0
                    for nx, ny in neighbors_all(x, y):
                        if nx < 0 or ny < 0 or nx >= len(line) or ny >= len(grid):
                            continue

                        cnt += 1 if tmp_grid[ny][nx] == "#" else 0

                    if e == "#" and cnt not in [2, 3]:
                        grid[y][x] = "."
                    if e == "." and cnt == 3:
                        grid[y][x] = "#"

        return sum([line.count("#") for line in grid])

    @answer(886)
    def part_2(self) -> int:
        grid = deepcopy(self.grid)
        corners = [(0, 0), (0, 99), (99, 0), (99, 99)]

        for x, y in corners:
            grid[y][x] = "#"

        for _ in range(100):
            tmp_grid = deepcopy(grid)
            for y, line in enumerate(grid):
                for x, e in enumerate(line):
                    if (x, y) in corners:
                        continue
                    cnt = 0
                    for nx, ny in neighbors_all(x, y):
                        if nx < 0 or ny < 0 or nx >= len(line) or ny >= len(grid):
                            continue

                        cnt += 1 if tmp_grid[ny][nx] == "#" else 0

                    if e == "#" and cnt not in [2, 3]:
                        grid[y][x] = "."
                    if e == "." and cnt == 3:
                        grid[y][x] = "#"

        return sum([line.count("#") for line in grid])
