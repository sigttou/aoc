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

var directions = [][2]int{{-1, 0}, {0, 1}, {1, 0}, {0, -1}} // N, E, S, W

func abs(x int) int {
	if x < 0 {
		return -x
	}
	return x
}

func ManhattanDistance(x1, y1, x2, y2 int) int {
	return abs(x1-x2) + abs(y1-y2)
}

func run(part2 bool, input string) any {
	var direction int
	var row, col int

	visited := make(map[[2]int]bool)

	visited[[2]int{0, 0}] = true

	for _, instr := range strings.Split(input, ", ") {
		var dir string
		var cnt int
		_, err := fmt.Sscanf(instr, "%1s%d", &dir, &cnt)
		if err != nil {
			return "invalid input format"
		}

		if dir == "R" {
			direction = (direction + 1) % 4
		} else if dir == "L" {
			direction = (direction + 3) % 4
		} else {
			return "invalid input format"
		}

		for i := 0; i < cnt; i++ {
			row += directions[direction][0]
			col += directions[direction][1]

			if part2 {
				if visited[[2]int{row, col}] {
					return ManhattanDistance(0, 0, row, col)
				}
				visited[[2]int{row, col}] = true
			}
		}
	}

	return ManhattanDistance(0, 0, row, col)
}
