package main

import (
	"strconv"
	"strings"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

func run_code(registers map[string]int, instructions []string) {
	n := 0
	for n < len(instructions) {
		if n < 0 {
			n = 0
		}
		words := strings.Split(instructions[n], " ")
		switch words[0] {
		case "cpy":
			x, err := strconv.Atoi(words[1])
			if err != nil {
				x = registers[words[1]]
			}
			registers[words[2]] = x
		case "inc":
			registers[words[1]]++
		case "dec":
			registers[words[1]]--
		case "jnz":
			x, err := strconv.Atoi(words[1])
			if err != nil {
				x = registers[words[1]]
			}
			y, err := strconv.Atoi(words[2])
			if err != nil {
				y = registers[words[2]]
			}
			if x != 0 {
				n += y - 1 // because n++ at the end
			}
		}
		n++
	}
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	instructions := strings.Split(input, "\n")
	registers := make(map[string]int)
	// when you're ready to do part 2, remove this "not implemented" block
	if part2 {
		registers["c"] = 1
	}

	run_code(registers, instructions)

	return registers["a"]
}
