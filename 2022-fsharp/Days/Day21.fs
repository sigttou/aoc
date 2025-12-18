module Day21

open System
open System.Collections.Generic

type Job =
    | Number of int64
    | Operation of string * string * string

let parseMonkey (line: string) =
    let parts = line.Split([| ": " |], StringSplitOptions.None)
    let name = parts.[0]
    let jobParts = parts.[1].Split(' ')

    let job =
        if jobParts.Length = 1 then
            Number(int64 jobParts.[0])
        else
            Operation(jobParts.[0], jobParts.[1], jobParts.[2])

    (name, job)

let parseMonkeys (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map parseMonkey
    |> Map.ofArray

let rec evaluate (monkeys: Map<string, Job>) (cache: Dictionary<string, int64>) (name: string) =
    match cache.TryGetValue(name) with
    | true, value -> value
    | false, _ ->
        let result =
            match monkeys.[name] with
            | Number n -> n
            | Operation(left, op, right) ->
                let leftVal = evaluate monkeys cache left
                let rightVal = evaluate monkeys cache right

                match op with
                | "+" ->
                    leftVal
                    + rightVal
                | "-" ->
                    leftVal
                    - rightVal
                | "*" ->
                    leftVal
                    * rightVal
                | "/" ->
                    leftVal
                    / rightVal
                | _ -> failwith "Unknown operator"

        cache.[name] <- result
        result

let part1 (input: string) =
    let monkeys = parseMonkeys input
    let cache = Dictionary<string, int64>()
    evaluate monkeys cache "root"

let rec containsHuman (monkeys: Map<string, Job>) (cache: Dictionary<string, bool>) (name: string) =
    if name = "humn" then
        true
    else
        match cache.TryGetValue(name) with
        | true, value -> value
        | false, _ ->
            let result =
                match monkeys.[name] with
                | Number _ -> false
                | Operation(left, _, right) ->
                    containsHuman monkeys cache left
                    || containsHuman monkeys cache right

            cache.[name] <- result
            result

let rec solveForHuman
    (monkeys: Map<string, Job>)
    (evalCache: Dictionary<string, int64>)
    (humanCache: Dictionary<string, bool>)
    (name: string)
    (targetValue: int64)
    =
    if name = "humn" then
        targetValue
    else
        match monkeys.[name] with
        | Number _ -> failwith "Should not reach here"
        | Operation(left, op, right) ->
            let leftHasHuman = containsHuman monkeys humanCache left
            let rightHasHuman = containsHuman monkeys humanCache right

            if leftHasHuman then
                let rightVal = evaluate monkeys evalCache right

                let newTarget =
                    match op with
                    | "+" ->
                        targetValue
                        - rightVal
                    | "-" ->
                        targetValue
                        + rightVal
                    | "*" ->
                        targetValue
                        / rightVal
                    | "/" ->
                        targetValue
                        * rightVal
                    | _ -> failwith "Unknown operator"

                solveForHuman monkeys evalCache humanCache left newTarget
            else
                let leftVal = evaluate monkeys evalCache left

                let newTarget =
                    match op with
                    | "+" ->
                        targetValue
                        - leftVal
                    | "-" ->
                        leftVal
                        - targetValue
                    | "*" ->
                        targetValue
                        / leftVal
                    | "/" ->
                        leftVal
                        / targetValue
                    | _ -> failwith "Unknown operator"

                solveForHuman monkeys evalCache humanCache right newTarget

let part2 (input: string) =
    let monkeys = parseMonkeys input
    let evalCache = Dictionary<string, int64>()
    let humanCache = Dictionary<string, bool>()

    let (left, _, right) =
        match monkeys.["root"] with
        | Operation(l, _, r) -> (l, "", r)
        | _ -> failwith "Root should be an operation"

    let leftHasHuman = containsHuman monkeys humanCache left

    if leftHasHuman then
        let rightVal = evaluate monkeys evalCache right
        solveForHuman monkeys evalCache humanCache left rightVal
    else
        let leftVal = evaluate monkeys evalCache left
        solveForHuman monkeys evalCache humanCache right leftVal
