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

func chk_triangle(t [3]int64) bool {
	return t[0]+t[1] > t[2] && t[1]+t[2] > t[0] && t[0]+t[2] > t[1]
}

func parse_input(input string) (ret [][3]int64) {
	for _, line := range strings.Split(input, "\n") {
		t := [3]int64{}
		for idx, n := range strings.Fields(line) {
			t[idx], _ = strconv.ParseInt(n, 10, 64)
		}
		ret = append(ret, t)
	}
	return ret
}

func get_vert_triangles(triangles [][3]int64) (ret [][3]int64) {
	for i := 0; i < len(triangles); i += 3 {
		for col := 0; col < 3; col++ {
			t := [3]int64{}
			for row := 0; row < 3; row++ {
				t[row] = triangles[row+i][col]
			}
			ret = append(ret, t)
		}
	}
	return ret
}

func run(part2 bool, input string) any {
	ret := 0
	triangles := parse_input(input)
	if part2 {
		triangles = get_vert_triangles(triangles)
	}
	for _, t := range triangles {
		if chk_triangle(t) {
			ret += 1
		}
	}

	return ret
}
