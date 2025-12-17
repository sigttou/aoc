module Day14

open System
open System.Collections.Generic

type Position = { X: int; Y: int }

let parsePath (line: string) =
    line.Split([| " -> " |], StringSplitOptions.None)
    |> Array.map (fun coord ->
        let parts = coord.Split(',')
        { X = int parts.[0]; Y = int parts.[1] }
    )
    |> Array.toList

let drawLine (from: Position) (to_: Position) =
    if from.X = to_.X then
        let minY = min from.Y to_.Y
        let maxY = max from.Y to_.Y
        [ for y in minY..maxY -> { X = from.X; Y = y } ]
    else
        let minX = min from.X to_.X
        let maxX = max from.X to_.X
        [ for x in minX..maxX -> { X = x; Y = from.Y } ]

let buildRocks (paths: Position list list) =
    let rocks = HashSet<Position>()

    for path in paths do
        for i in
            0 .. (List.length path
                  - 2) do
            let line = drawLine path.[i] path.[i + 1]

            for pos in line do
                rocks.Add(pos)
                |> ignore

    rocks

let simulateSand (rocks: HashSet<Position>) (maxY: int) (hasFloor: bool) =
    let sand = HashSet<Position>()
    let source = { X = 500; Y = 0 }

    let isBlocked (pos: Position) =
        if
            hasFloor
            && pos.Y = maxY + 2
        then
            true
        else
            rocks.Contains(pos)
            || sand.Contains(pos)

    let rec dropSand (pos: Position) =
        if
            not hasFloor
            && pos.Y > maxY
        then
            None
        else
            let down = { pos with Y = pos.Y + 1 }
            let downLeft = { X = pos.X - 1; Y = pos.Y + 1 }
            let downRight = { X = pos.X + 1; Y = pos.Y + 1 }

            if not (isBlocked down) then dropSand down
            elif not (isBlocked downLeft) then dropSand downLeft
            elif not (isBlocked downRight) then dropSand downRight
            else Some pos

    let rec simulate count =
        match dropSand source with
        | None -> count
        | Some pos ->
            sand.Add(pos)
            |> ignore

            if pos = source then
                count + 1 // Source blocked (part 2)
            else
                simulate (count + 1)

    simulate 0

let part1 (input: string) =
    let paths =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parsePath
        |> Array.toList

    let rocks = buildRocks paths

    let maxY =
        rocks
        |> Seq.map (fun r -> r.Y)
        |> Seq.max

    simulateSand rocks maxY false

let part2 (input: string) =
    let paths =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parsePath
        |> Array.toList

    let rocks = buildRocks paths

    let maxY =
        rocks
        |> Seq.map (fun r -> r.Y)
        |> Seq.max

    simulateSand rocks maxY true
