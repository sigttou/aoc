package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

const height = 6
const width = 50

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

func update_screen(screen *[height][width]bool, command string) {
	args := strings.Fields(command)

	if args[0] == "rect" {
		size := strings.Split(args[1], "x")
		w, _ := strconv.Atoi(size[0])
		h, _ := strconv.Atoi(size[1])

		for row := 0; row < h; row++ {
			for col := 0; col < w; col++ {
				screen[row][col] = true
			}
		}

	} else if args[0] == "rotate" {
		idx, _ := strconv.Atoi(strings.Split(args[2], "=")[1])
		by, _ := strconv.Atoi(args[4])
		if args[1] == "row" {
			for cnt := 0; cnt < by; cnt++ {
				tmp := screen[idx][width-1]
				for i := width - 1; i > 0; i-- {
					screen[idx][i] = screen[idx][i-1]
				}
				screen[idx][0] = tmp
			}
		} else if args[1] == "column" {
			for cnt := 0; cnt < by; cnt++ {
				tmp := screen[height-1][idx]
				for i := height - 1; i > 0; i-- {
					screen[i][idx] = screen[i-1][idx]
				}
				screen[0][idx] = tmp
			}
		}
	}
}

func run(part2 bool, input string) any {
	screen := [height][width]bool{}

	for _, line := range strings.Split(input, "\n") {
		update_screen(&screen, line)
	}

	p1 := 0
	for row := 0; row < height; row++ {
		for col := 0; col < width; col++ {
			if screen[row][col] {
				if part2 {
					fmt.Print("#")
				}
				p1 += 1
			} else {
				if part2 {
					fmt.Print(" ")
				}
			}
		}
		if part2 {
			fmt.Println()
		}
	}

	// when you're ready to do part 2, remove this "not implemented" block
	if part2 {
		return "see above"
	}
	// solve part 1 here
	return p1
}
