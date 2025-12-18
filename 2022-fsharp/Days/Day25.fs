module Day25

let snafuToDecimal (snafu: string) =
    snafu
    |> Seq.rev
    |> Seq.mapi (fun i c ->
        let digit =
            match c with
            | '=' -> -2L
            | '-' -> -1L
            | '0' -> 0L
            | '1' -> 1L
            | '2' -> 2L
            | _ -> failwith $"Invalid SNAFU digit: {c}"

        digit
        * (pown 5L i)
    )
    |> Seq.sum

let decimalToSnafu (n: int64) =
    let rec convert acc n =
        if n = 0L then
            if acc = "" then "0" else acc
        else
            let remainder = n % 5L
            let quotient = n / 5L

            match remainder with
            | 0L
            | 1L
            | 2L ->
                convert
                    (string remainder
                     + acc)
                    quotient
            | 3L ->
                convert
                    ("=" + acc)
                    (quotient
                     + 1L)
            | 4L ->
                convert
                    ("-" + acc)
                    (quotient
                     + 1L)
            | _ -> failwith "Unexpected remainder"

    convert "" n

let part1 (input: string) =
    input.Split('\n')
    |> Array.filter (fun s -> s.Length > 0)
    |> Array.map snafuToDecimal
    |> Array.sum
    |> decimalToSnafu

let part2 (input: string) = "Just press the button!"
