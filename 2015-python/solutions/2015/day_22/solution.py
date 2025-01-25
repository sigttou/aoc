# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/22

from copy import deepcopy

from ...base import StrSplitSolution, answer

leastManaUsed = 9999999999999999


class Solution(StrSplitSolution):
    _year = 2015
    _day = 22

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)

        self.player = {"hp": 50, "mana": 500, "mana_spent": 0, "spells": []}

        self.boss = {"hp": 0, "damage": 0}
        for i, k in enumerate(self.boss.keys()):
            self.boss[k] = int(self.input[i].split(" ")[-1])

        self.spells = {}  # [cost, dmg, hp, armour, mana, turns]
        self.spells["missile"] = [53, 4, 0, 0, 0, 0]
        self.spells["drain"] = [73, 2, 2, 0, 0, 0]
        self.spells["shield"] = [113, 0, 0, 7, 0, 6]
        self.spells["poison"] = [173, 3, 0, 0, 0, 6]
        self.spells["recharge"] = [229, 0, 0, 0, 101, 5]

        self.least_mana = 9999999999999999

    def simulate(self, my_turn, player, boss, p2):
        armour = 0

        if p2 and my_turn:
            player["hp"] -= 1
            if player["hp"] <= 0:
                return False

        n_spells = []
        for spell in player["spells"]:
            boss["hp"] -= spell[1]
            player["hp"] += spell[2]
            armour += spell[3]
            player["mana"] += spell[4]

            n_spell = deepcopy(spell)
            n_spell[-1] -= 1
            if n_spell[-1] > 0:
                n_spells.append(n_spell)

        if boss["hp"] <= 0:
            self.least_mana = min(self.least_mana, player["mana_spent"])
            return True

        if player["mana_spent"] >= self.least_mana:
            return False

        if my_turn:
            for v in self.spells.values():
                if v[0] in [e[0] for e in n_spells]:
                    continue
                if v[0] <= player["mana"]:
                    call_spells = deepcopy(n_spells)
                    call_spells.append(deepcopy(v))
                    n_player = deepcopy(player)
                    n_player["mana"] -= v[0]
                    n_player["mana_spent"] += v[0]
                    n_player["spells"] = call_spells
                    self.simulate(False, n_player, deepcopy(boss), p2)
            return None
        player["hp"] += armour - boss["damage"] if armour - boss["damage"] < 0 else -1
        if player["hp"] > 0:
            player["spells"] = n_spells
            self.simulate(True, player, boss, p2)
            return None
        return None

    @answer(953)
    def part_1(self) -> int:
        self.simulate(True, deepcopy(self.player), deepcopy(self.boss), False)
        return self.least_mana

    @answer(1289)
    def part_2(self) -> int:
        self.least_mana = 999999999999
        self.simulate(True, deepcopy(self.player), deepcopy(self.boss), True)
        return self.least_mana
