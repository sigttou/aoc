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

	var grid [][]string
	for _, line := range strings.Split(input, "\n") {
		grid = append(grid, strings.Split(line, ""))
	}

	var freqMaps []map[string]int
	for col := 0; col < len(grid[0]); col++ {
		freqMaps = append(freqMaps, map[string]int{})
		for row := 0; row < len(grid); row++ {
			freqMaps[col][grid[row][col]]++
		}
	}

	p1 := ""
	p2 := ""
	for col := 0; col < len(freqMaps); col++ {
		mx := 0
		mx_chr := ""
		mn := 999999999999
		mn_chr := ""
		for k, cnt := range freqMaps[col] {
			if cnt > mx {
				mx = cnt
				mx_chr = k
			}
			if cnt < mn {
				mn = cnt
				mn_chr = k
			}
		}
		p1 += mx_chr
		p2 += mn_chr
	}

	// when you're ready to do part 2, remove this "not implemented" block
	if part2 {
		return p2
	}
	// solve part 1 here
	return p1
}
