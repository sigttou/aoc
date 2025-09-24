# Generated using @xavdid's AoC Python Template: https://github.com/xavdid/advent-of-code-python-template

# puzzle prompt: https://adventofcode.com/2022/day/19

import re
import math
import dataclasses
import enum
from functools import cached_property
from ...base import StrSplitSolution, answer


RobotRecipe = dict[str, int]  # ingredient => quantity required
Blueprint = dict[str, RobotRecipe]  # robot => recipe to make the robot


class Materials(str, enum.Enum):
    ORE = "ore"
    CLAY = "clay"
    OBSIDIAN = "obsidian"
    GEODE = "geode"


def parse_blueprints(lines):
    blueprints = []
    for line in lines:
        blueprint = {}
        # Parse robot recipes
        for robot, *costs in re.findall(r"Each (\w+) robot costs ([^\.]+)\.", line):
            recipe = {}
            for cost in costs[0].split(" and "):
                n, mat = cost.strip().split()
                recipe[mat] = int(n)
            blueprint[robot] = recipe
        blueprints.append(blueprint)
    return blueprints


@dataclasses.dataclass(frozen=True)
class State:
    time: int
    time_steps: int
    blueprint: Blueprint
    inventory: dict[str, int]
    robots: dict[str, int]

    def get_possible_states(self):
        states = []
        for robot_you_can_make in self.robots.keys():
            # Must have at least one robot for each required material
            if min([self.robots[x] for x in self.blueprint[robot_you_can_make]]) > 0:
                # Don't build more robots than needed (except geode)
                if (
                    robot_you_can_make != Materials.GEODE
                    and max(
                        [x.get(robot_you_can_make, 0) for x in self.blueprint.values()]
                    )
                    <= self.robots[robot_you_can_make]
                ):
                    continue
                # Time until enough resources to build
                time_until = max(
                    [
                        math.ceil(max(q - self.inventory[m], 0) / self.robots[m])
                        for m, q in self.blueprint[robot_you_can_make].items()
                    ]
                )
                if (self.time + time_until + 1) < self.time_steps:
                    new_inventory = {
                        material: (
                            quantity
                            - self.blueprint[robot_you_can_make].get(material, 0)
                            + (time_until + 1) * self.robots.get(material, 0)
                        )
                        for material, quantity in self.inventory.items()
                    }
                    new_robots = {
                        robot: quantity + (1 if robot == robot_you_can_make else 0)
                        for robot, quantity in self.robots.items()
                    }
                    states.append(
                        State(
                            time=self.time + time_until + 1,
                            time_steps=self.time_steps,
                            blueprint=self.blueprint,
                            inventory=new_inventory,
                            robots=new_robots,
                        )
                    )
        return states

    @cached_property
    def objective(self):
        return (
            self.robots[Materials.GEODE] * (self.time_steps - self.time)
        ) + self.inventory[Materials.GEODE]

    @cached_property
    def estimate(self):
        # Highball estimate: build a geode robot every step
        return self.objective + sum(range(self.time_steps - self.time))


def simulate_blueprint(blueprint, time_steps):
    starting_state = State(
        time=0,
        time_steps=time_steps,
        blueprint=blueprint,
        inventory={m.value: 0 for m in Materials},
        robots={m.value: 1 if m == Materials.ORE else 0 for m in Materials},
    )
    frontier = starting_state.get_possible_states()
    best_state = starting_state
    while frontier:
        state = frontier.pop(0)
        for possible_state in state.get_possible_states():
            if possible_state.objective > best_state.objective:
                best_state = possible_state
            if (
                possible_state.time < time_steps
                and possible_state.estimate > best_state.objective
            ):
                frontier.append(possible_state)
    return best_state


class Solution(StrSplitSolution):
    _year = 2022
    _day = 19

    def _parse(self):
        return parse_blueprints(self.input)

    @answer(1650)
    def part_1(self) -> int:
        blueprints = self._parse()
        total = 0
        for i, blueprint in enumerate(blueprints):
            solution = simulate_blueprint(blueprint, 24)
            total += (i + 1) * solution.objective
        return total

    @answer(5824)
    def part_2(self) -> int:
        blueprints = self._parse()[:3]
        prod = 1
        for blueprint in blueprints:
            solution = simulate_blueprint(blueprint, 32)
            prod *= solution.objective
        return prod

    # @answer((1234, 4567))
    # def solve(self) -> tuple[int, int]:
    #     pass
