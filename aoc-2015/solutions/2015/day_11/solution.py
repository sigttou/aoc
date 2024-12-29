# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/11

import re

from ...base import TextSolution, answer


class Solution(TextSolution):
    _year = 2015
    _day = 11

    def new_password(self, password):
        while True:
            password = re.sub(
                r"([a-y])(z*)$",
                lambda x: chr(ord(x.group(1)) + 1) + len(x.group(2)) * "a",
                password,
            )
            if (
                ("i" in password or "o" in password or "l" in password)
                or (len(re.findall(r"([a-z])\1", password)) < 2)
                or (
                    len(
                        [
                            1
                            for x, y, z in zip(password, password[1:], password[2:])
                            if ord(z) - ord(y) == 1 and ord(y) - ord(x) == 1
                        ]
                    )
                    == 0
                )
            ):
                continue

            return password

    @answer(("hxbxxyzz", "hxcaabcc"))
    def solve(self) -> tuple[str, str]:
        p1 = self.new_password(self.input)
        p2 = self.new_password(p1)
        return (p1, p2)
