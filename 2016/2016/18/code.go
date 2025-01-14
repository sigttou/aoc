package main

import (
	"strings"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	row_cnt := 40
	if part2 {
		row_cnt = 400000
	}

	last_row := "." + input + "."
	patterns := "^^. .^^ ^.. ..^"

	var rows []string
	for len(rows) < row_cnt {
		rows = append(rows, last_row[1:len(last_row)-1])
		nxt_row := "."
		for i := 1; i < len(last_row)-1; i++ {
			if strings.Contains(patterns, last_row[i-1:i+2]) {
				nxt_row += "^"
			} else {
				nxt_row += "."
			}
		}
		last_row = nxt_row + "."
	}

	safe_tiles := 0
	for _, row := range rows {
		for _, c := range row {
			if c == '.' {
				safe_tiles++
			}
		}
	}

	return safe_tiles
}
