open System
open System.IO
open System.Reflection

let runDay day isTest =
    let sufix = if isTest then "_test" else ""
    let inputFile = Path.Combine("input", $"day{day:D2}{sufix}.txt")

    if not (File.Exists inputFile) then
        printfn $"Error: Input file '{inputFile}' not found"
        exit 1

    let input = File.ReadAllText inputFile

    let moduleName = $"Day{day:D2}"
    let assembly = Assembly.GetExecutingAssembly()
    let moduleType = assembly.GetType(moduleName)

    match moduleType with
    | null ->
        printfn $"Day {day} not implemented yet (module '{moduleName}' not found)"
        exit 1
    | t ->
        let part1Method = t.GetMethod("part1")
        let part2Method = t.GetMethod("part2")

        match part1Method, part2Method with
        | null, _
        | _, null ->
            printfn $"Day {day} is missing part1 or part2 function"
            exit 1
        | p1, p2 ->
            let result1 = p1.Invoke(null, [| box input |])
            let result2 = p2.Invoke(null, [| box input |])
            printfn "Part 1: %A" result1
            printfn "Part 2: %A" result2

[<EntryPoint>]
let main args =
    match args with
    | [| dayStr |] ->
        match Int32.TryParse dayStr with
        | true, day -> runDay day false
        | false, _ ->
            printfn "Usage: dotnet run <day> [--test]"
            exit 1
    | [| dayStr; "--test" |] ->
        match Int32.TryParse dayStr with
        | true, day -> runDay day true
        | false, _ ->
            printfn "Usage: dotnet run <day> [--test]"
            exit 1
    | _ ->
        printfn "Usage: dotnet run <day> [--test]"
        exit 1

    0
