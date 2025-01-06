package main

import (
	"fmt"
	"os"
	"sort"
	"strings"
)

type room struct {
	enc_name []string
	id       int
	chksum   string
}

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

func parse_input(input string) (ret []room) {
	for _, line := range strings.Split(input, "\n") {
		r := room{}
		split := strings.Split(line, "-")
		r.enc_name = split[:len(split)-1]
		if _, err := fmt.Sscanf(split[len(split)-1], "%d[%5s]", &r.id, &r.chksum); err != nil {
			fmt.Println("Error parsing line:", err)
			continue
		}
		ret = append(ret, r)
	}

	return ret
}

func is_valid_room(r room) bool {
	letter_count := make(map[rune]int)
	for _, part := range r.enc_name {
		for _, char := range part {
			letter_count[char]++
		}
	}

	type kv struct {
		Key   rune
		Value int
	}

	var sorted []kv
	for k, v := range letter_count {
		sorted = append(sorted, kv{k, v})
	}

	sort.Slice(sorted, func(i, j int) bool {
		if sorted[i].Value == sorted[j].Value {
			return sorted[i].Key < sorted[j].Key
		}
		return sorted[i].Value > sorted[j].Value
	})

	calculated_checksum := ""
	for i := 0; i < 5; i++ {
		calculated_checksum += string(sorted[i].Key)
	}

	return calculated_checksum == r.chksum
}

func CaesarShift(input string, cnt int) string {
	shifted := []rune{}
	for _, char := range input {
		if char >= 'a' && char <= 'z' {
			shifted = append(shifted, 'a'+(char-'a'+rune(cnt))%26)
		} else {
			shifted = append(shifted, char)
		}
	}
	return string(shifted)
}

func run(part2 bool, input string) any {
	rooms := parse_input(input)

	ret := 0
	for _, r := range rooms {
		if is_valid_room(r) {
			ret += r.id
			if part2 {
				if CaesarShift(r.enc_name[0], r.id) == "northpole" {
					return r.id
				}
			}
		}
	}

	return ret
}
