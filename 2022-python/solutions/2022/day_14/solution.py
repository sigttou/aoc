# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/14

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2022
    _day = 14

    def _parse(self):
        rocks = set()
        max_y = 0
        for line in self.input:
            points = [tuple(map(int, p.split(","))) for p in line.split(" -> ")]
            for (x1, y1), (x2, y2) in zip(points, points[1:]):
                if x1 == x2:
                    for y in range(min(y1, y2), max(y1, y2) + 1):
                        rocks.add((x1, y))
                        max_y = max(max_y, y)
                else:
                    for x in range(min(x1, x2), max(x1, x2) + 1):
                        rocks.add((x, y1))
                        max_y = max(max_y, y1)
        return rocks, max_y

    def simulate(self, rocks, max_y, part2=False):
        sand = set()
        source = (500, 0)
        floor = max_y + 2 if part2 else None
        while True:
            x, y = source
            while True:
                if not part2 and y > max_y:
                    return len(sand)
                if part2 and y + 1 == floor:
                    # Sand comes to rest on the floor
                    sand.add((x, y))
                    break
                if (
                    (x, y + 1) not in rocks
                    and (x, y + 1) not in sand
                    and (not part2 or y + 1 < floor)
                ):
                    y += 1
                elif (
                    (x - 1, y + 1) not in rocks
                    and (x - 1, y + 1) not in sand
                    and (not part2 or y + 1 < floor)
                ):
                    x -= 1
                    y += 1
                elif (
                    (x + 1, y + 1) not in rocks
                    and (x + 1, y + 1) not in sand
                    and (not part2 or y + 1 < floor)
                ):
                    x += 1
                    y += 1
                else:
                    sand.add((x, y))
                    if part2 and (x, y) == source:
                        return len(sand)
                    break

    @answer(683)
    def part_1(self) -> int:
        rocks, max_y = self._parse()
        return self.simulate(rocks, max_y, part2=False)

    @answer(28821)
    def part_2(self) -> int:
        rocks, max_y = self._parse()
        return self.simulate(rocks, max_y, part2=True)

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
