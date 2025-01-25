package main

import (
	"crypto/md5"
	"encoding/hex"
	"fmt"
	"os"
)

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

type pos struct {
	x, y int
	path string
}

func get_moves(p pos, input string) (moves []string) {
	hash := get_hash(input + p.path)
	for i, dir := range []string{"U", "D", "L", "R"} {
		if hash[i] > 'a' {
			moves = append(moves, dir)
		}
	}
	return moves
}

func move(p pos, dir string) (pos, bool) {
	valid := false
	switch {
	case dir == "U" && p.y > 0:
		p.y--
		p.path += "U"
		valid = true
	case dir == "D" && p.y < 3:
		p.y++
		p.path += "D"
		valid = true
	case dir == "L" && p.x > 0:
		p.x--
		p.path += "L"
		valid = true
	case dir == "R" && p.x < 3:
		p.x++
		p.path += "R"
		valid = true
	}
	return p, valid
}

func is_end(p pos) bool {
	return p.x == 3 && p.y == 3
}

func get_hash(s string) string {
	hash := md5.Sum([]byte(s))
	return hex.EncodeToString(hash[:])
}

func get_short_long_paths(input string) (string, string) {
	start := pos{0, 0, ""}
	shortest := ""
	longest := ""
	queue := []pos{start}

	for len(queue) > 0 {
		p := queue[0]
		queue = queue[1:]

		if is_end(p) {
			if shortest == "" {
				shortest = p.path
			}
			longest = p.path
			continue
		}

		for _, dir := range get_moves(p, input) {
			p2, valid := move(p, dir)
			if valid {
				queue = append(queue, p2)
			}
		}
	}
	return shortest, longest
}

func run(part2 bool, input string) any {
	shortest, longest := get_short_long_paths(input)

	if part2 {
		return len(longest)
	}
	return shortest
}
