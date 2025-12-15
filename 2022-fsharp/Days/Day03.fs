module Day03

open System

let priority (c: char) =
    if c >= 'a' && c <= 'z' then
        int c - int 'a' + 1
    else
        int c - int 'A' + 27

let findCommonItem (compartment1: string) (compartment2: string) =
    let set1 = Set.ofSeq compartment1
    let set2 = Set.ofSeq compartment2
    Set.intersect set1 set2 |> Set.minElement

let findBadge (rucksacks: string list) =
    rucksacks
    |> List.map Set.ofSeq
    |> List.reduce Set.intersect
    |> Set.minElement

let part1 (input: string) =
    input.Split([|'\n'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.sumBy (fun rucksack ->
        let mid = rucksack.Length / 2
        let compartment1 = rucksack.Substring(0, mid)
        let compartment2 = rucksack.Substring(mid)
        let common = findCommonItem compartment1 compartment2
        priority common)

let part2 (input: string) =
    input.Split([|'\n'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.chunkBySize 3
    |> Array.sumBy (fun group ->
        let badge = findBadge (Array.toList group)
        priority badge)