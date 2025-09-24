# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/18

from ...base import StrSplitSolution, answer
from collections import deque

DIRS = [(1, 0, 0), (-1, 0, 0), (0, 1, 0), (0, -1, 0), (0, 0, 1), (0, 0, -1)]


class Solution(StrSplitSolution):
    _year = 2022
    _day = 18

    def _parse(self):
        return {tuple(map(int, line.split(","))) for line in self.input}

    def surface_area(self, cubes):
        area = 0
        for x, y, z in cubes:
            for dx, dy, dz in DIRS:
                if (x + dx, y + dy, z + dz) not in cubes:
                    area += 1
        return area

    def exterior_surface_area(self, cubes):
        # Find bounds
        xs = [x for x, _, _ in cubes]
        ys = [y for _, y, _ in cubes]
        zs = [z for _, _, z in cubes]
        min_x, max_x = min(xs) - 1, max(xs) + 1
        min_y, max_y = min(ys) - 1, max(ys) + 1
        min_z, max_z = min(zs) - 1, max(zs) + 1
        # BFS from outside
        visited = set()
        q = deque()
        q.append((min_x, min_y, min_z))
        area = 0
        while q:
            x, y, z = q.popleft()
            if (x, y, z) in visited:
                continue
            visited.add((x, y, z))
            for dx, dy, dz in DIRS:
                nx, ny, nz = x + dx, y + dy, z + dz
                if (
                    min_x <= nx <= max_x
                    and min_y <= ny <= max_y
                    and min_z <= nz <= max_z
                ):
                    if (nx, ny, nz) in cubes:
                        area += 1
                    elif (nx, ny, nz) not in visited:
                        q.append((nx, ny, nz))
        return area

    @answer(3662)
    def part_1(self) -> int:
        return self.surface_area(self._parse())

    @answer(2060)
    def part_2(self) -> int:
        return self.exterior_surface_area(self._parse())

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
