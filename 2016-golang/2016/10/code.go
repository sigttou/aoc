package main

import (
	"fmt"
	"sort"
	"strconv"
	"strings"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

type rule struct {
	id   int
	low  string
	high string
}

func parse_input(input string) (bots map[int][]int, rules []rule) {
	bots = map[int][]int{}
	for _, line := range strings.Split(input, "\n") {
		parts := strings.Fields(line)
		if parts[0] == "bot" {
			id, _ := strconv.Atoi(parts[1])
			r := rule{id: id, low: parts[5] + parts[6], high: parts[10] + parts[11]}
			rules = append(rules, r)
		} else {
			val, _ := strconv.Atoi(parts[1])
			bot, _ := strconv.Atoi(parts[5])
			bots[bot] = append(bots[bot], val)
		}
	}
	return bots, rules
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	// when you're ready to do part 2, remove this "not implemented" block
	bots, rules := parse_input(input)
	p1_low := 17
	p1_high := 61
	if len(input) < 250 {
		p1_low = 2
		p1_high = 5
	}

	outputs := map[int]int{}

	for outputs[1] == 0 {
		for _, r := range rules {
			if len(bots[r.id]) == 2 {
				sort.Ints(bots[r.id])
				low, high := bots[r.id][0], bots[r.id][1]
				if !part2 && low == p1_low && high == p1_high {
					return r.id
				}

				var out_idx, recv int
				if strings.Contains(r.low, "output") {
					_, err := fmt.Sscanf(r.low, "output%d", &out_idx)
					if err != nil {
						panic(err)
					}
					outputs[out_idx] = low
				} else {
					_, err := fmt.Sscanf(r.low, "bot%d", &recv)
					if err != nil {
						panic(err)
					}
					bots[recv] = append(bots[recv], low)
				}
				if strings.Contains(r.high, "output") {
					_, err := fmt.Sscanf(r.high, "output%d", &out_idx)
					if err != nil {
						panic(err)
					}
					outputs[out_idx] = high
				} else {
					_, err := fmt.Sscanf(r.high, "bot%d", &recv)
					if err != nil {
						panic(err)
					}
					bots[recv] = append(bots[recv], high)
				}
				bots[r.id] = []int{}
			}
		}
	}

	return outputs[0] * outputs[1] * outputs[2]
}
