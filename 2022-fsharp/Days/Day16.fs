module Day16

open System
open System.Text.RegularExpressions
open System.Collections.Generic

type Valve = {
    Name: string
    FlowRate: int
    Tunnels: string list
}

let parseValve (line: string) =
    let m =
        Regex.Match(line, @"Valve ([A-Z]+) has flow rate=(\d+); tunnels? leads? to valves? (.+)")

    {
        Name = m.Groups.[1].Value
        FlowRate = int m.Groups.[2].Value
        Tunnels =
            m.Groups.[3].Value.Split([| ", " |], StringSplitOptions.None)
            |> List.ofArray
    }

let buildDistanceMap (valves: Map<string, Valve>) =
    let valveNames =
        valves
        |> Map.toList
        |> List.map fst

    let mutable dist = Map.empty

    for v1 in valveNames do
        for v2 in valveNames do
            if v1 = v2 then
                dist <-
                    dist
                    |> Map.add (v1, v2) 0
            elif List.contains v2 valves.[v1].Tunnels then
                dist <-
                    dist
                    |> Map.add (v1, v2) 1
            else
                dist <-
                    dist
                    |> Map.add (v1, v2) 1000000

    for k in valveNames do
        for i in valveNames do
            for j in valveNames do
                let current = dist.[(i, j)]

                let through =
                    dist.[(i, k)]
                    + dist.[(k, j)]

                if through < current then
                    dist <-
                        dist
                        |> Map.add (i, j) through

    dist

let solve (valves: Map<string, Valve>) (timeLimit: int) (usePart2: bool) =
    let distances = buildDistanceMap valves

    let importantValves =
        valves
        |> Map.toList
        |> List.filter (fun (_, v) -> v.FlowRate > 0)
        |> List.map fst

    let valveToMask =
        importantValves
        |> List.mapi (fun i name -> name, 1 <<< i)
        |> Map.ofList

    let cache = Dictionary<string * int * int, int>()

    let rec maxPressure (current: string) (openMask: int) (timeLeft: int) =
        if
            timeLeft
            <= 0
        then
            0
        else
            let key = (current, openMask, timeLeft)

            match cache.TryGetValue(key) with
            | true, result -> result
            | false, _ ->
                let mutable best = 0

                for valve in importantValves do
                    let mask = valveToMask.[valve]

                    if
                        (openMask
                         &&& mask) = 0
                    then
                        let dist = distances.[(current, valve)]

                        let timeAfterMove =
                            timeLeft
                            - dist
                            - 1

                        if timeAfterMove > 0 then
                            let pressure =
                                valves.[valve].FlowRate
                                * timeAfterMove

                            let future =
                                maxPressure
                                    valve
                                    (openMask
                                     ||| mask)
                                    timeAfterMove

                            best <-
                                max
                                    best
                                    (pressure
                                     + future)

                cache.[key] <- best
                best

    if not usePart2 then
        maxPressure "AA" 0 timeLimit
    else
        let allMasks =
            1
            <<< (List.length importantValves)

        let mutable bestTotal = 0

        for myMask in
            0 .. (allMasks
                  - 1) do
            let elephantMask =
                (allMasks
                 - 1)
                ^^^ myMask

            let myPressure =
                maxPressure
                    "AA"
                    ((allMasks
                      - 1)
                     ^^^ myMask)
                    timeLimit

            let elephantPressure =
                maxPressure
                    "AA"
                    ((allMasks
                      - 1)
                     ^^^ elephantMask)
                    timeLimit

            bestTotal <-
                max
                    bestTotal
                    (myPressure
                     + elephantPressure)

        bestTotal

let part1 (input: string) =
    let valves =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseValve
        |> Array.map (fun v -> v.Name, v)
        |> Map.ofArray

    solve valves 30 false

let part2 (input: string) =
    let valves =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseValve
        |> Array.map (fun v -> v.Name, v)
        |> Map.ofArray

    solve valves 26 true
