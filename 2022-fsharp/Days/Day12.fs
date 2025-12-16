module Day12

open System
open System.Collections.Generic

type Position = { Row: int; Col: int }

let parseGrid (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line -> line.ToCharArray())

let findPosition (grid: char[][]) (target: char) =
    seq {
        for row in
            0 .. (Array.length grid
                  - 1) do
            for col in
                0 .. (Array.length grid.[0]
                      - 1) do
                if grid.[row].[col] = target then
                    yield { Row = row; Col = col }
    }
    |> Seq.head

let findAllPositions (grid: char[][]) (target: char) =
    seq {
        for row in
            0 .. (Array.length grid
                  - 1) do
            for col in
                0 .. (Array.length grid.[0]
                      - 1) do
                if grid.[row].[col] = target then
                    yield { Row = row; Col = col }
    }
    |> List.ofSeq

let getElevation (c: char) =
    match c with
    | 'S' -> int 'a'
    | 'E' -> int 'z'
    | _ -> int c

let getNeighbors (pos: Position) (rows: int) (cols: int) =
    [
        {
            Row =
                pos.Row
                - 1
            Col = pos.Col
        }
        {
            Row =
                pos.Row
                + 1
            Col = pos.Col
        }
        {
            Row = pos.Row
            Col =
                pos.Col
                - 1
        }
        {
            Row = pos.Row
            Col =
                pos.Col
                + 1
        }
    ]
    |> List.filter (fun p ->
        p.Row >= 0
        && p.Row < rows
        && p.Col >= 0
        && p.Col < cols
    )

let bfs (grid: char[][]) (start: Position) (target: Position) =
    let rows = Array.length grid
    let cols = Array.length grid.[0]

    let queue = Queue<Position * int>()
    let visited = HashSet<Position>()

    queue.Enqueue((start, 0))

    visited.Add(start)
    |> ignore

    let rec search () =
        if queue.Count = 0 then
            None
        else
            let pos, dist = queue.Dequeue()

            if pos = target then
                Some dist
            else
                let currentElevation = getElevation grid.[pos.Row].[pos.Col]

                for neighbor in getNeighbors pos rows cols do
                    if not (visited.Contains neighbor) then
                        let neighborElevation = getElevation grid.[neighbor.Row].[neighbor.Col]

                        if
                            neighborElevation
                            <= currentElevation
                               + 1
                        then
                            visited.Add(neighbor)
                            |> ignore

                            queue.Enqueue((neighbor, dist + 1))

                search ()

    search ()

let part1 (input: string) =
    let grid = parseGrid input
    let start = findPosition grid 'S'
    let target = findPosition grid 'E'

    match bfs grid start target with
    | Some dist -> dist
    | None -> -1

let part2 (input: string) =
    let grid = parseGrid input
    let target = findPosition grid 'E'

    let startPositions =
        (findAllPositions grid 'a')
        @ (findAllPositions grid 'S')

    startPositions
    |> List.choose (fun start -> bfs grid start target)
    |> List.min
