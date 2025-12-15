module Day07

open System
open System.Collections.Generic

type FileSystemItem =
    | File of name: string * size: int
    | Directory of name: string * items: ResizeArray<FileSystemItem>

let buildFileSystem (input: string) =
    let lines = input.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
    let root = Directory("/", ResizeArray())
    let mutable currentPath = [ root ]

    for line in lines do
        match line.Split(' ') with
        | [| "$"; "cd"; "/" |] -> currentPath <- [ root ]
        | [| "$"; "cd"; ".." |] -> currentPath <- List.tail currentPath
        | [| "$"; "cd"; dirName |] ->
            match List.head currentPath with
            | Directory(_, items) ->
                let subDir =
                    items
                    |> Seq.find (
                        function
                        | Directory(name, _) when name = dirName -> true
                        | _ -> false
                    )

                currentPath <-
                    subDir
                    :: currentPath
            | File _ -> failwith "Cannot cd into a file"
        | [| "$"; "ls" |] -> ()
        | [| "dir"; dirName |] ->
            match List.head currentPath with
            | Directory(_, items) -> items.Add(Directory(dirName, ResizeArray()))
            | File _ -> failwith "Cannot list directory contents of a file"
        | [| size; fileName |] ->
            match List.head currentPath with
            | Directory(_, items) -> items.Add(File(fileName, int size))
            | File _ -> failwith "Cannot add file to a file"
        | _ -> ()

    root

let rec calculateSize =
    function
    | File(_, size) -> size
    | Directory(_, items) ->
        items
        |> Seq.sumBy calculateSize

let rec getAllDirectorySizes (item: FileSystemItem) =
    match item with
    | File _ -> []
    | Directory(_, items) ->
        let currentSize = calculateSize item

        let subDirSizes =
            items
            |> Seq.collect getAllDirectorySizes
            |> List.ofSeq

        currentSize
        :: subDirSizes

let part1 (input: string) =
    let root = buildFileSystem input
    let allSizes = getAllDirectorySizes root

    allSizes
    |> List.filter (fun size ->
        size
        <= 100000
    )
    |> List.sum

let part2 (input: string) =
    let root = buildFileSystem input
    let totalSpace = 70000000
    let requiredSpace = 30000000
    let usedSpace = calculateSize root

    let freeSpace =
        totalSpace
        - usedSpace

    let needToFree =
        requiredSpace
        - freeSpace

    let allSizes = getAllDirectorySizes root

    allSizes
    |> List.filter (fun size ->
        size
        >= needToFree
    )
    |> List.min
