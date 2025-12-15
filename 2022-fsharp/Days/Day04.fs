module Day04

open System

type Range = { Start: int; End: int }

let parseRange (s: string) =
    let parts = s.Split('-')

    {
        Start = int parts.[0]
        End = int parts.[1]
    }

let parseLine (line: string) =
    let parts = line.Split(',')
    parseRange parts.[0], parseRange parts.[1]

let fullyContains (r1: Range) (r2: Range) =
    (r1.Start
     <= r2.Start
     && r1.End
        >= r2.End)
    || (r2.Start
        <= r1.Start
        && r2.End
           >= r1.End)

let overlaps (r1: Range) (r2: Range) =
    r1.Start
    <= r2.End
    && r2.Start
       <= r1.End

let part1 (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.filter (fun line ->
        let r1, r2 = parseLine line
        fullyContains r1 r2
    )
    |> Array.length

let part2 (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.filter (fun line ->
        let r1, r2 = parseLine line
        overlaps r1 r2
    )
    |> Array.length
