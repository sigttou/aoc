package main

import (
	"fmt"
	"strings"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

type IpRange struct {
	low  int
	high int
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	blacklist := []IpRange{}
	for _, line := range strings.Split(input, "\n") {
		low := 0
		high := 0
		_, err := fmt.Sscanf(line, "%d-%d", &low, &high)
		if err != nil {
			fmt.Printf("Error parsing line: %s\n", line)
			continue
		}
		blacklist = append(blacklist, IpRange{low, high})
	}

	cnt := 0
	for i := 0; i < 4294967295; i++ {
		allowed := true
		for _, range_ := range blacklist {
			if i >= range_.low && i <= range_.high {
				allowed = false
				break
			}
		}
		if allowed {
			cnt++
			if !part2 {
				return i
			}
		}
	}

	return cnt
}
