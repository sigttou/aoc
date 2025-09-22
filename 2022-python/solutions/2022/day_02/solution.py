# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/2

from ...base import StrSplitSolution, answer

# Mapping for moves and scores
MOVE_SCORE = {"X": 1, "Y": 2, "Z": 3}
OPPONENT = {"A": 0, "B": 1, "C": 2}
PLAYER = {"X": 0, "Y": 1, "Z": 2}


class Solution(StrSplitSolution):
    _year = 2022
    _day = 2

    @answer(11449)
    def part_1(self) -> int:
        total = 0
        for opp, you in [line.split() for line in self.input]:
            opp_idx = OPPONENT[opp]
            you_idx = PLAYER[you]
            # Outcome: 0=draw, 1=win, 2=lose (modulo 3)
            outcome = (you_idx - opp_idx) % 3
            if outcome == 0:
                score = 3  # draw
            elif outcome == 1:
                score = 6  # win
            else:
                score = 0  # lose
            total += MOVE_SCORE[you] + score
        return total

    @answer(13187)
    def part_2(self) -> int:
        total = 0
        for opp, outcome in [line.split() for line in self.input]:
            opp_idx = OPPONENT[opp]
            # X=lose, Y=draw, Z=win
            if outcome == "X":  # need to lose
                you_idx = (opp_idx - 1) % 3
                score = 0
            elif outcome == "Y":  # need to draw
                you_idx = opp_idx
                score = 3
            else:  # need to win
                you_idx = (opp_idx + 1) % 3
                score = 6
            # Convert you_idx back to move for score lookup
            move = ["X", "Y", "Z"][you_idx]
            total += MOVE_SCORE[move] + score
        return total

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
