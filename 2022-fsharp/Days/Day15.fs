module Day15

open System
open System.Text.RegularExpressions

type Position = { X: int64; Y: int64 }

type Sensor = {
    Position: Position
    Beacon: Position
    Distance: int64
}

let manhattanDistance (p1: Position) (p2: Position) =
    abs (
        p1.X
        - p2.X
    )
    + abs (
        p1.Y
        - p2.Y
    )

let parseSensor (line: string) =
    let m =
        Regex.Match(
            line,
            @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)"
        )

    let sensorPos = {
        X = int64 m.Groups.[1].Value
        Y = int64 m.Groups.[2].Value
    }

    let beaconPos = {
        X = int64 m.Groups.[3].Value
        Y = int64 m.Groups.[4].Value
    }

    {
        Position = sensorPos
        Beacon = beaconPos
        Distance = manhattanDistance sensorPos beaconPos
    }

let getCoverageOnRow (sensor: Sensor) (row: int64) =
    let distToRow =
        abs (
            sensor.Position.Y
            - row
        )

    let remainingDist =
        sensor.Distance
        - distToRow

    if remainingDist < 0L then
        None
    else
        Some(
            sensor.Position.X
            - remainingDist,
            sensor.Position.X
            + remainingDist
        )

let mergeRanges (ranges: (int64 * int64) list) =
    let sorted =
        ranges
        |> List.sortBy fst

    let rec merge (acc: (int64 * int64) list) (remaining: (int64 * int64) list) =
        match acc, remaining with
        | [], r :: rest -> merge [ r ] rest
        | [], [] -> []
        | (aStart, aEnd) :: accRest, (rStart, rEnd) :: rest ->
            if
                rStart
                <= aEnd + 1L
            then
                merge
                    ((aStart, max aEnd rEnd)
                     :: accRest)
                    rest
            else
                merge
                    ((rStart, rEnd)
                     :: (aStart, aEnd)
                     :: accRest)
                    rest
        | acc, [] -> List.rev acc

    merge [] sorted

let countCoveredPositions (ranges: (int64 * int64) list) (beaconsOnRow: Set<int64>) =
    ranges
    |> List.sumBy (fun (start, end_) ->
        let rangeSize =
            end_
            - start
            + 1L

        let beaconsInRange =
            beaconsOnRow
            |> Set.filter (fun x ->
                x >= start
                && x <= end_
            )
            |> Set.count
            |> int64

        rangeSize
        - beaconsInRange
    )

let part1 (input: string) =
    let targetRow = 2000000L

    let sensors =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseSensor
        |> Array.toList

    let ranges =
        sensors
        |> List.choose (fun s -> getCoverageOnRow s targetRow)
        |> mergeRanges

    let beaconsOnRow =
        sensors
        |> List.map (fun s -> s.Beacon)
        |> List.filter (fun b -> b.Y = targetRow)
        |> List.map (fun b -> b.X)
        |> Set.ofList

    countCoveredPositions ranges beaconsOnRow

let findDistressBeacon (sensors: Sensor list) (maxCoord: int64) =
    let rec checkRow row =
        if row > maxCoord then
            None
        else
            let ranges =
                sensors
                |> List.choose (fun s -> getCoverageOnRow s row)
                |> List.filter (fun (start, end_) ->
                    end_ >= 0L
                    && start
                       <= maxCoord
                )
                |> List.map (fun (start, end_) -> (max 0L start, min maxCoord end_))
                |> mergeRanges

            match ranges with
            | [] -> checkRow (row + 1L)
            | [ (start, end_) ] when
                start = 0L
                && end_ = maxCoord
                ->
                checkRow (row + 1L)
            | _ ->
                let rec findGap ranges =
                    match ranges with
                    | (_, end1) :: (start2, _) :: _ when end1 + 2L = start2 ->
                        Some { X = end1 + 1L; Y = row }
                    | _ :: rest -> findGap rest
                    | [] -> None

                match findGap ranges with
                | Some pos -> Some pos
                | None ->
                    let (firstStart, _) = List.head ranges
                    let (_, lastEnd) = List.last ranges

                    if firstStart > 0L then Some { X = 0L; Y = row }
                    elif lastEnd < maxCoord then Some { X = maxCoord; Y = row }
                    else checkRow (row + 1L)

    checkRow 0L

let part2 (input: string) =
    let maxCoord = 4000000L

    let sensors =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseSensor
        |> Array.toList

    match findDistressBeacon sensors maxCoord with
    | Some pos ->
        pos.X
        * 4000000L
        + pos.Y
    | None -> -1L
