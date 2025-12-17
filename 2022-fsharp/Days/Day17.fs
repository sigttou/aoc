module Day17

open System
open System.Collections.Generic

type Rock = (int * int) list

let rocks = [
    [
        (0, 0)
        (1, 0)
        (2, 0)
        (3, 0)
    ]
    [
        (1, 0)
        (0, 1)
        (1, 1)
        (2, 1)
        (1, 2)
    ]
    [
        (0, 0)
        (1, 0)
        (2, 0)
        (2, 1)
        (2, 2)
    ]
    [
        (0, 0)
        (0, 1)
        (0, 2)
        (0, 3)
    ]
    [
        (0, 0)
        (1, 0)
        (0, 1)
        (1, 1)
    ]
]

let canMove (rock: Rock) (x: int) (y: int) (grid: HashSet<int * int>) =
    rock
    |> List.forall (fun (dx, dy) ->
        let newX = x + dx
        let newY = y + dy

        newX >= 0
        && newX < 7
        && newY >= 0
        && not (grid.Contains((newX, newY)))
    )

let addRock (rock: Rock) (x: int) (y: int) (grid: HashSet<int * int>) =
    for (dx, dy) in rock do
        grid.Add((x + dx, y + dy))
        |> ignore

let simulate (jets: char[]) (targetRocks: int64) =
    let grid = HashSet<int * int>()
    let mutable height = 0
    let mutable jetIndex = 0
    let mutable rockIndex = 0

    let states = Dictionary<int * int * string, int64 * int>()
    let mutable cycleFound = false
    let mutable additionalHeight = 0L
    let mutable rocksDropped = 0L

    while rocksDropped < targetRocks do
        if
            not cycleFound
            && rocksDropped > 0L
        then
            let topRows =
                seq {
                    for dy in 0 .. min 30 (height - 1) do
                        for x in 0..6 do
                            if
                                grid.Contains(
                                    (x,
                                     height
                                     - 1
                                     - dy)
                                )
                            then
                                yield "1"
                            else
                                yield "0"
                }
                |> String.concat ""

            let stateKey = (rockIndex, jetIndex, topRows)

            match states.TryGetValue(stateKey) with
            | true, (prevRocks, prevHeight) ->
                let cycleLength =
                    rocksDropped
                    - prevRocks

                let cycleHeight =
                    height
                    - prevHeight

                let remainingRocks =
                    targetRocks
                    - rocksDropped

                let fullCycles =
                    remainingRocks
                    / cycleLength

                additionalHeight <-
                    fullCycles
                    * int64 cycleHeight

                rocksDropped <-
                    rocksDropped
                    + fullCycles
                      * cycleLength

                cycleFound <- true
            | false, _ -> states.[stateKey] <- (rocksDropped, height)

        if rocksDropped < targetRocks then
            let rock = rocks.[rockIndex]

            rockIndex <-
                (rockIndex
                 + 1) % 5

            let mutable x = 2
            let mutable y = height + 3

            let mutable falling = true

            while falling do
                let jet = jets.[jetIndex]

                jetIndex <-
                    (jetIndex
                     + 1) % jets.Length

                let newX = if jet = '<' then x - 1 else x + 1

                if canMove rock newX y grid then
                    x <- newX

                let newY = y - 1

                if canMove rock x newY grid then
                    y <- newY
                else
                    addRock rock x y grid

                    for (dx, dy) in rock do
                        height <- max height (y + dy + 1)

                    falling <- false

            rocksDropped <-
                rocksDropped
                + 1L

    int64 height
    + additionalHeight

let part1 (input: string) =
    let jets = input.Trim().ToCharArray()
    simulate jets 2022L

let part2 (input: string) =
    let jets = input.Trim().ToCharArray()
    simulate jets 1000000000000L
