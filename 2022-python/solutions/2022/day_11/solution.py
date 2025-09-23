# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/11

from ...base import StrSplitSolution, answer
import re
from math import prod


class Solution(StrSplitSolution):
    separator = "\n\n"
    _year = 2022
    _day = 11

    def parse_monkeys(self):
        self.monkeys = []
        for block in self.input:
            lines = block.splitlines()
            items = list(map(int, re.findall(r"\d+", lines[1])))
            op = lines[2].split(" = ")[1]
            test_div = int(re.search(r"\d+", lines[3]).group())
            if_true = int(re.search(r"\d+", lines[4]).group())
            if_false = int(re.search(r"\d+", lines[5]).group())
            self.monkeys.append(
                {
                    "items": items,
                    "op": op,
                    "test_div": test_div,
                    "if_true": if_true,
                    "if_false": if_false,
                    "inspects": 0,
                }
            )

    def simulate(self, rounds, divide_by_3=True):
        # For part 2, keep worry manageable by modulo product of all divisors
        mod = prod(m["test_div"] for m in self.monkeys)
        for _ in range(rounds):
            for m in self.monkeys:
                new_items = []
                while m["items"]:
                    old = m["items"].pop(0)
                    m["inspects"] += 1
                    # Evaluate operation
                    new = eval(m["op"], {}, {"old": old})
                    if divide_by_3:
                        new //= 3
                    else:
                        new %= mod
                    if new % m["test_div"] == 0:
                        self.monkeys[m["if_true"]]["items"].append(new)
                    else:
                        self.monkeys[m["if_false"]]["items"].append(new)

    @answer(110220)
    def part_1(self) -> int:
        self.parse_monkeys()
        self.simulate(20, divide_by_3=True)
        counts = sorted((m["inspects"] for m in self.monkeys), reverse=True)
        return counts[0] * counts[1]

    @answer(19457438264)
    def part_2(self) -> int:
        self.parse_monkeys()
        self.simulate(10000, divide_by_3=False)
        counts = sorted((m["inspects"] for m in self.monkeys), reverse=True)
        return counts[0] * counts[1]

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
