module Day18

open System
open System.Collections.Generic

type Position = { X: int; Y: int; Z: int }

let parsePosition (line: string) =
    let parts = line.Split(',')

    {
        X = int parts.[0]
        Y = int parts.[1]
        Z = int parts.[2]
    }

let getNeighbors (pos: Position) = [
    { pos with X = pos.X + 1 }
    { pos with X = pos.X - 1 }
    { pos with Y = pos.Y + 1 }
    { pos with Y = pos.Y - 1 }
    { pos with Z = pos.Z + 1 }
    { pos with Z = pos.Z - 1 }
]

let countExposedSides (cubes: Set<Position>) =
    cubes
    |> Set.toList
    |> List.sumBy (fun cube ->
        getNeighbors cube
        |> List.filter (fun neighbor -> not (cubes.Contains(neighbor)))
        |> List.length
    )

let findBounds (cubes: Set<Position>) =
    let minX =
        cubes
        |> Set.toSeq
        |> Seq.map (fun p -> p.X)
        |> Seq.min

    let maxX =
        cubes
        |> Set.toSeq
        |> Seq.map (fun p -> p.X)
        |> Seq.max

    let minY =
        cubes
        |> Set.toSeq
        |> Seq.map (fun p -> p.Y)
        |> Seq.min

    let maxY =
        cubes
        |> Set.toSeq
        |> Seq.map (fun p -> p.Y)
        |> Seq.max

    let minZ =
        cubes
        |> Set.toSeq
        |> Seq.map (fun p -> p.Z)
        |> Seq.min

    let maxZ =
        cubes
        |> Set.toSeq
        |> Seq.map (fun p -> p.Z)
        |> Seq.max

    (minX - 1, maxX + 1, minY - 1, maxY + 1, minZ - 1, maxZ + 1)

let floodFillExterior (cubes: Set<Position>) =
    let (minX, maxX, minY, maxY, minZ, maxZ) = findBounds cubes

    let isInBounds (pos: Position) =
        pos.X
        >= minX
        && pos.X
           <= maxX
        && pos.Y
           >= minY
        && pos.Y
           <= maxY
        && pos.Z
           >= minZ
        && pos.Z
           <= maxZ

    let queue = Queue<Position>()
    let exterior = HashSet<Position>()

    let start = { X = minX; Y = minY; Z = minZ }
    queue.Enqueue(start)

    exterior.Add(start)
    |> ignore

    while queue.Count > 0 do
        let current = queue.Dequeue()

        for neighbor in getNeighbors current do
            if
                isInBounds neighbor
                && not (cubes.Contains(neighbor))
                && not (exterior.Contains(neighbor))
            then
                exterior.Add(neighbor)
                |> ignore

                queue.Enqueue(neighbor)

    exterior

let countExteriorSurface (cubes: Set<Position>) =
    let exterior = floodFillExterior cubes

    cubes
    |> Set.toList
    |> List.sumBy (fun cube ->
        getNeighbors cube
        |> List.filter (fun neighbor -> exterior.Contains(neighbor))
        |> List.length
    )

let part1 (input: string) =
    let cubes =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parsePosition
        |> Set.ofArray

    countExposedSides cubes

let part2 (input: string) =
    let cubes =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parsePosition
        |> Set.ofArray

    countExteriorSurface cubes
