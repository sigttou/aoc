package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

func swap_pos(s string, x, y int) string {
	tmp := []rune(s)
	tmp[x], tmp[y] = tmp[y], tmp[x]
	return string(tmp)
}

func swap_letter(s string, x, y string) string {
	return swap_pos(s, strings.Index(s, x), strings.Index(s, y))
}

func rot_left(s string, x int) string {
	ret := s
	for i := 0; i < x; i++ {
		ret = ret[1:] + string(ret[0])
	}
	return ret
}

func rot_right(s string, x int) string {
	ret := s
	for i := 0; i < x; i++ {
		ret = string(ret[len(ret)-1]) + ret[:len(ret)-1]
	}
	return ret
}

func rot_based(s string, x string) string {
	pos := strings.Index(s, x)
	if pos >= 4 {
		pos++
	}
	pos++
	return rot_right(s, pos)
}

func de_rot_based(s string, x string) string {
	pos := strings.Index(s, x)
	var steps int
	switch pos {
	case 0:
		steps = 1
	case 1:
		steps = 1
	case 2:
		steps = 6
	case 3:
		steps = 2
	case 4:
		steps = 7
	case 5:
		steps = 3
	case 6:
		steps = 0
	case 7:
		steps = 4
	}
	return rot_left(s, steps)
}

func reverse(s string, x, y int) (ret string) {
	ret = s
	for x < y {
		ret = swap_pos(ret, x, y)
		x++
		y--
	}
	return ret
}

func move(s string, x, y int) (ret string) {
	letter := string(s[x])
	ret = s[:x] + s[x+1:] + letter
	for strings.Index(ret, letter) != y {
		ret = swap_pos(ret, strings.Index(ret, letter), strings.Index(ret, letter)-1)
	}
	return ret
}

func scramble(start string, instructions []string) (ret string) {
	ret = start

	for _, instr := range instructions {
		parts := strings.Split(instr, " ")
		switch parts[0] {
		case "swap":
			switch parts[1] {
			case "position":
				a, _ := strconv.Atoi(parts[2])
				b, _ := strconv.Atoi(parts[5])
				ret = swap_pos(ret, a, b)
			case "letter":
				ret = swap_letter(ret, parts[2], parts[5])
			}
		case "rotate":
			switch parts[1] {
			case "left":
				pos, _ := strconv.Atoi(parts[2])
				ret = rot_left(ret, pos)
			case "right":
				pos, _ := strconv.Atoi(parts[2])
				ret = rot_right(ret, pos)
			case "based":
				ret = rot_based(ret, parts[6])
			}
		case "reverse":
			a, _ := strconv.Atoi(parts[2])
			b, _ := strconv.Atoi(parts[4])
			ret = reverse(ret, a, b)
		case "move":
			a, _ := strconv.Atoi(parts[2])
			b, _ := strconv.Atoi(parts[5])
			ret = move(ret, a, b)
		default:
			panic("Unknown instruction")
		}
	}

	return ret
}

func unscramble(password string, instructions []string) (ret string) {
	ret = password
	for i := len(instructions) - 1; i >= 0; i-- {
		instr := instructions[i]
		parts := strings.Split(instr, " ")
		switch parts[0] {
		case "swap":
			switch parts[1] {
			case "position":
				a, _ := strconv.Atoi(parts[2])
				b, _ := strconv.Atoi(parts[5])
				ret = swap_pos(ret, a, b)
			case "letter":
				ret = swap_letter(ret, parts[2], parts[5])
			}
		case "rotate":
			switch parts[1] {
			case "left":
				pos, _ := strconv.Atoi(parts[2])
				ret = rot_right(ret, pos)
			case "right":
				pos, _ := strconv.Atoi(parts[2])
				ret = rot_left(ret, pos)
			case "based":
				ret = de_rot_based(ret, parts[6])
			}
		case "reverse":
			a, _ := strconv.Atoi(parts[2])
			b, _ := strconv.Atoi(parts[4])
			ret = reverse(ret, a, b)
		case "move":
			a, _ := strconv.Atoi(parts[2])
			b, _ := strconv.Atoi(parts[5])
			ret = move(ret, b, a)
		default:
			panic("Unknown instruction")
		}
	}

	return ret
}

func run(part2 bool, input string) any {
	instructions := strings.Split(input, "\n")

	if part2 {
		password := "fbgdceah"
		return unscramble(password, instructions)
	}

	start := "abcdefgh"
	return scramble(start, instructions)
}
