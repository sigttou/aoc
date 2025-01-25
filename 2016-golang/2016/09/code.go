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

func run(part2 bool, input string) int {
	decompressed := 0

	for i := 0; i < len(input); {
		switch input[i] {
		case '(':
			close_idx := strings.Index(input[i:], ")") + 1 + i

			copy_cnt := 0
			times := 0
			_, err := fmt.Sscanf(input[i:close_idx], "(%dx%d)", &copy_cnt, &times)
			if err != nil {
				fmt.Println("Error parsing input:", err)
				return 0
			}

			substring := input[close_idx : close_idx+copy_cnt]
			sub_len := len(substring)
			if part2 {
				sub_len = run(part2, substring)
			}

			decompressed += sub_len * times
			i = close_idx + copy_cnt
		default:
			decompressed += 1
			i++
		}
	}

	return decompressed
}
