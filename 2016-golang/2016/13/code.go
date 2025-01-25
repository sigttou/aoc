package main

import (
	"fmt"
	"strconv"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

var dirs = [][2]int{{-1, 0}, {1, 0}, {0, 1}, {0, -1}}

func bfs(num int, dest [2]int, part2 bool) int {
	queue := [][3]int{[3]int{1, 1, 0}} // x,y, DIST
	visited := map[[2]int]bool{}
	unique_visits := map[[2]int]bool{}

	for len(queue) > 0 {
		front := queue[0]
		queue = queue[1:]

		cur_x, cur_y := front[0], front[1]
		cur_dist := front[2]

		if !part2 && cur_x == dest[0] && cur_y == dest[1] {
			return cur_dist
		}

		if !visited[[2]int{cur_x, cur_y}] {

			if cur_dist <= 50 {
				unique_visits[[2]int{cur_x, cur_y}] = true
			}

			if part2 && cur_dist > 50 {
				break
			}

			for _, diff := range dirs {
				nextX, nextY := cur_x+diff[0], cur_y+diff[1]
				if nextX >= 0 && nextY >= 0 {
					if is_open(nextX, nextY, num) {
						queue = append(queue, [3]int{nextX, nextY, cur_dist + 1})
					}
				}
			}
		}
		visited[[2]int{cur_x, cur_y}] = true
	}

	return len(unique_visits)
}

func is_open(x, y, input int) bool {
	num := x*x + 3*x + 2*x*y + y + y*y + input
	binStr := fmt.Sprintf("%b", num)

	var ones int
	for _, char := range binStr {
		if char == '1' {
			ones++
		}
	}

	return ones%2 == 0
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	fav_num, _ := strconv.Atoi(input)
	target := [2]int{7, 4}
	if fav_num > 1000 {
		target = [2]int{31, 39}
	}

	return bfs(fav_num, target, part2)
}
