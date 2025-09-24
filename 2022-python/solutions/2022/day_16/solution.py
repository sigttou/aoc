# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/16

from ...base import StrSplitSolution, answer
import re
from functools import lru_cache


class Solution(StrSplitSolution):
    _year = 2022
    _day = 16

    def _parse(self):
        valves = {}
        rates = {}
        for line in self.input:
            name = line[6:8]
            rate = int(re.search(r"rate=(\d+)", line).group(1))
            tunnels = re.findall(r"[A-Z]{2}", line[23:])
            valves[name] = tunnels
            rates[name] = rate
        return valves, rates

    def floyd_warshall(self, valves):
        # Compute shortest paths between all valves
        dist = {v: {u: float("inf") for u in valves} for v in valves}
        for v in valves:
            dist[v][v] = 0
            for u in valves[v]:
                dist[v][u] = 1
        for k in valves:
            for i in valves:
                for j in valves:
                    if dist[i][j] > dist[i][k] + dist[k][j]:
                        dist[i][j] = dist[i][k] + dist[k][j]
        return dist

    @answer(1488)
    def part_1(self) -> int:
        valves, rates = self._parse()
        dist = self.floyd_warshall(valves)
        useful = tuple([v for v in rates if rates[v] > 0])

        @lru_cache(maxsize=None)
        def dfs(pos, time, opened):
            best = 0
            for v in useful:
                if v in opened:
                    continue
                t = dist[pos][v] + 1
                if time + t > 30:
                    continue
                released = rates[v] * (30 - (time + t))
                best = max(best, released + dfs(v, time + t, opened + (v,)))
            return best

        return dfs("AA", 0, ())

    @answer(2111)
    def part_2(self) -> int:
        valves, rates = self._parse()
        dist = self.floyd_warshall(valves)
        useful = tuple([v for v in rates if rates[v] > 0])

        # Try all possible splits of useful valves between you and elephant
        best = 0
        n = len(useful)
        from itertools import combinations

        # Only need to try half the splits due to symmetry
        for i in range(n // 2 + 1):
            for mine in combinations(useful, i):
                elephant = tuple(v for v in useful if v not in mine)

                @lru_cache(maxsize=None)
                def dfs(pos, time, opened, targets):
                    res = 0
                    for v in targets:
                        if v in opened:
                            continue
                        t = dist[pos][v] + 1
                        if time + t > 26:
                            continue
                        released = rates[v] * (26 - (time + t))
                        res = max(
                            res, released + dfs(v, time + t, opened + (v,), targets)
                        )
                    return res

                total = dfs("AA", 0, (), mine) + dfs("AA", 0, (), elephant)
                if total > best:
                    best = total
        return best

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
