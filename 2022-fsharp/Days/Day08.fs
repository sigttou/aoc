module Day08

open System

let parseGrid (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line ->
        line.ToCharArray()
        |> Array.map (fun c ->
            int c
            - int '0'
        )
    )

let isVisibleFromDirection (grid: int[][]) (row: int) (col: int) (dRow: int) (dCol: int) =
    let height = grid.[row].[col]
    let rows = Array.length grid
    let cols = Array.length grid.[0]

    let rec check r c =
        let newR = r + dRow
        let newC = c + dCol

        if
            newR < 0
            || newR
               >= rows
            || newC < 0
            || newC
               >= cols
        then
            true // Reached edge, so visible
        elif
            grid.[newR].[newC]
            >= height
        then
            false // Blocked by taller or equal tree
        else
            check newR newC

    check row col

let isVisible (grid: int[][]) (row: int) (col: int) =
    isVisibleFromDirection grid row col (-1) 0
    || isVisibleFromDirection grid row col 1 0
    || isVisibleFromDirection grid row col 0 (-1)
    || isVisibleFromDirection grid row col 0 1

let viewingDistance (grid: int[][]) (row: int) (col: int) (dRow: int) (dCol: int) =
    let height = grid.[row].[col]
    let rows = Array.length grid
    let cols = Array.length grid.[0]

    let rec count r c distance =
        let newR = r + dRow
        let newC = c + dCol

        if
            newR < 0
            || newR
               >= rows
            || newC < 0
            || newC
               >= cols
        then
            distance // Reached edge
        elif
            grid.[newR].[newC]
            >= height
        then
            distance
            + 1 // Can see this tree but it blocks further view
        else
            count
                newR
                newC
                (distance
                 + 1)

    count row col 0

let scenicScore (grid: int[][]) (row: int) (col: int) =
    let up = viewingDistance grid row col (-1) 0
    let down = viewingDistance grid row col 1 0
    let left = viewingDistance grid row col 0 (-1)
    let right = viewingDistance grid row col 0 1

    up
    * down
    * left
    * right

let part1 (input: string) =
    let grid = parseGrid input
    let rows = Array.length grid
    let cols = Array.length grid.[0]

    seq {
        for row in 0 .. (rows - 1) do
            for col in 0 .. (cols - 1) do
                if isVisible grid row col then
                    yield 1
    }
    |> Seq.sum

let part2 (input: string) =
    let grid = parseGrid input
    let rows = Array.length grid
    let cols = Array.length grid.[0]

    seq {
        for row in 0 .. (rows - 1) do
            for col in 0 .. (cols - 1) do
                yield scenicScore grid row col
    }
    |> Seq.max
