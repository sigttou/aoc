module Day02

open System

type Shape =
    | Rock
    | Paper
    | Scissors

type Outcome =
    | Loss
    | Draw
    | Win

let parseOpponent =
    function
    | "A" -> Rock
    | "B" -> Paper
    | "C" -> Scissors
    | _ -> failwith "Invalid opponent shape"

let parsePlayer =
    function
    | "X" -> Rock
    | "Y" -> Paper
    | "Z" -> Scissors
    | _ -> failwith "Invalid player shape"

let parseOutcome =
    function
    | "X" -> Loss
    | "Y" -> Draw
    | "Z" -> Win
    | _ -> failwith "Invalid outcome"

let shapeScore =
    function
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3

let outcomeScore opponent player =
    match opponent, player with
    | Rock, Paper
    | Paper, Scissors
    | Scissors, Rock -> 6 // Win
    | Rock, Rock
    | Paper, Paper
    | Scissors, Scissors -> 3 // Draw
    | _ -> 0 // Loss

let roundScore opponent player =
    shapeScore player
    + outcomeScore opponent player

let chooseShape opponent outcome =
    match opponent, outcome with
    | _, Draw -> opponent
    | Rock, Win -> Paper
    | Rock, Loss -> Scissors
    | Paper, Win -> Scissors
    | Paper, Loss -> Rock
    | Scissors, Win -> Rock
    | Scissors, Loss -> Paper

let part1 (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.sumBy (fun line ->
        let parts = line.Split(' ')
        let opponent = parseOpponent parts.[0]
        let player = parsePlayer parts.[1]
        roundScore opponent player
    )

let part2 (input: string) =
    input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.sumBy (fun line ->
        let parts = line.Split(' ')
        let opponent = parseOpponent parts.[0]
        let outcome = parseOutcome parts.[1]
        let player = chooseShape opponent outcome
        roundScore opponent player
    )
