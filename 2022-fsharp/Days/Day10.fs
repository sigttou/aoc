module Day10

open System

type Instruction =
    | Noop
    | Addx of int

let parseInstruction (line: string) =
    match line.Split(' ') with
    | [| "noop" |] -> Noop
    | [| "addx"; value |] -> Addx(int value)
    | _ -> failwith "Invalid instruction"

let executeCycles (instructions: Instruction list) =
    let mutable x = 1
    let mutable cycle = 0
    let mutable history = []

    for instruction in instructions do
        match instruction with
        | Noop ->
            cycle <- cycle + 1

            history <-
                (cycle, x)
                :: history
        | Addx value ->
            cycle <- cycle + 1

            history <-
                (cycle, x)
                :: history

            cycle <- cycle + 1

            history <-
                (cycle, x)
                :: history

            x <- x + value

    List.rev history

let part1 (input: string) =
    let instructions =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseInstruction
        |> List.ofArray

    let history = executeCycles instructions

    [
        20
        60
        100
        140
        180
        220
    ]
    |> List.sumBy (fun targetCycle ->
        let x =
            history
            |> List.find (fun (cycle, _) -> cycle = targetCycle)
            |> snd

        targetCycle
        * x
    )

let part2 (input: string) =
    let instructions =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseInstruction
        |> List.ofArray

    let history = executeCycles instructions

    let screen = Array.init 6 (fun _ -> Array.create 40 '.')

    for (cycle, x) in history do
        let row =
            (cycle - 1)
            / 40

        let col = (cycle - 1) % 40

        if row < 6 then
            if
                abs (col - x)
                <= 1
            then
                screen.[row].[col] <- '#'

    screen
    |> Array.map String
    |> fun lines ->
        "\n"
        + String.concat "\n" lines
