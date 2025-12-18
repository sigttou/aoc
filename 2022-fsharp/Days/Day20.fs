module Day20

open System

let parseNumbers (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map int64
    |> Array.mapi (fun i v -> (i, v))
    |> Array.toList

let mix (numbers: (int * int64) list) =
    let mutable current = numbers
    let length = List.length numbers

    for originalIndex in 0 .. (length - 1) do
        let currentPos =
            current
            |> List.findIndex (fun (idx, _) -> idx = originalIndex)

        let (_, value) = current.[currentPos]

        current <- List.removeAt currentPos current

        let newPos =
            if length = 1 then
                0
            else
                let pos =
                    (int64 currentPos
                     + value) % int64 (length - 1)

                let pos =
                    if pos < 0L then
                        pos
                        + int64 (length - 1)
                    else
                        pos

                int pos

        current <- List.insertAt newPos (originalIndex, value) current

    current

let findGroveCoordinates (numbers: (int * int64) list) =
    let zeroPos =
        numbers
        |> List.findIndex (fun (_, v) -> v = 0L)

    let length = List.length numbers

    let getValue offset =
        let pos =
            (zeroPos
             + offset) % length

        let (_, value) = numbers.[pos]
        value

    getValue 1000
    + getValue 2000
    + getValue 3000

let part1 (input: string) =
    let numbers = parseNumbers input
    let mixed = mix numbers
    findGroveCoordinates mixed

let part2 (input: string) =
    let decryptionKey = 811589153L

    let numbers =
        parseNumbers input
        |> List.map (fun (idx, v) ->
            (idx,
             v
             * decryptionKey)
        )

    let mutable mixed = numbers

    for _ in 1..10 do
        mixed <- mix mixed

    findGroveCoordinates mixed
