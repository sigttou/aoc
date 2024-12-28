# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/3

from ...base import TextSolution, answer

DIR = {">": (1, 0), "<": (-1, 0), "v": (0, 1), "^": (0, -1)}


class Solution(TextSolution):
    _year = 2015
    _day = 3

    @answer(2572)
    def part_1(self) -> int:
        pos = (0, 0)
        visited = set()
        visited.add(pos)

        for c in self.input:
            pos = (pos[0] + DIR[c][0], pos[1] + DIR[c][1])
            visited.add(pos)

        return len(visited)

    @answer(2631)
    def part_2(self) -> int:
        santa_pos = (0, 0)
        robot_pos = (0, 0)
        visited = set()
        visited.add(santa_pos)

        it = iter(self.input)
        for s, r in zip(it, it):
            santa_pos = (santa_pos[0] + DIR[s][0], santa_pos[1] + DIR[s][1])
            robot_pos = (robot_pos[0] + DIR[r][0], robot_pos[1] + DIR[r][1])
            visited.add(santa_pos)
            visited.add(robot_pos)

        return len(visited)
