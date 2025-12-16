module Day11

open System
open System.Text.RegularExpressions

type Operation =
    | Add of int64
    | Multiply of int64
    | Square

type Monkey = {
    Items: int64 list
    Operation: Operation
    Divisor: int64
    TrueTarget: int
    FalseTarget: int
    InspectionCount: int64
}

let parseMonkey (text: string) =
    let lines = text.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)

    let items =
        let itemsMatch = Regex.Match(lines.[1], @"Starting items: (.+)")

        itemsMatch.Groups.[1].Value.Split([| ',' |])
        |> Array.map (fun s -> int64 (s.Trim()))
        |> List.ofArray

    let operation =
        let opMatch = Regex.Match(lines.[2], @"Operation: new = old ([+*]) (.+)")
        let op = opMatch.Groups.[1].Value
        let value = opMatch.Groups.[2].Value

        if value = "old" then Square
        elif op = "+" then Add(int64 value)
        else Multiply(int64 value)

    let divisor =
        Regex.Match(lines.[3], @"divisible by (\d+)").Groups.[1].Value
        |> int64

    let trueTarget =
        Regex.Match(lines.[4], @"monkey (\d+)").Groups.[1].Value
        |> int

    let falseTarget =
        Regex.Match(lines.[5], @"monkey (\d+)").Groups.[1].Value
        |> int

    {
        Items = items
        Operation = operation
        Divisor = divisor
        TrueTarget = trueTarget
        FalseTarget = falseTarget
        InspectionCount = 0L
    }

let applyOperation (operation: Operation) (worry: int64) =
    match operation with
    | Add value ->
        worry
        + value
    | Multiply value ->
        worry
        * value
    | Square ->
        worry
        * worry

let simulateRound (monkeys: Monkey[]) (worryDivisor: int64) (modulo: int64 option) =
    for i in
        0 .. (Array.length monkeys
              - 1) do
        let monkey = monkeys.[i]

        for item in monkey.Items do
            let worry = applyOperation monkey.Operation item

            let managedWorry =
                match modulo with
                | Some m -> worry % m
                | None ->
                    worry
                    / worryDivisor

            let target =
                if managedWorry % monkey.Divisor = 0L then
                    monkey.TrueTarget
                else
                    monkey.FalseTarget

            monkeys.[target] <- {
                monkeys.[target] with
                    Items =
                        monkeys.[target].Items
                        @ [ managedWorry ]
            }

        monkeys.[i] <- {
            monkey with
                Items = []
                InspectionCount =
                    monkey.InspectionCount
                    + int64 (List.length monkey.Items)
        }

let calculateMonkeyBusiness (monkeys: Monkey[]) (rounds: int) (worryDivisor: int64) =
    let monkeys = Array.copy monkeys

    let modulo =
        if worryDivisor = 1L then
            Some(
                monkeys
                |> Array.map (fun m -> m.Divisor)
                |> Array.fold (*) 1L
            )
        else
            None

    for _ in 1..rounds do
        simulateRound monkeys worryDivisor modulo

    monkeys
    |> Array.map (fun m -> m.InspectionCount)
    |> Array.sortDescending
    |> Array.take 2
    |> Array.reduce (*)

let part1 (input: string) =
    let monkeys =
        input.Split([| "\n\n" |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseMonkey

    calculateMonkeyBusiness monkeys 20 3L

let part2 (input: string) =
    let monkeys =
        input.Split([| "\n\n" |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseMonkey

    calculateMonkeyBusiness monkeys 10000 1L
