package main

import (
	"fmt"
	"strings"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

type disc struct {
	pos_cnt int
	start   int
}

func parse(input string) (discs []disc) {
	for _, line := range strings.Split(input, "\n") {
		d := disc{}
		n, err := fmt.Sscanf(line, "Disc #%d has %d positions; at time=0, it is at position %d.",
			&d.pos_cnt, &d.pos_cnt, &d.start)
		if err != nil || n != 3 {
			continue
		}
		discs = append(discs, d)
	}
	return discs
}

func get_pos_at_t(d disc, t int) int {
	return (d.start + t) % d.pos_cnt
}

func get_alignment(discs []disc) int {
loop:
	for t := 1; ; t++ {
		for i, d := range discs {
			if get_pos_at_t(d, t+i) != 0 {
				continue loop
			}
		}
		return t - 1
	}
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	discs := parse(input)
	if part2 {
		discs = append(discs, disc{11, 0})
	}
	return get_alignment(discs)

}
