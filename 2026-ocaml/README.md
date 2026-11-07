# 🎄 Advent of Code 2026

Solutions for [Advent of Code 2026](https://adventofcode.com/2026) in [OCaml](https://ocaml.org/).

## Setup

```sh
opam switch create 5.2.0   # if needed
opam install dune ocamlformat utop
```

## Usage

```sh
make run      # run all solved days on real inputs
make today    # run the latest solved day
make example  # run against data/examples/ instead
make bench    # run with averaged timings
make fmt      # dune build @fmt (ocamlformat)
```

Fine-grained via the runner directly:

```sh
dune exec bin/main.exe 3               # only day 3
dune exec bin/main.exe -- 3 --example  # day 3 on example input
dune exec bin/main.exe -- 5 --bench    # day 5 with averaged timing
dune exec bin/main.exe -- scaffold 8   # create lib/day08.ml from template
dune exec bin/main.exe -- fetch 8      # fetch input for day 8 (skips existing files)
```

## Template format

Every day lives in one module: `lib/day{NN}.ml`, exposing this exact shape:

```ocaml
(* --- Day N: <Puzzle Title> --- *)

let parse : string -> model          (* optional, raw input -> domain model *)

val part1 : string -> string         (* raw input -> answer *)
val part2 : string -> string         (* raw input -> answer *)
```

Notes:

- `part1` / `part2` take the **raw input string** and return the answer as a
  `string`, so the runner stays generic.
- `parse` is a plain `let`, not part of the interface — feel free to add
  helpers, extra datatypes or multiple parse variants below it.
- dune scans `lib/day*.ml` and generates `lib/index.ml`, used as day list.
- put your session cookie into `~/.config/aoc/session.cookie`.
- example input must be manually pasted into `data/examples/NN.txt`.
- use `dune exec bin/main.exe -- scaffold NN` to create the day file with
  the template shape and fetch the puzzle input.

## Layout

```
2026-ocaml/
├── dune-project
├── lib/
│   ├── util.ml   # shared helpers + the Util.S module type
│   ├── index.ml  # generated: auto-detected day index
│   └── dayNN.ml  # one module per day
├── bin/main.ml   # CLI runner
└── data/
    ├── examples/ # example inputs shown on the puzzle page
    └── inputs/   # personal puzzle inputs
```
