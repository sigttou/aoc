.PHONY: build run example bench today fmt

build:
	dune build

run:
	dune exec bin/main.exe

example:
	dune exec bin/main.exe -- --example

bench:
	dune exec bin/main.exe -- --bench

today:
	dune exec bin/main.exe today

fmt:
	dune build @fmt
