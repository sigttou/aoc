# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/7

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 7

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)
        self.results = {}
        self.code = {}
        for line in self.input:
            (ops, ret) = line.split("->")
            self.code[ret.strip()] = ops.strip().split(" ")

    def exec(self, val):
        if val.isnumeric():
            return int(val)

        if self.results.get(val):
            return self.results[val]

        ops = self.code[val]
        if len(ops) == 1:
            res = self.exec(ops[0])
        else:
            op = ops[-2]
            if op == "AND":
                res = self.exec(ops[0]) & self.exec(ops[2])
            elif op == "OR":
                res = self.exec(ops[0]) | self.exec(ops[2])
            elif op == "NOT":
                res = ~self.exec(ops[1]) & 0xFFFF
            elif op == "RSHIFT":
                res = self.exec(ops[0]) >> self.exec(ops[2])
            elif op == "LSHIFT":
                res = self.exec(ops[0]) << self.exec(ops[2])
            else:
                res = -1  # not possible

        self.results[val] = res
        return self.results[val]

    @answer(956)
    def part_1(self) -> int:
        return self.exec("a")

    @answer(40149)
    def part_2(self) -> int:
        self.code["b"] = [str(self.part_1())]
        self.results = {}
        return self.exec("a")
