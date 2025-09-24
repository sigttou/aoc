# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/15

from ...base import StrSplitSolution, answer
import re


class Solution(StrSplitSolution):
    _year = 2022
    _day = 15

    def _parse(self):
        sensors = []
        beacons = set()
        for line in self.input:
            sx, sy, bx, by = map(int, re.findall(r"-?\d+", line))
            sensors.append(((sx, sy), abs(sx - bx) + abs(sy - by)))
            beacons.add((bx, by))
        return sensors, beacons

    def covered_ranges(self, sensors, target_y):
        ranges = []
        for (sx, sy), dist in sensors:
            dy = abs(sy - target_y)
            if dy <= dist:
                dx = dist - dy
                ranges.append((sx - dx, sx + dx))
        # Merge overlapping ranges
        ranges.sort()
        merged = []
        for start, end in ranges:
            if not merged or start > merged[-1][1] + 1:
                merged.append([start, end])
            else:
                merged[-1][1] = max(merged[-1][1], end)
        return merged

    @answer(4737567)
    def part_1(self) -> int:
        sensors, beacons = self._parse()
        target_y = 2000000
        merged = self.covered_ranges(sensors, target_y)
        # Count covered positions, excluding beacons on that row
        total = sum(end - start + 1 for start, end in merged)
        beacons_on_row = {bx for bx, by in beacons if by == target_y}
        for bx in beacons_on_row:
            for start, end in merged:
                if start <= bx <= end:
                    total -= 1
                    break
        return total

    @answer(13267474686239)
    def part_2(self) -> int:
        sensors, _ = self._parse()
        # Search for a gap in coverage in y=0..4000000
        max_coord = 4000000
        for y in range(max_coord + 1):
            merged = self.covered_ranges(sensors, y)
            x = 0
            for start, end in merged:
                if x < start:
                    # Found a gap at (x, y)
                    return x * 4000000 + y
                x = max(x, end + 1)
                if x > max_coord:
                    break
            if x <= max_coord:
                return x * 4000000 + y
        return -1

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
