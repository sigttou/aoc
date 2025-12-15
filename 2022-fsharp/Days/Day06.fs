module Day06

open System

let findMarker (length: int) (input: string) =
    input
    |> Seq.windowed length
    |> Seq.findIndex (fun window ->
        window
        |> Set.ofArray
        |> Set.count = length
    )
    |> (+) length

let part1 (input: string) = findMarker 4 (input.Trim())

let part2 (input: string) = findMarker 14 (input.Trim())
