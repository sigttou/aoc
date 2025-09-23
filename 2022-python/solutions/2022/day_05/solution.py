# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/5

from ...base import TextSolution, answer
import re
from copy import deepcopy


class Solution(TextSolution):
    _year = 2022
    _day = 5

    def _parse(self):
        # Split into stack drawing and moves
        stacks_str, moves_str = self.input.rstrip().split("\n\n")
        stacks_lines = stacks_str.splitlines()
        # Last line contains stack numbers
        stack_count = len(stacks_lines[-1].split())
        stacks = [[] for _ in range(stack_count)]
        # Parse stacks from bottom up (excluding the numbers line)
        for line in reversed(stacks_lines[:-1]):
            for i in range(stack_count):
                # Each crate is at 1 + 4*i
                idx = 1 + 4 * i
                if idx < len(line) and line[idx] != " ":
                    stacks[i].append(line[idx])
        # Parse moves
        moves = []
        for line in moves_str.splitlines():
            m = re.match(r"move (\d+) from (\d+) to (\d+)", line)
            if m:
                count, src, dst = map(int, m.groups())
                moves.append((count, src - 1, dst - 1))
        return stacks, moves

    @answer("CWMTGHBDW")
    def part_1(self) -> str:
        stacks, moves = self._parse()
        stacks = deepcopy(stacks)
        for count, src, dst in moves:
            for _ in range(count):
                if stacks[src]:
                    stacks[dst].append(stacks[src].pop())
        return "".join(stack[-1] for stack in stacks if stack)

    @answer("SSCGWJCRB")
    def part_2(self) -> str:
        stacks, moves = self._parse()
        stacks = deepcopy(stacks)
        for count, src, dst in moves:
            # Move multiple crates at once, preserving order
            if stacks[src]:
                crates = stacks[src][-count:]
                stacks[src] = stacks[src][:-count]
                stacks[dst].extend(crates)
        return "".join(stack[-1] for stack in stacks if stack)

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
