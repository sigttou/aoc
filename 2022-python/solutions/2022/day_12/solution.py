# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/12

from ...base import StrSplitSolution, answer
from collections import deque


class Solution(StrSplitSolution):
    _year = 2022
    _day = 12

    def parse_grid(self):
        grid = []
        start = end = None
        for r, line in enumerate(self.input):
            row = []
            for c, ch in enumerate(line):
                if ch == "S":
                    start = (r, c)
                    row.append(0)
                elif ch == "E":
                    end = (r, c)
                    row.append(25)
                else:
                    row.append(ord(ch) - ord("a"))
            grid.append(row)
        return grid, start, end

    def bfs(self, grid, starts, end):
        rows, cols = len(grid), len(grid[0])
        visited = [[False] * cols for _ in range(rows)]
        q = deque()
        for s in starts:
            q.append((s[0], s[1], 0))
            visited[s[0]][s[1]] = True
        while q:
            r, c, steps = q.popleft()
            if (r, c) == end:
                return steps
            for dr, dc in [(-1, 0), (1, 0), (0, -1), (0, 1)]:
                nr, nc = r + dr, c + dc
                if 0 <= nr < rows and 0 <= nc < cols and not visited[nr][nc]:
                    if grid[nr][nc] <= grid[r][c] + 1:
                        visited[nr][nc] = True
                        q.append((nr, nc, steps + 1))
        return -1

    @answer(520)
    def part_1(self) -> int:
        grid, start, end = self.parse_grid()
        return self.bfs(grid, [start], end)

    @answer(508)
    def part_2(self) -> int:
        grid, _, end = self.parse_grid()
        starts = [
            (r, c)
            for r in range(len(grid))
            for c in range(len(grid[0]))
            if grid[r][c] == 0
        ]
        return self.bfs(grid, starts, end)

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
