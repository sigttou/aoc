# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/7

from ...base import StrSplitSolution, answer


class Dir:
    def __init__(self, name, parent=None):
        self.name = name
        self.parent = parent
        self.dirs = {}
        self.files = {}

    def size(self):
        return sum(self.files.values()) + sum(d.size() for d in self.dirs.values())

    def all_dirs(self):
        yield self
        for d in self.dirs.values():
            yield from d.all_dirs()


class Solution(StrSplitSolution):
    _year = 2022
    _day = 7

    def build_fs(self):
        root = Dir("/")
        cwd = root
        for line in self.input:
            if line.startswith("$ cd"):
                arg = line[5:]
                if arg == "/":
                    cwd = root
                elif arg == "..":
                    cwd = cwd.parent
                else:
                    cwd = cwd.dirs[arg]
            elif line.startswith("$ ls"):
                continue
            elif line.startswith("dir "):
                name = line[4:]
                if name not in cwd.dirs:
                    cwd.dirs[name] = Dir(name, cwd)
            else:
                size, name = line.split()
                cwd.files[name] = int(size)
        return root

    @answer(1141028)
    def part_1(self) -> int:
        root = self.build_fs()
        return sum(d.size() for d in root.all_dirs() if d.size() <= 100000)

    @answer(8278005)
    def part_2(self) -> int:
        root = self.build_fs()
        total = root.size()
        need_to_free = 30000000 - (70000000 - total)
        return min(d.size() for d in root.all_dirs() if d.size() >= need_to_free)

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
