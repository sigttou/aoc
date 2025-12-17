module Day19

open System
open System.Text.RegularExpressions
open System.Collections.Generic

type Blueprint = {
    Id: int
    OreRobotCost: int
    ClayRobotCost: int
    ObsidianRobotCost: int * int
    GeodeRobotCost: int * int
}

let parseBlueprint (line: string) =
    let m =
        Regex.Match(
            line,
            @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian."
        )

    {
        Id = int m.Groups.[1].Value
        OreRobotCost = int m.Groups.[2].Value
        ClayRobotCost = int m.Groups.[3].Value
        ObsidianRobotCost = (int m.Groups.[4].Value, int m.Groups.[5].Value)
        GeodeRobotCost = (int m.Groups.[6].Value, int m.Groups.[7].Value)
    }

type State = {
    Time: int
    OreRobots: int
    ClayRobots: int
    ObsidianRobots: int
    GeodeRobots: int
    Ore: int
    Clay: int
    Obsidian: int
    Geodes: int
}

let simulate (blueprint: Blueprint) (timeLimit: int) =
    let maxOreCost =
        [
            blueprint.OreRobotCost
            blueprint.ClayRobotCost
            fst blueprint.ObsidianRobotCost
            fst blueprint.GeodeRobotCost
        ]
        |> List.max

    let maxClayCost = snd blueprint.ObsidianRobotCost
    let maxObsidianCost = snd blueprint.GeodeRobotCost

    let cache = Dictionary<State, int>()
    let mutable globalBest = 0

    let rec search (state: State) =
        if state.Time = 0 then
            globalBest <- max globalBest state.Geodes
            state.Geodes
        else
            let maxPossible =
                state.Geodes
                + state.GeodeRobots
                  * state.Time
                + (state.Time
                   * (state.Time
                      - 1))
                  / 2

            if
                maxPossible
                <= globalBest
            then
                0
            else
                let cacheState = {
                    state with
                        Ore =
                            min
                                state.Ore
                                (state.Time
                                 * maxOreCost
                                 - state.OreRobots
                                   * (state.Time
                                      - 1))
                        Clay =
                            min
                                state.Clay
                                (state.Time
                                 * maxClayCost
                                 - state.ClayRobots
                                   * (state.Time
                                      - 1))
                        Obsidian =
                            min
                                state.Obsidian
                                (state.Time
                                 * maxObsidianCost
                                 - state.ObsidianRobots
                                   * (state.Time
                                      - 1))
                }

                match cache.TryGetValue(cacheState) with
                | true, result -> result
                | false, _ ->
                    let mutable options = []

                    if
                        state.Ore
                        >= fst blueprint.GeodeRobotCost
                        && state.Obsidian
                           >= snd blueprint.GeodeRobotCost
                    then
                        let newState = {
                            Time =
                                state.Time
                                - 1
                            OreRobots = state.OreRobots
                            ClayRobots = state.ClayRobots
                            ObsidianRobots = state.ObsidianRobots
                            GeodeRobots =
                                state.GeodeRobots
                                + 1
                            Ore =
                                state.Ore
                                + state.OreRobots
                                - fst blueprint.GeodeRobotCost
                            Clay =
                                state.Clay
                                + state.ClayRobots
                            Obsidian =
                                state.Obsidian
                                + state.ObsidianRobots
                                - snd blueprint.GeodeRobotCost
                            Geodes =
                                state.Geodes
                                + state.GeodeRobots
                        }

                        options <-
                            newState
                            :: options
                    elif state.ObsidianRobots > 0 then
                        let oreNeeded =
                            max
                                0
                                (fst blueprint.GeodeRobotCost
                                 - state.Ore)

                        let obsidianNeeded =
                            max
                                0
                                (snd blueprint.GeodeRobotCost
                                 - state.Obsidian)

                        let timeToWait =
                            max
                                ((oreNeeded
                                  + state.OreRobots
                                  - 1)
                                 / state.OreRobots)
                                ((obsidianNeeded
                                  + state.ObsidianRobots
                                  - 1)
                                 / state.ObsidianRobots)

                        if timeToWait < state.Time then
                            let newState = {
                                Time =
                                    state.Time
                                    - timeToWait
                                    - 1
                                OreRobots = state.OreRobots
                                ClayRobots = state.ClayRobots
                                ObsidianRobots = state.ObsidianRobots
                                GeodeRobots =
                                    state.GeodeRobots
                                    + 1
                                Ore =
                                    state.Ore
                                    + state.OreRobots
                                      * (timeToWait
                                         + 1)
                                    - fst blueprint.GeodeRobotCost
                                Clay =
                                    state.Clay
                                    + state.ClayRobots
                                      * (timeToWait
                                         + 1)
                                Obsidian =
                                    state.Obsidian
                                    + state.ObsidianRobots
                                      * (timeToWait
                                         + 1)
                                    - snd blueprint.GeodeRobotCost
                                Geodes =
                                    state.Geodes
                                    + state.GeodeRobots
                                      * (timeToWait
                                         + 1)
                            }

                            options <-
                                newState
                                :: options

                    if state.ObsidianRobots < maxObsidianCost then
                        if
                            state.Ore
                            >= fst blueprint.ObsidianRobotCost
                            && state.Clay
                               >= snd blueprint.ObsidianRobotCost
                        then
                            let newState = {
                                Time =
                                    state.Time
                                    - 1
                                OreRobots = state.OreRobots
                                ClayRobots = state.ClayRobots
                                ObsidianRobots =
                                    state.ObsidianRobots
                                    + 1
                                GeodeRobots = state.GeodeRobots
                                Ore =
                                    state.Ore
                                    + state.OreRobots
                                    - fst blueprint.ObsidianRobotCost
                                Clay =
                                    state.Clay
                                    + state.ClayRobots
                                    - snd blueprint.ObsidianRobotCost
                                Obsidian =
                                    state.Obsidian
                                    + state.ObsidianRobots
                                Geodes =
                                    state.Geodes
                                    + state.GeodeRobots
                            }

                            options <-
                                newState
                                :: options
                        elif state.ClayRobots > 0 then
                            let oreNeeded =
                                max
                                    0
                                    (fst blueprint.ObsidianRobotCost
                                     - state.Ore)

                            let clayNeeded =
                                max
                                    0
                                    (snd blueprint.ObsidianRobotCost
                                     - state.Clay)

                            let timeToWait =
                                max
                                    ((oreNeeded
                                      + state.OreRobots
                                      - 1)
                                     / state.OreRobots)
                                    ((clayNeeded
                                      + state.ClayRobots
                                      - 1)
                                     / state.ClayRobots)

                            if timeToWait < state.Time then
                                let newState = {
                                    Time =
                                        state.Time
                                        - timeToWait
                                        - 1
                                    OreRobots = state.OreRobots
                                    ClayRobots = state.ClayRobots
                                    ObsidianRobots =
                                        state.ObsidianRobots
                                        + 1
                                    GeodeRobots = state.GeodeRobots
                                    Ore =
                                        state.Ore
                                        + state.OreRobots
                                          * (timeToWait
                                             + 1)
                                        - fst blueprint.ObsidianRobotCost
                                    Clay =
                                        state.Clay
                                        + state.ClayRobots
                                          * (timeToWait
                                             + 1)
                                        - snd blueprint.ObsidianRobotCost
                                    Obsidian =
                                        state.Obsidian
                                        + state.ObsidianRobots
                                          * (timeToWait
                                             + 1)
                                    Geodes =
                                        state.Geodes
                                        + state.GeodeRobots
                                          * (timeToWait
                                             + 1)
                                }

                                options <-
                                    newState
                                    :: options

                        if state.ClayRobots < maxClayCost then
                            if
                                state.Ore
                                >= blueprint.ClayRobotCost
                            then
                                let newState = {
                                    Time =
                                        state.Time
                                        - 1
                                    OreRobots = state.OreRobots
                                    ClayRobots =
                                        state.ClayRobots
                                        + 1
                                    ObsidianRobots = state.ObsidianRobots
                                    GeodeRobots = state.GeodeRobots
                                    Ore =
                                        state.Ore
                                        + state.OreRobots
                                        - blueprint.ClayRobotCost
                                    Clay =
                                        state.Clay
                                        + state.ClayRobots
                                    Obsidian =
                                        state.Obsidian
                                        + state.ObsidianRobots
                                    Geodes =
                                        state.Geodes
                                        + state.GeodeRobots
                                }

                                options <-
                                    newState
                                    :: options
                            else
                                let oreNeeded =
                                    blueprint.ClayRobotCost
                                    - state.Ore

                                let timeToWait =
                                    (oreNeeded
                                     + state.OreRobots
                                     - 1)
                                    / state.OreRobots

                                if timeToWait < state.Time then
                                    let newState = {
                                        Time =
                                            state.Time
                                            - timeToWait
                                            - 1
                                        OreRobots = state.OreRobots
                                        ClayRobots =
                                            state.ClayRobots
                                            + 1
                                        ObsidianRobots = state.ObsidianRobots
                                        GeodeRobots = state.GeodeRobots
                                        Ore =
                                            state.Ore
                                            + state.OreRobots
                                              * (timeToWait
                                                 + 1)
                                            - blueprint.ClayRobotCost
                                        Clay =
                                            state.Clay
                                            + state.ClayRobots
                                              * (timeToWait
                                                 + 1)
                                        Obsidian =
                                            state.Obsidian
                                            + state.ObsidianRobots
                                              * (timeToWait
                                                 + 1)
                                        Geodes =
                                            state.Geodes
                                            + state.GeodeRobots
                                              * (timeToWait
                                                 + 1)
                                    }

                                    options <-
                                        newState
                                        :: options

                    if state.OreRobots < maxOreCost then
                        if
                            state.Ore
                            >= blueprint.OreRobotCost
                        then
                            let newState = {
                                Time =
                                    state.Time
                                    - 1
                                OreRobots =
                                    state.OreRobots
                                    + 1
                                ClayRobots = state.ClayRobots
                                ObsidianRobots = state.ObsidianRobots
                                GeodeRobots = state.GeodeRobots
                                Ore =
                                    state.Ore
                                    + state.OreRobots
                                    - blueprint.OreRobotCost
                                Clay =
                                    state.Clay
                                    + state.ClayRobots
                                Obsidian =
                                    state.Obsidian
                                    + state.ObsidianRobots
                                Geodes =
                                    state.Geodes
                                    + state.GeodeRobots
                            }

                            options <-
                                newState
                                :: options
                        else
                            let oreNeeded =
                                blueprint.OreRobotCost
                                - state.Ore

                            let timeToWait =
                                (oreNeeded
                                 + state.OreRobots
                                 - 1)
                                / state.OreRobots

                            if timeToWait < state.Time then
                                let newState = {
                                    Time =
                                        state.Time
                                        - timeToWait
                                        - 1
                                    OreRobots =
                                        state.OreRobots
                                        + 1
                                    ClayRobots = state.ClayRobots
                                    ObsidianRobots = state.ObsidianRobots
                                    GeodeRobots = state.GeodeRobots
                                    Ore =
                                        state.Ore
                                        + state.OreRobots
                                          * (timeToWait
                                             + 1)
                                        - blueprint.OreRobotCost
                                    Clay =
                                        state.Clay
                                        + state.ClayRobots
                                          * (timeToWait
                                             + 1)
                                    Obsidian =
                                        state.Obsidian
                                        + state.ObsidianRobots
                                          * (timeToWait
                                             + 1)
                                    Geodes =
                                        state.Geodes
                                        + state.GeodeRobots
                                          * (timeToWait
                                             + 1)
                                }

                                options <-
                                    newState
                                    :: options

                    let result =
                        if List.isEmpty options then
                            state.Geodes
                            + state.GeodeRobots
                              * state.Time
                        else
                            options
                            |> List.map search
                            |> List.max

                    cache.[cacheState] <- result
                    result

    let initialState = {
        Time = timeLimit
        OreRobots = 1
        ClayRobots = 0
        ObsidianRobots = 0
        GeodeRobots = 0
        Ore = 0
        Clay = 0
        Obsidian = 0
        Geodes = 0
    }

    search initialState

let part1 (input: string) =
    let blueprints =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseBlueprint

    blueprints
    |> Array.map (fun bp ->
        bp.Id
        * simulate bp 24
    )
    |> Array.sum

let part2 (input: string) =
    let blueprints =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseBlueprint
        |> Array.truncate 3

    blueprints
    |> Array.map (fun bp -> simulate bp 32)
    |> Array.fold (*) 1
