# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template
# puzzle prompt: https://adventofcode.com/2022/day/23

from ...base import StrSplitSolution, answer
from collections import defaultdict, deque


class Solution(StrSplitSolution):
    _year = 2022
    _day = 23

    def parse_elves(self):
        """Parse the initial positions of elves"""
        elves = set()
        for row, line in enumerate(self.input):
            for col, char in enumerate(line):
                if char == "#":
                    elves.add((row, col))
        return elves

    def get_adjacent_positions(self, row, col):
        """Get all 8 adjacent positions"""
        return [
            (row - 1, col - 1),
            (row - 1, col),
            (row - 1, col + 1),  # N row
            (row, col - 1),
            (row, col + 1),  # Same row
            (row + 1, col - 1),
            (row + 1, col),
            (row + 1, col + 1),  # S row
        ]

    def get_direction_positions(self, row, col, direction):
        """Get the 3 positions to check for a given direction"""
        if direction == "N":
            return [(row - 1, col - 1), (row - 1, col), (row - 1, col + 1)]
        elif direction == "S":
            return [(row + 1, col - 1), (row + 1, col), (row + 1, col + 1)]
        elif direction == "W":
            return [(row - 1, col - 1), (row, col - 1), (row + 1, col - 1)]
        elif direction == "E":
            return [(row - 1, col + 1), (row, col + 1), (row + 1, col + 1)]

    def get_proposed_move(self, row, col, direction):
        """Get the proposed new position for a direction"""
        if direction == "N":
            return (row - 1, col)
        elif direction == "S":
            return (row + 1, col)
        elif direction == "W":
            return (row, col - 1)
        elif direction == "E":
            return (row, col + 1)

    def simulate_round(self, elves, directions):
        """Simulate one round of movement"""
        proposals = {}  # elf_pos -> proposed_pos
        proposal_counts = defaultdict(
            list
        )  # proposed_pos -> list of elves proposing it

        # First half: each elf considers where to move
        for elf_row, elf_col in elves:
            # Check if any adjacent position has an elf
            adjacent = self.get_adjacent_positions(elf_row, elf_col)
            if not any(pos in elves for pos in adjacent):
                # No adjacent elves, don't move
                continue

            # Try each direction in order
            for direction in directions:
                positions_to_check = self.get_direction_positions(
                    elf_row, elf_col, direction
                )
                if not any(pos in elves for pos in positions_to_check):
                    # This direction is clear, propose moving there
                    proposed_pos = self.get_proposed_move(elf_row, elf_col, direction)
                    proposals[(elf_row, elf_col)] = proposed_pos
                    proposal_counts[proposed_pos].append((elf_row, elf_col))
                    break

        # Second half: move elves if they're the only one proposing that position
        new_elves = set()
        moved_count = 0

        for elf_pos in elves:
            if elf_pos in proposals:
                proposed_pos = proposals[elf_pos]
                if len(proposal_counts[proposed_pos]) == 1:
                    # Only this elf proposed this position, move there
                    new_elves.add(proposed_pos)
                    moved_count += 1
                else:
                    # Multiple elves proposed this position, don't move
                    new_elves.add(elf_pos)
            else:
                # This elf didn't propose moving
                new_elves.add(elf_pos)

        return new_elves, moved_count

    def get_bounding_rectangle(self, elves):
        """Get the bounding rectangle dimensions"""
        min_row = min(pos[0] for pos in elves)
        max_row = max(pos[0] for pos in elves)
        min_col = min(pos[1] for pos in elves)
        max_col = max(pos[1] for pos in elves)
        return min_row, max_row, min_col, max_col

    def count_empty_tiles(self, elves):
        """Count empty tiles in the bounding rectangle"""
        min_row, max_row, min_col, max_col = self.get_bounding_rectangle(elves)
        total_tiles = (max_row - min_row + 1) * (max_col - min_col + 1)
        return total_tiles - len(elves)

    @answer(4075)
    def part_1(self) -> int:
        elves = self.parse_elves()
        directions = deque(["N", "S", "W", "E"])

        for round_num in range(10):
            elves, moved_count = self.simulate_round(elves, directions)
            # Rotate directions for next round
            directions.append(directions.popleft())

        return self.count_empty_tiles(elves)

    @answer(950)
    def part_2(self) -> int:
        elves = self.parse_elves()
        directions = deque(["N", "S", "W", "E"])

        round_num = 1
        while True:
            elves, moved_count = self.simulate_round(elves, directions)

            if moved_count == 0:
                # No elf moved, we found our answer
                return round_num

            # Rotate directions for next round
            directions.append(directions.popleft())
            round_num += 1
