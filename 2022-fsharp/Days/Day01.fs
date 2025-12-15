module Day01

open System

let parseElves (input: string) =
    input.Split([| "\n\n" |], StringSplitOptions.None)
    |> Array.map (fun elf ->
        elf.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.sumBy int
    )

let part1 (input: string) =
    parseElves input
    |> Array.max

let part2 (input: string) =
    parseElves input
    |> Array.sortDescending
    |> Array.take 3
    |> Array.sum
