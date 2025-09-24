# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/17

from ...base import TextSolution, answer

ROCKS = [
    [(0, 0), (1, 0), (2, 0), (3, 0)],  # -
    [(1, 0), (0, 1), (1, 1), (2, 1), (1, 2)],  # +
    [(0, 0), (1, 0), (2, 0), (2, 1), (2, 2)],  # mirrored L
    [(0, 0), (0, 1), (0, 2), (0, 3)],  # |
    [(0, 0), (1, 0), (0, 1), (1, 1)],  # square
]


class Solution(TextSolution):
    _year = 2022
    _day = 17

    def simulate(self, num_rocks):
        chamber = set((x, 0) for x in range(7))  # Floor
        jet_len = len(self.input)
        jet_idx = 0
        max_y = 0
        states = {}
        added_height = 0
        rock_idx = 0

        while rock_idx < num_rocks:
            shape = ROCKS[rock_idx % len(ROCKS)]
            x, y = 2, max_y + 4
            rock = [(x + dx, y + dy) for dx, dy in shape]
            while True:
                # Jet push
                dx = -1 if self.input[jet_idx % jet_len] == "<" else 1
                jet_idx += 1
                pushed = [(rx + dx, ry) for rx, ry in rock]
                if all(0 <= px < 7 and (px, py) not in chamber for px, py in pushed):
                    rock = pushed
                # Fall
                fallen = [(rx, ry - 1) for rx, ry in rock]
                if all((fx, fy) not in chamber for fx, fy in fallen):
                    rock = fallen
                else:
                    for rx, ry in rock:
                        chamber.add((rx, ry))
                        if ry > max_y:
                            max_y = ry
                    break
            # Cycle detection for part 2
            if num_rocks > 2022:
                # Use top 30 rows as a "signature"
                top = []
                for dy in range(30):
                    row = tuple((x, max_y - dy) in chamber for x in range(7))
                    top.append(row)
                key = (rock_idx % len(ROCKS), jet_idx % jet_len, tuple(top))
                if key in states:
                    prev_rock, prev_y = states[key]
                    cycle_len = rock_idx - prev_rock
                    cycle_height = max_y - prev_y
                    cycles = (num_rocks - rock_idx) // cycle_len
                    added_height += cycles * cycle_height
                    rock_idx += cycles * cycle_len
                    states.clear()  # Only apply cycle once
                else:
                    states[key] = (rock_idx, max_y)
            rock_idx += 1
        return max_y + added_height

    @answer((3133, 1547953216393))
    def solve(self) -> tuple[int, int]:
        return (self.simulate(2022), self.simulate(1000000000000))
