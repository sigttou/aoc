module Day23

type Direction =
    | N
    | S
    | W
    | E

let parse (input: string) =
    input.Split('\n')
    |> Array.mapi (fun y line ->
        line
        |> Seq.mapi (fun x c -> if c = '#' then Some(x, y) else None)
        |> Seq.choose id
    )
    |> Seq.concat
    |> Set.ofSeq

let adjacent (x, y) = [
    (x - 1, y - 1)
    (x, y - 1)
    (x + 1, y - 1) // NW, N, NE
    (x - 1, y)
    (x + 1, y) // W, E
    (x - 1, y + 1)
    (x, y + 1)
    (x + 1, y + 1) // SW, S, SE
]

let checkDirection dir (x, y) elves =
    let positions =
        match dir with
        | N -> [
            (x - 1, y - 1)
            (x, y - 1)
            (x + 1, y - 1)
          ] // NW, N, NE
        | S -> [
            (x - 1, y + 1)
            (x, y + 1)
            (x + 1, y + 1)
          ] // SW, S, SE
        | W -> [
            (x - 1, y - 1)
            (x - 1, y)
            (x - 1, y + 1)
          ] // NW, W, SW
        | E -> [
            (x + 1, y - 1)
            (x + 1, y)
            (x + 1, y + 1)
          ] // NE, E, SE

    positions
    |> List.forall (fun pos -> not (Set.contains pos elves))

let move dir (x, y) =
    match dir with
    | N -> (x, y - 1)
    | S -> (x, y + 1)
    | W -> (x - 1, y)
    | E -> (x + 1, y)

let proposeMove elves directions elf =
    let adjPositions = adjacent elf

    if
        adjPositions
        |> List.forall (fun pos -> not (Set.contains pos elves))
    then
        None
    else
        directions
        |> List.tryFind (fun dir -> checkDirection dir elf elves)
        |> Option.map (fun dir -> move dir elf)

let simulateRound elves directions =
    let proposals =
        elves
        |> Set.toList
        |> List.map (fun elf -> (elf, proposeMove elves directions elf))

    let proposalCounts =
        proposals
        |> List.choose (fun (elf, proposal) ->
            proposal
            |> Option.map (fun dest -> dest)
        )
        |> List.groupBy id
        |> List.map (fun (dest, group) -> (dest, List.length group))
        |> Map.ofList

    let newElves =
        proposals
        |> List.map (fun (elf, proposal) ->
            match proposal with
            | None -> elf
            | Some dest ->
                match Map.tryFind dest proposalCounts with
                | Some 1 -> dest
                | _ -> elf
        )
        |> Set.ofList

    let newDirections =
        (List.tail directions)
        @ [ List.head directions ]

    (newElves, newDirections)

let getBounds elves =
    let positions = Set.toList elves

    let xs =
        positions
        |> List.map fst

    let ys =
        positions
        |> List.map snd

    (List.min xs, List.max xs, List.min ys, List.max ys)

let countEmpty elves =
    let (minX, maxX, minY, maxY) = getBounds elves

    let area =
        (maxX
         - minX
         + 1)
        * (maxY
           - minY
           + 1)

    area
    - Set.count elves

let rec simulate rounds elves directions =
    if rounds = 0 then
        elves
    else
        let (newElves, newDirections) = simulateRound elves directions
        simulate (rounds - 1) newElves newDirections

let rec findStableRound round elves directions =
    let (newElves, newDirections) = simulateRound elves directions

    if newElves = elves then
        round
    else
        findStableRound (round + 1) newElves newDirections

let part1 (input: string) =
    let elves = parse input

    let directions = [
        N
        S
        W
        E
    ]

    let finalElves = simulate 10 elves directions
    countEmpty finalElves

let part2 (input: string) =
    let elves = parse input

    let directions = [
        N
        S
        W
        E
    ]

    findStableRound 1 elves directions
