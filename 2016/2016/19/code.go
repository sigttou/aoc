package main

import (
	"math"
	"strconv"

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
	// https://en.wikipedia.org/wiki/Josephus_problem
	val, _ := strconv.Atoi(input)

	if part2 {
		return val - int(math.Pow(3, math.Floor(math.Log(float64(val))/math.Log(3))))
	}
	return (val-int(math.Exp2(math.Floor(math.Log2(float64(val))))))*2 + 1
}
