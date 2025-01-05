package main

import (
	"fmt"
	"os"
	"strings"
)

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

var directions = map[string][2]int{"U": {0, -1}, "D": {0, 1}, "L": {-1, 0}, "R": {1, 0}}

func run(part2 bool, input string) any {

	var ret string

	var instructions [][]string
	for _, line := range strings.Split(input, "\n") {
		instructions = append(instructions, strings.Split(line, ""))
	}

	row, col := 2, 2
	pad := [][]string{
		{"", "", "", "", ""},
		{"", "1", "2", "3", ""},
		{"", "4", "5", "6", ""},
		{"", "7", "8", "9", ""},
		{"", "", "", "", ""},
	}

	if part2 {
		row, col = 3, 1
		pad = [][]string{
			{"", "", "", "", "", "", ""},
			{"", "", "", "1", "", "", ""},
			{"", "", "2", "3", "4", "", ""},
			{"", "5", "6", "7", "8", "9", ""},
			{"", "", "A", "B", "C", "", ""},
			{"", "", "", "D", "", "", ""},
			{"", "", "", "", "", "", ""},
		}
	}

	for _, line := range instructions {
		for _, dir := range line {
			d := directions[dir]
			dr := d[1]
			dc := d[0]

			if pad[row+dr][col+dc] != "" {
				row += dr
				col += dc
			}

		}
		ret += pad[row][col]
	}

	return ret
}
