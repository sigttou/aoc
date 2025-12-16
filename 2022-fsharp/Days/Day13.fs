module Day13

open System

type Packet =
    | Number of int
    | List of Packet list

let parsePacket (s: string) =
    let chars =
        s.Trim().ToCharArray()
        |> Array.toList

    let rec parseValue (chars: char list) : Packet * char list =
        match chars with
        | '[' :: rest -> parseList rest []
        | _ -> parseNumber chars ""

    and parseNumber (chars: char list) (acc: string) : Packet * char list =
        match chars with
        | c :: rest when Char.IsDigit(c) ->
            parseNumber
                rest
                (acc
                 + string c)
        | rest -> Number(int acc), rest

    and parseList (chars: char list) (items: Packet list) : Packet * char list =
        match chars with
        | ']' :: rest -> List(List.rev items), rest
        | ',' :: rest -> parseList rest items
        | chars ->
            let item, remaining = parseValue chars

            parseList
                remaining
                (item
                 :: items)

    let result, _ = parseValue chars
    result

let rec compare (left: Packet) (right: Packet) : int =
    match left, right with
    | Number l, Number r ->
        if l < r then -1
        elif l > r then 1
        else 0
    | List l, List r ->
        let rec compareLists left right =
            match left, right with
            | [], [] -> 0
            | [], _ -> -1
            | _, [] -> 1
            | lh :: lt, rh :: rt ->
                let result = compare lh rh
                if result = 0 then compareLists lt rt else result

        compareLists l r
    | Number l, List r -> compare (List [ Number l ]) (List r)
    | List l, Number r -> compare (List l) (List [ Number r ])

let part1 (input: string) =
    let pairs =
        input.Split([| "\n\n" |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun pair ->
            let lines = pair.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
            parsePacket lines.[0], parsePacket lines.[1]
        )

    pairs
    |> Array.mapi (fun i (left, right) -> if compare left right < 0 then i + 1 else 0)
    |> Array.sum

let part2 (input: string) =
    let divider1 = parsePacket "[[2]]"
    let divider2 = parsePacket "[[6]]"

    let allPackets =
        input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parsePacket
        |> Array.toList
        |> fun packets ->
            divider1
            :: divider2
            :: packets
        |> List.sortWith compare

    let index1 =
        (allPackets
         |> List.findIndex ((=) divider1))
        + 1

    let index2 =
        (allPackets
         |> List.findIndex ((=) divider2))
        + 1

    index1
    * index2
