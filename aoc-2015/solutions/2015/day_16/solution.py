# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/16

import re
from operator import eq, gt, lt

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 16

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        ticket_str = """children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1
"""
        self.ticket = self.to_dict(ticket_str)

    def to_dict(self, string):
        return eval("{" + re.sub(r"(\w+): (\d+),?", r'"\1":\2,', string) + "}")

    @answer((103, 405))
    def solve(self) -> tuple[int, int]:
        p1 = 0
        p2 = 0

        for line in self.input:
            aunt, info = line.split(": ", 1)
            info = self.to_dict(info)
            if not p1 and all(info[k] == self.ticket[k] for k in info):
                p1 = int(aunt.split(" ")[1])

            p2_checks = {"cats": gt, "trees": gt, "pomeranians": lt, "goldfish": lt}
            if all(p2_checks.get(k, eq)(info[k], self.ticket[k]) for k in info):
                p2 = int(aunt.split(" ")[1])
                break

        return (p1, p2)
