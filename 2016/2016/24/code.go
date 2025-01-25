package main

import (
	"strings"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

type pos struct {
	x, y int
}

func (p pos) add(p2 pos) (sum pos) {
	return pos{p.x + p2.x, p.y + p2.y}
}

func (p pos) get_neighbors(walls map[pos]bool) []pos {
	to_chk := []pos{{1, 0}, {0, 1}, {0, -1}, {-1, 0}}

	neighbors := make([]pos, 0, 4)
	for _, chk := range to_chk {
		tmp := p.add(chk)
		if !walls[tmp] {
			neighbors = append(neighbors, tmp)
		}
	}
	return neighbors
}

func parse_input(input string) (walls map[pos]bool, points map[int]pos) {
	walls = make(map[pos]bool)
	points = make(map[int]pos)
	for y, line := range strings.Split(input, "\n") {
		for x, c := range line {
			p := pos{x, y}
			switch c {
			case '#':
				walls[p] = true
			case '.':
				// do nothing
			default:
				points[int(c-'0')] = p
			}
		}
	}
	return walls, points
}

func get_paths(destinations []int) [][]int {
	paths := make([][]int, 0)

	for _, d := range destinations {
		others := make([]int, 0, len(destinations)-1)
		for _, o := range destinations {
			if o != d {
				others = append(others, o)
			}
		}
		combination := []int{d}
		if len(others) > 0 {
			for _, p := range get_paths(others) {
				paths = append(paths, append(combination, p...))
			}
		} else {
			paths = append(paths, combination)
		}
	}

	return paths
}

func get_shortest_path(walls map[pos]bool, start pos, dest pos) []pos {
	visited := make(map[pos]bool)
	queue := []pos{start}
	paths := make(map[pos][]pos)
	paths[start] = []pos{start}
	for len(queue) > 0 {
		curr := queue[0]
		queue = queue[1:]
		if curr == dest {
			break
		}
		for _, n := range curr.get_neighbors(walls) {
			if !visited[n] {
				visited[n] = true
				queue = append(queue, n)
				paths[n] = append(paths[curr], n)
			}
		}
	}
	if _, found := paths[dest]; !found {
		return nil // No valid path found
	}
	return paths[dest]
}

func get_shortest_travel(walls map[pos]bool, points map[int]pos, p2 bool) int {
	start := points[0]
	destinations := make([]int, 0, len(points))
	for k := range points {
		if k != 0 {
			destinations = append(destinations, k)
		}
	}

	paths := get_paths(destinations)
	lengths := make(chan int, len(paths))
	for _, path := range paths {
		tmp := make([]int, len(path))
		copy(tmp, path)
		go func() {
			length := 0
			for i := range tmp {
				from := start
				if i > 0 {
					from = points[tmp[i-1]]
				}
				to := points[tmp[i]]
				length += len(get_shortest_path(walls, from, to)) - 1
			}
			if p2 {
				length += len(get_shortest_path(walls, points[tmp[len(tmp)-1]], start)) - 1
			}
			lengths <- length
		}()
	}

	steps := 1000000000000000000
	for range paths {
		length := <-lengths
		if length < steps {
			steps = length
		}
	}
	return steps
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	walls, points := parse_input(input)

	return get_shortest_travel(walls, points, part2)
}
