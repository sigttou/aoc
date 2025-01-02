# Generated using @xavdid's AoC Python Template:
# https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2015/day/21

from itertools import combinations

from ...base import StrSplitSolution, answer


class Solution(StrSplitSolution):
    _year = 2015
    _day = 21

    def __init__(self, run_slow=False, is_debugging=False, use_test_data=False):
        super().__init__(run_slow, is_debugging, use_test_data)

        shop = """Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0

Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5

Rings:      Cost  Damage  Armor
Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3"""

        self.shop = {"weapons": [], "armors": [], "rings": []}
        for i, e in enumerate(self.shop.keys()):
            for line in shop.split("\n\n")[i].splitlines()[1:]:
                self.shop[e].append(tuple(map(int, line.split()[-3:])))

        self.shop["armors"].append((0, 0, 0))
        self.shop["rings"].append((0, 0, 0))

        for c in combinations(self.shop["rings"], 2):
            self.shop["rings"].append(
                (c[0][0] + c[1][0], c[0][1] + c[1][1], c[0][2] + c[1][2])
            )

        self.boss = {"hp": 0, "damage": 0, "armor": 0}
        for i, e in enumerate(self.boss.keys()):
            self.boss[e] = int(self.input[i].split()[-1])

        self.player = {"hp": 100, "damage": 0, "armor": 0}

    def chk_win(self, p_damage, p_armor):
        hps = [self.player["hp"], self.boss["hp"]]
        damage = (p_damage, self.boss["damage"])
        armor = (p_armor, self.boss["armor"])

        cur = 0
        while all(hp > 0 for hp in hps):
            hps[1 - cur] -= max(damage[cur] - armor[1 - cur], 1)
            cur = 1 - cur

        return hps[0] > 0

    @answer((78, 148))
    def solve(self) -> tuple[int, int]:
        min_spent, max_spent = 9999999999999, 0

        for weapon in self.shop["weapons"]:
            for armor in self.shop["armors"]:
                for ring in self.shop["rings"]:
                    player = list(zip(weapon, armor, ring))
                    c, d, a = sum(player[0]), sum(player[1]), sum(player[2])
                    if self.chk_win(d, a):
                        min_spent = c if c < min_spent else min_spent
                    else:
                        max_spent = c if c > max_spent else max_spent

        return min_spent, max_spent
