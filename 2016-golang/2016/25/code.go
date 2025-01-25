package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

func exec(registry map[string]int, instructions []string, cancel chan bool, out chan int) map[string]int {
	pos := 0
	for pos < len(instructions) {
		select {
		case <-cancel:
			return registry
		default:
		}
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
		case "out":
			out <- registry[parts[1]]
		}
		pos++
	}
	return registry
}

func run(part2 bool, input string) any {
	if part2 {
		return "click the button"
	}

outer:
	for i := 0; ; i++ {
		registry := map[string]int{"a": i}
		instructions := strings.Split(input, "\n")
		cancel := make(chan bool)
		out := make(chan int, 1000)
		go exec(registry, instructions, cancel, out)

		prev := 0
		for j := 0; j < 10; j++ {
			val := <-out
			if j > 0 && val == prev {
				cancel <- true
				continue outer
			}
			prev = val
		}
		cancel <- true
		return i
	}
}
