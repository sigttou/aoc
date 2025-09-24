# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template
# puzzle prompt: https://adventofcode.com/2022/day/21

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2022
    _day = 21

    def parse_monkeys(self):
        """Parse the monkey jobs into a dictionary"""
        monkeys = {}
        for line in self.input:
            name, job = line.split(": ")
            if job.isdigit() or (job.startswith("-") and job[1:].isdigit()):
                # It's a number
                monkeys[name] = int(job)
            else:
                # It's an operation
                parts = job.split()
                left, op, right = parts[0], parts[1], parts[2]
                monkeys[name] = (left, op, right)
        return monkeys

    def evaluate(self, name, monkeys):
        """Recursively evaluate a monkey's value"""
        job = monkeys[name]

        if isinstance(job, int):
            return job

        left, op, right = job
        left_val = self.evaluate(left, monkeys)
        right_val = self.evaluate(right, monkeys)

        if op == "+":
            return left_val + right_val
        elif op == "-":
            return left_val - right_val
        elif op == "*":
            return left_val * right_val
        elif op == "/":
            return left_val // right_val  # Integer division

    @answer(110181395003396)
    def part_1(self) -> int:
        monkeys = self.parse_monkeys()
        return self.evaluate("root", monkeys)

    def contains_humn(self, name, monkeys):
        """Check if a monkey's calculation depends on 'humn'"""
        if name == "humn":
            return True

        job = monkeys[name]
        if isinstance(job, int):
            return False

        left, op, right = job
        return self.contains_humn(left, monkeys) or self.contains_humn(right, monkeys)

    def solve_for_humn(self, name, target_value, monkeys):
        """Solve backwards to find what humn should be"""
        if name == "humn":
            return target_value

        job = monkeys[name]
        if isinstance(job, int):
            return job  # This shouldn't happen in the path to humn

        left, op, right = job

        # Determine which side contains humn
        left_has_humn = self.contains_humn(left, monkeys)

        if left_has_humn:
            # Left side has humn, evaluate right side
            right_val = self.evaluate(right, monkeys)

            # Solve for what left side should be
            if op == "+":
                # left + right_val = target_value
                # left = target_value - right_val
                left_target = target_value - right_val
            elif op == "-":
                # left - right_val = target_value
                # left = target_value + right_val
                left_target = target_value + right_val
            elif op == "*":
                # left * right_val = target_value
                # left = target_value / right_val
                left_target = target_value // right_val
            elif op == "/":
                # left / right_val = target_value
                # left = target_value * right_val
                left_target = target_value * right_val

            return self.solve_for_humn(left, left_target, monkeys)
        else:
            # Right side has humn, evaluate left side
            left_val = self.evaluate(left, monkeys)

            # Solve for what right side should be
            if op == "+":
                # left_val + right = target_value
                # right = target_value - left_val
                right_target = target_value - left_val
            elif op == "-":
                # left_val - right = target_value
                # right = left_val - target_value
                right_target = left_val - target_value
            elif op == "*":
                # left_val * right = target_value
                # right = target_value / left_val
                right_target = target_value // left_val
            elif op == "/":
                # left_val / right = target_value
                # right = left_val / target_value
                right_target = left_val // target_value

            return self.solve_for_humn(right, right_target, monkeys)

    @answer(3721298272959)
    def part_2(self) -> int:
        monkeys = self.parse_monkeys()

        # Get root's operands
        root_job = monkeys["root"]
        left, op, right = root_job

        # Determine which side contains humn
        left_has_humn = self.contains_humn(left, monkeys)

        if left_has_humn:
            # Left side has humn, so we need left side to equal right side's value
            target_value = self.evaluate(right, monkeys)
            return self.solve_for_humn(left, target_value, monkeys)
        else:
            # Right side has humn, so we need right side to equal left side's value
            target_value = self.evaluate(left, monkeys)
            return self.solve_for_humn(right, target_value, monkeys)
