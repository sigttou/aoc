module Day24

open System.Collections.Generic

let parse (input: string) =
    input.Split('\n')
    |> Array.filter (fun s -> s.Length > 0)

let nextPositions (r, c) = [
    (r, c - 1)
    (r - 1, c)
    (r + 1, c)
    (r, c + 1)
    (r, c)
]

let walk (grid: string array) startRow startCol goalRow goalCol baseTime =
    let valleyHeight =
        grid.Length
        - 2

    let valleyWidth =
        grid.[0].Length
        - 2

    let rows = grid.Length
    let cols = grid.[0].Length

    let cycle =
        valleyHeight
        * valleyWidth

    let upPos a t size =
        let p =
            a
            + ((t + 1) % size)

        if p > size then p - size else p

    let downPos a t size =
        let p =
            a
            - ((t + 1) % size)

        if p < 1 then p + size else p

    let isValidAndEmpty visited t (r, c) =
        r >= 0
        && r < rows
        && c >= 0
        && c < cols
        && grid.[r].[c]
           <> '#'
        && grid.[downPos r t valleyHeight].[c]
           <> 'v'
        && grid.[upPos r t valleyHeight].[c]
           <> '^'
        && grid.[r].[downPos c t valleyWidth]
           <> '>'
        && grid.[r].[upPos c t valleyWidth]
           <> '<'
        && not (Set.contains (r, c, (t + 1) % cycle) visited)

    let queue = Queue<int * int * int>()
    queue.Enqueue(startRow, startCol, baseTime)

    let rec loop visited =
        if queue.Count = 0 then
            -1
        else
            let (r, c, t) = queue.Dequeue()

            if (r, c) = (goalRow, goalCol) then
                t
            else
                let mutable newVisited = visited

                for (nr, nc) in nextPositions (r, c) do
                    if isValidAndEmpty newVisited t (nr, nc) then
                        queue.Enqueue(nr, nc, t + 1)
                        newVisited <- Set.add (nr, nc, (t + 1) % cycle) newVisited

                loop newVisited

    loop Set.empty

let part1 (input: string) =
    let grid = parse input
    let startRow = 0
    let startCol = 1

    let goalRow =
        grid.Length
        - 1

    let goalCol =
        grid.[0].Length
        - 2

    walk grid startRow startCol goalRow goalCol 0

let part2 (input: string) =
    let grid = parse input
    let startRow = 0
    let startCol = 1

    let goalRow =
        grid.Length
        - 1

    let goalCol =
        grid.[0].Length
        - 2

    let time1 = walk grid startRow startCol goalRow goalCol 0

    let time2 = walk grid goalRow goalCol startRow startCol time1

    let time3 = walk grid startRow startCol goalRow goalCol time2

    time3
