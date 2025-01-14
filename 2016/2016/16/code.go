package main

import (
	"fmt"
	"os"
)

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

func fill_disk(data []int, target_len int) []int {
	for len(data) < target_len {
		data = append(data, 0)
		for i := len(data) - 2; i >= 0; i-- {
			data = append(data, 1-data[i])
		}
	}
	return data[:target_len]
}

func chk_sum(data []int) string {
	for len(data)%2 == 0 {
		new_data := []int{}
		for i := 0; i < len(data); i += 2 {
			if data[i] == data[i+1] {
				new_data = append(new_data, 1)
			} else {
				new_data = append(new_data, 0)
			}
		}
		data = new_data
	}
	res := ""
	for _, d := range data {
		if d == 0 {
			res += "0"
		} else {
			res += "1"
		}
	}
	return res
}

func run(part2 bool, input string) any {
	target_len := 272
	if part2 {
		target_len = 35651584
	}

	data := []int{}
	for _, c := range input {
		if c == '0' {
			data = append(data, 0)
		} else {
			data = append(data, 1)
		}
	}
	return chk_sum(fill_disk(data, target_len))
}
