module Day09

open System

type Position = { X: int; Y: int }

let parseMotion (line: string) =
    let parts = line.Split(' ')
    let direction = parts.[0]
    let steps = int parts.[1]
    direction, steps

let move (pos: Position) (direction: string) =
    match direction with
    | "R" -> { pos with X = pos.X + 1 }
    | "L" -> { pos with X = pos.X - 1 }
    | "U" -> { pos with Y = pos.Y + 1 }
    | "D" -> { pos with Y = pos.Y - 1 }
    | _ -> pos

let isTouching (head: Position) (tail: Position) =
    abs (
        head.X
        - tail.X
    )
    <= 1
    && abs (
        head.Y
        - tail.Y
       )
       <= 1

let updateTail (head: Position) (tail: Position) =
    if isTouching head tail then
        tail
    else
        let dx =
            head.X
            - tail.X

        let dy =
            head.Y
            - tail.Y

        {
            X =
                tail.X
                + sign dx
            Y =
                tail.Y
                + sign dy
        }

let simulateRope (ropeLength: int) (motions: (string * int) list) =
    let mutable rope = Array.create ropeLength { X = 0; Y = 0 }

    let mutable visited =
        Set.singleton
            rope.[ropeLength
                  - 1]

    for direction, steps in motions do
        for _ in 1..steps do
            rope.[0] <- move rope.[0] direction

            for i in
                1 .. (ropeLength
                      - 1) do
                rope.[i] <- updateTail rope.[i - 1] rope.[i]

            visited <-
                Set.add
                    rope.[ropeLength
                          - 1]
                    visited

    Set.count visited

let part1 (input: string) =
    let motions =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseMotion
        |> List.ofArray

    simulateRope 2 motions

let part2 (input: string) =
    let motions =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseMotion
        |> List.ofArray

    simulateRope 10 motions
