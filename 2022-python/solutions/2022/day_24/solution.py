# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template
# puzzle prompt: https://adventofcode.com/2022/day/24

from ...base import StrSplitSolution, answer
from math import lcm
from heapq import heappush, heappop


class Solution(StrSplitSolution):
    _year = 2022
    _day = 24

    def __init__(self, **kwargs):
        super().__init__()
        self.dirs_move = {">": (0, 1), "<": (0, -1), "^": (-1, 0), "v": (1, 0)}
        self.dirs_neighbor = list(self.dirs_move.values()) + [(0, 0)]
        self.grid = None
        self.grids = []
        self.states = 0
        self.start = None
        self.end = None
        self.lrows = 0
        self.lcols = 0

    def setup(self):
        """Initialize grid and pre-compute all blizzard states"""
        if self.grid is not None:
            return  # Already initialized

        self.grid = [list(row) for row in self.input]
        self.lrows, self.lcols = len(self.grid), len(self.grid[0])
        self.start = (0, 1)
        self.end = (self.lrows - 1, self.lcols - 2)

        # Calculate how many states before blizzards repeat
        self.states = lcm(self.lrows - 2, self.lcols - 2)

        # Initial blizzard positions
        curr_blizz_grid = set(
            (r, c, self.grid[r][c])
            for c in range(self.lcols)
            for r in range(self.lrows)
            if self.grid[r][c] not in ["#", "."]
        )

        # Pre-compute all blizzard states
        self.grids = []
        for s in range(self.states + 1):
            curr_blizz_grid = self.next_blizz_grid(curr_blizz_grid)
            self.grids.append(set((t[0], t[1]) for t in curr_blizz_grid))

    def next_blizz_grid(self, curr_blizz_grid):
        """Calculate next blizzard positions with wrapping"""
        blizz = set()
        for r, c, direction in curr_blizz_grid:
            m = self.dirs_move[direction]
            r_new, c_new = r + m[0], c + m[1]

            # Handle wrapping
            r_new = 1 if r_new == self.lrows - 1 else r_new
            r_new = self.lrows - 2 if r_new == 0 else r_new
            c_new = 1 if c_new == self.lcols - 1 else c_new
            c_new = self.lcols - 2 if c_new == 0 else c_new

            blizz.add((r_new, c_new, direction))
        return blizz

    def get_neighbors(self, r, c, t):
        """Get valid neighboring positions at time t"""
        for dis_r, dis_c in self.dirs_neighbor:
            r1, c1 = r + dis_r, c + dis_c

            # Check if position is valid (inside valley or start/end)
            if (1 <= r1 < len(self.grid) - 1 and 1 <= c1 < len(self.grid[0]) - 1) or (
                r1,
                c1,
            ) in [self.start, self.end]:
                # Check if position is not occupied by blizzard at time t
                if (r1, c1) not in self.grids[t % self.states]:
                    yield (r1, c1, t)

    def get_steps(self, start, end, t_start):
        """Use Dijkstra to find shortest path from start to end beginning at t_start"""
        visited = set()
        prio_queue = []
        heappush(prio_queue, (t_start, (start[0], start[1])))

        while prio_queue:
            t, (r, c) = heappop(prio_queue)
            s = t % self.states

            if (r, c, s) not in visited:
                visited.add((r, c, s))

                if (r, c) == end:
                    return t - t_start

                for rnew, cnew, t_new in self.get_neighbors(r, c, t):
                    heappush(prio_queue, (t_new + 1, (rnew, cnew)))

        return -1  # No path found

    @answer(332)
    def part_1(self) -> int:
        self.setup()
        return self.get_steps(self.start, self.end, 0)

    @answer(942)
    def part_2(self) -> int:
        self.setup()

        # First trip: start to end
        t = self.get_steps(self.start, self.end, 0)

        # Second trip: end to start
        t += self.get_steps(self.end, self.start, t)

        # Third trip: start to end again
        t += self.get_steps(self.start, self.end, t)

        return t
