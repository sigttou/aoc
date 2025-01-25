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

type pos struct {
	x, y int
}

func (p pos) add(p2 pos) (sum pos) {
	return pos{p.x + p2.x, p.y + p2.y}
}

func (p pos) is_in_bounds(x_mx, y_mx int) bool {
	return p.x >= 0 && p.y >= 0 && p.x <= x_mx && p.y <= y_mx
}

func (p pos) get_pos_moves(x_mx, y_mx int) []pos {
	moves := []pos{{1, 0}, {0, 1},
		{0, -1}, {-1, 0}}
	valid := make([]pos, 0, 4)
	for _, m := range moves {
		new := p.add(m)
		if new.is_in_bounds(x_mx, y_mx) {
			valid = append(valid, new)
		}
	}
	return valid
}

type node struct {
	size, used, avail int
}

type grid map[pos]node

func (g grid) getLimits() (x, y int) {
	for p := range g {
		if p.x > x {
			x = p.x
		}
		if p.y > y {
			y = p.y
		}
	}
	return
}

func (g grid) get_start() pos {
	for p, n := range g {
		if n.used == 0 {
			return p
		}
	}
	return pos{}
}

func (g grid) get_illegal_pos() map[pos]bool {
	illegal := make(map[pos]bool)
	smallest := 0
	for _, n := range g {
		if smallest == 0 || n.size < smallest {
			smallest = n.size
		}
	}
	// find nodes that don't fit on the smallest node.
	for p, n := range g {
		if n.used > smallest {
			illegal[p] = true
		}
	}
	return illegal
}

func (g grid) get_viable_pairs_cnt() int {
	cnt := 0
	for p1, n1 := range g {
		for p2, n2 := range g {
			if p1 != p2 && n1.used != 0 && n1.used <= n2.avail {
				cnt++
			}
		}
	}
	return cnt
}

func (g grid) get_shortest_path(from, to pos, illegal map[pos]bool) []pos {
	x_mx, y_mx := g.getLimits()
	// map with current positions and the path they have taken.
	status := map[pos][]pos{from: {}}
	visited := map[pos]bool{from: true}
	for {
		nxt_status := map[pos][]pos{}
		for cur_pos, path := range status {
			nxt_pos := cur_pos.get_pos_moves(x_mx, y_mx)
			for _, nxt := range nxt_pos {
				p := make([]pos, len(path))
				copy(p, path)
				p = append(p, nxt)
				if nxt == to {
					return p
				}
				if !visited[nxt] && !illegal[nxt] {
					visited[nxt] = true
					nxt_status[nxt] = p
				}
			}
		}
		status = nxt_status
	}
}

func (g grid) get_min_steps_to_goal() int {
	xmax, _ := g.getLimits()
	start := g.get_start()
	illegal := g.get_illegal_pos()
	steps := len(g.get_shortest_path(start, pos{xmax - 1, 0}, illegal))
	target_path := g.get_shortest_path(pos{xmax, 0}, pos{0, 0}, illegal)

	target := pos{xmax, 0}
	for i := 0; i < len(target_path); i++ {
		steps++

		if i < len(target_path)-1 {
			tmp_illegal := map[pos]bool{target_path[i]: true}
			for k, v := range illegal {
				tmp_illegal[k] = v
			}

			empty_path := g.get_shortest_path(target, target_path[i+1], tmp_illegal)
			steps += len(empty_path)
			target = target_path[i]
		}
	}
	return steps
}

func IgnoreError[T any](val T, err error) T {
	return val
}

func gen_grid(input string) grid {
	g := make(grid)
	for _, line := range strings.Split(input, "\n")[2:] {
		var x, y, size, used, avail int
		IgnoreError(fmt.Sscanf(line, "/dev/grid/node-x%d-y%d %dT %dT %dT %d%%", &x, &y, &size, &used, &avail))
		g[pos{x, y}] = node{size, used, avail}
	}
	return g
}

func run(part2 bool, input string) any {
	grid := gen_grid(input)

	if part2 {
		return grid.get_min_steps_to_goal()
	}

	return grid.get_viable_pairs_cnt()
}
