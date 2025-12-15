module Day05

open System
open System.Text.RegularExpressions

type Instruction = { Count: int; From: int; To: int }

let parseStacks (lines: string[]) =
    let numbersLine =
        lines
        |> Array.findIndex (fun line -> line.Contains("1"))

    let stackLines =
        lines.[0 .. numbersLine
                    - 1]

    let numStacks =
        (lines.[numbersLine].Trim().Split([| ' ' |], StringSplitOptions.RemoveEmptyEntries)
         |> Array.length)

    let stacks = Array.init numStacks (fun _ -> ResizeArray<char>())

    for i in
        (numbersLine
         - 1) .. -1 .. 0 do
        let line = stackLines.[i]

        for stackIdx in
            0 .. (numStacks
                  - 1) do
            let charPos =
                1
                + stackIdx
                  * 4

            if
                charPos < line.Length
                && line.[charPos]
                   <> ' '
            then
                stacks.[stackIdx].Add(line.[charPos])

    stacks

let parseInstruction (line: string) =
    let m = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)")

    {
        Count = int m.Groups.[1].Value
        From =
            int m.Groups.[2].Value
            - 1
        To =
            int m.Groups.[3].Value
            - 1
    }

let parseInput (input: string) =
    let sections = input.Split([| "\n\n" |], StringSplitOptions.None)
    let stackLines = sections.[0].Split('\n')

    let instructionLines =
        sections.[1].Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)

    let stacks = parseStacks stackLines

    let instructions =
        instructionLines
        |> Array.map parseInstruction

    stacks, instructions

let executeInstructionPart1 (stacks: ResizeArray<char>[]) (instr: Instruction) =
    for _ in 1 .. instr.Count do
        let crate =
            stacks.[instr.From].[stacks.[instr.From].Count
                                 - 1]

        stacks.[instr.From]
            .RemoveAt(
                stacks.[instr.From].Count
                - 1
            )

        stacks.[instr.To].Add(crate)

let executeInstructionPart2 (stacks: ResizeArray<char>[]) (instr: Instruction) =
    let startIdx =
        stacks.[instr.From].Count
        - instr.Count

    let crates = stacks.[instr.From].GetRange(startIdx, instr.Count)
    stacks.[instr.From].RemoveRange(startIdx, instr.Count)
    stacks.[instr.To].AddRange(crates)

let getTopCrates (stacks: ResizeArray<char>[]) =
    stacks
    |> Array.map (fun stack ->
        if stack.Count > 0 then
            stack.[stack.Count
                   - 1]
        else
            ' '
    )
    |> String

let part1 (input: string) =
    let stacks, instructions = parseInput input

    instructions
    |> Array.iter (executeInstructionPart1 stacks)

    getTopCrates stacks

let part2 (input: string) =
    let stacks, instructions = parseInput input

    instructions
    |> Array.iter (executeInstructionPart2 stacks)

    getTopCrates stacks
