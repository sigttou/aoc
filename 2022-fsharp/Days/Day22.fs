module Day22

open System
open System.Text.RegularExpressions

type Instruction =
    | Go of int
    | TurnLeft
    | TurnRight

let parse (input: string) =
    let parts = input.Split([| "\n\n" |], StringSplitOptions.None)
    let lines = parts.[0].Split('\n')
    let pathLine = parts.[1].Trim()

    let instructions =
        Regex.Matches(pathLine, @"\d+|[LR]")
        |> Seq.map (fun m ->
            match Int32.TryParse(m.Value) with
            | true, n -> Go n
            | false, _ -> if m.Value = "L" then TurnLeft else TurnRight
        )
        |> List.ofSeq

    (lines, instructions)

type Facing =
    | Up
    | Down
    | Left
    | Right

let turnLeft f =
    match f with
    | Up -> Left
    | Left -> Down
    | Down -> Right
    | Right -> Up

let turnRight f =
    match f with
    | Up -> Right
    | Right -> Down
    | Down -> Left
    | Left -> Up

let facingValue f =
    match f with
    | Right -> 0
    | Down -> 1
    | Left -> 2
    | Up -> 3

let delta f =
    match f with
    | Up -> (-1, 0)
    | Down -> (1, 0)
    | Left -> (0, -1)
    | Right -> (0, 1)

// Hardcoded cube transitions for actual input (50x50 faces)
let changeSideActual (maxI: int) (side: int) (facing: Facing) (row: int, col: int) =
    match (side, facing) with
    | (0, Up) -> (5, Right, (col, 0))
    | (0, Left) -> (3, Right, (maxI - row, 0))
    | (0, Right) -> (1, Right, (row, 0))
    | (0, Down) -> (2, Down, (0, col))

    | (1, Up) -> (5, Up, (maxI, col))
    | (1, Left) -> (0, Left, (row, maxI))
    | (1, Right) -> (4, Left, (maxI - row, maxI))
    | (1, Down) -> (2, Left, (col, maxI))

    | (2, Up) -> (0, Up, (maxI, col))
    | (2, Left) -> (3, Down, (0, row))
    | (2, Right) -> (1, Up, (maxI, row))
    | (2, Down) -> (4, Down, (0, col))

    | (3, Up) -> (2, Right, (col, 0))
    | (3, Left) -> (0, Right, (maxI - row, 0))
    | (3, Right) -> (4, Right, (row, 0))
    | (3, Down) -> (5, Down, (0, col))

    | (4, Up) -> (2, Up, (maxI, col))
    | (4, Left) -> (3, Left, (row, maxI))
    | (4, Right) -> (1, Left, (maxI - row, maxI))
    | (4, Down) -> (5, Left, (col, maxI))

    | (5, Up) -> (3, Up, (maxI, col))
    | (5, Left) -> (0, Down, (0, row))
    | (5, Right) -> (4, Up, (maxI, row))
    | (5, Down) -> (1, Down, (0, col))

    | _ -> failwith "Invalid transition"

let sidesActual = [|
    (0, 50)
    (0, 100)
    (50, 50)
    (100, 0)
    (100, 50)
    (150, 0)
|]

let part1 (input: string) =
    let (lines, instructions) = parse input

    let grid =
        lines
        |> Array.map (fun s -> s.ToCharArray())

    let height = grid.Length

    let width =
        grid
        |> Array.map Array.length
        |> Array.max

    let grid =
        grid
        |> Array.map (fun row ->
            if row.Length < width then
                Array.append
                    row
                    (Array.create
                        (width
                         - row.Length)
                        ' ')
            else
                row
        )

    let getCell r c =
        if
            r >= 0
            && r < height
            && c >= 0
            && c < width
        then
            grid.[r].[c]
        else
            ' '

    let mutable row = 0

    let mutable col =
        grid.[0]
        |> Array.findIndex ((=) '.')

    let mutable facing = Right

    for instr in instructions do
        match instr with
        | TurnLeft -> facing <- turnLeft facing
        | TurnRight -> facing <- turnRight facing
        | Go steps ->
            for _ in 1..steps do
                let (dr, dc) = delta facing
                let mutable nr = row + dr
                let mutable nc = col + dc

                if getCell nr nc = ' ' then
                    match facing with
                    | Right ->
                        nc <-
                            grid.[row]
                            |> Array.findIndex (fun c -> c <> ' ')
                    | Left ->
                        nc <-
                            grid.[row]
                            |> Array.findIndexBack (fun c -> c <> ' ')
                    | Down ->
                        let checkCol = col

                        nr <-
                            grid
                            |> Array.findIndex (fun r ->
                                checkCol < r.Length
                                && r.[checkCol]
                                   <> ' '
                            )
                    | Up ->
                        let checkCol = col

                        nr <-
                            grid
                            |> Array.findIndexBack (fun r ->
                                checkCol < r.Length
                                && r.[checkCol]
                                   <> ' '
                            )

                if getCell nr nc = '.' then
                    row <- nr
                    col <- nc

    1000
    * (row + 1)
    + 4
      * (col + 1)
    + facingValue facing

let part2 (input: string) =
    let (lines, instructions) = parse input

    let grid =
        lines
        |> Array.map (fun s -> s.ToCharArray())

    let sideLen = 50

    let maxI =
        sideLen
        - 1

    let getCell r c =
        if
            r >= 0
            && r < grid.Length
            && c >= 0
            && c < grid.[r].Length
        then
            grid.[r].[c]
        else
            ' '

    let mutable sideIdx = 0
    let mutable row = 0
    let mutable col = 0
    let mutable facing = Right

    for instr in instructions do
        match instr with
        | TurnLeft -> facing <- turnLeft facing
        | TurnRight -> facing <- turnRight facing
        | Go steps ->
            for _ in 1..steps do
                let (dr, dc) = delta facing
                let mutable nr = row + dr
                let mutable nc = col + dc
                let mutable nSide = sideIdx
                let mutable nFacing = facing

                if
                    (dr = 1
                     && nr = sideLen)
                    || (dc = 1
                        && nc = sideLen)
                    || (dr = -1
                        && nr = -1)
                    || (dc = -1
                        && nc = -1)
                then
                    let (newSide, newFacing, (newRow, newCol)) =
                        changeSideActual maxI sideIdx facing (row, col)

                    nSide <- newSide
                    nFacing <- newFacing
                    nr <- newRow
                    nc <- newCol

                let (sideR, sideC) = sidesActual.[nSide]
                let actualR = sideR + nr
                let actualC = sideC + nc

                if getCell actualR actualC = '.' then
                    sideIdx <- nSide
                    row <- nr
                    col <- nc
                    facing <- nFacing

    let (sideR, sideC) = sidesActual.[sideIdx]

    1000
    * (sideR
       + row
       + 1)
    + 4
      * (sideC
         + col
         + 1)
    + facingValue facing
