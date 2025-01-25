package main

import (
	"strconv"
	"strings"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

func exec(registry map[string]int, instructions []string) map[string]int {
	pos := 0
	for pos < len(instructions) {
		parts := strings.Split(instructions[pos], " ")
		switch parts[0] {
		case "inc":
			registry[parts[1]]++
		case "dec":
			registry[parts[1]]--
		case "cpy":
			val, err := strconv.Atoi(parts[1])
			if err != nil {
				val = registry[parts[1]]
			}
			registry[parts[2]] = val
		case "jnz":
			a, err := strconv.Atoi(parts[1])
			if err != nil {
				a = registry[parts[1]]
			}
			b, err := strconv.Atoi(parts[2])
			if err != nil {
				b = registry[parts[2]]
			}
			if a != 0 {
				pos += b - 1
			}
		case "tgl":
			val, err := strconv.Atoi(parts[1])
			if err != nil {
				val = registry[parts[1]]
			}
			if pos+val >= len(instructions) {
				break
			}
			tmp := instructions[pos+val]
			new_instr := ""
			switch len(strings.Split(tmp, " ")) {
			case 2:
				new_instr = "inc"
				if tmp[:3] == "inc" {
					new_instr = "dec"
				}
			case 3:
				new_instr = "jnz"
				if tmp[:3] == "jnz" {
					new_instr = "cpy"
				}
			}
			instructions[pos+val] = new_instr + tmp[3:]
		}
		pos++
	}
	return registry
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	// when you're ready to do part 2, remove this "not implemented" block
	instructions := strings.Split(input, "\n")
	registry := map[string]int{"a": 7}
	if part2 {
		registry["a"] = 12
	}
	// solve part 1 here
	return exec(registry, instructions)["a"]
}
