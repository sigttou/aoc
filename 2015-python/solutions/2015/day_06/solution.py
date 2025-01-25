# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/6

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 6

    @answer((543903, 14687245))
    def solve(self) -> tuple[int, int]:
        p1_grid = [[0 for _ in range(1000)] for _ in range(1000)]
        p2_grid = [[0 for _ in range(1000)] for _ in range(1000)]

        for line in self.input:
            instr, from_coord, _, to_coord = line.strip().rsplit(" ", 3)
            from_coord = [int(x) for x in from_coord.split(",")]
            to_coord = [int(x) for x in to_coord.split(",")]

            for x in range(from_coord[0], to_coord[0] + 1):
                for y in range(from_coord[1], to_coord[1] + 1):
                    if instr == "turn on":
                        p1_grid[x][y] = 1
                        p2_grid[x][y] += 1
                    elif instr == "turn off":
                        p1_grid[x][y] = 0
                        p2_grid[x][y] -= 1 if p2_grid[x][y] else 0
                    else:
                        p1_grid[x][y] = 0 if p1_grid[x][y] else 1
                        p2_grid[x][y] += 2

        return (sum([sum(x) for x in p1_grid]), sum([sum(x) for x in p2_grid]))
