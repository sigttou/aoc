# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template
# puzzle prompt: https://adventofcode.com/2022/day/25

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2022
    _day = 25

    def snafu_to_decimal(self, snafu):
        """Convert a SNAFU number to decimal"""
        decimal = 0
        power = 0

        # Process from right to left
        for char in reversed(snafu):
            if char == "2":
                digit_value = 2
            elif char == "1":
                digit_value = 1
            elif char == "0":
                digit_value = 0
            elif char == "-":
                digit_value = -1
            elif char == "=":
                digit_value = -2

            decimal += digit_value * (5**power)
            power += 1

        return decimal

    def decimal_to_snafu(self, decimal):
        """Convert a decimal number to SNAFU"""
        if decimal == 0:
            return "0"

        snafu = ""

        while decimal > 0:
            remainder = decimal % 5
            decimal = decimal // 5

            if remainder == 0:
                snafu = "0" + snafu
            elif remainder == 1:
                snafu = "1" + snafu
            elif remainder == 2:
                snafu = "2" + snafu
            elif remainder == 3:
                # 3 in base 5 becomes 1= (1*5 + (-2))
                snafu = "=" + snafu
                decimal += 1  # Carry over
            elif remainder == 4:
                # 4 in base 5 becomes 1- (1*5 + (-1))
                snafu = "-" + snafu
                decimal += 1  # Carry over

        return snafu

    @answer("2-0-0=1-0=2====20=-2")
    def part_1(self) -> str:
        total_decimal = 0

        # Convert each SNAFU number to decimal and sum them
        for snafu_number in self.input:
            decimal_value = self.snafu_to_decimal(snafu_number)
            total_decimal += decimal_value

        # Convert the sum back to SNAFU
        return self.decimal_to_snafu(total_decimal)

    # Part 2 is typically just getting all 49 stars, so no implementation needed
    @answer("Merry Christmas!")
    def part_2(self) -> str:
        return "Merry Christmas!"
