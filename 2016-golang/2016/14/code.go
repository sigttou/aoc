package main

import (
	"crypto/md5"
	"fmt"
	"os"
	"regexp"
	"strings"
)

func main() {
	input_file := "input-user.txt"
	input, _ := os.ReadFile(input_file)

	fmt.Println(run(false, string(input)))
	fmt.Println((run(true, string(input))))
}

func hash(input string, idx int, cycles int) string {
	ret := fmt.Sprintf("%s%d", input, idx)
	for i := 0; i < cycles; i++ {
		ret = fmt.Sprintf("%x", md5.Sum([]byte(ret)))
	}
	return ret
}

func chk_triple(input string) string {
	for i := 2; i < len(input); i++ {
		if input[i-2] == input[i-1] && input[i-1] == input[i] {
			return string(input[i])
		}
	}
	return ""
}

func run(part2 bool, input string) any {
	cycles := 1
	if part2 {
		cycles += 2016
	}
	hashes := []string{}
	for i := 0; i < 1000; i++ {
		hashes = append(hashes, hash(input, i, cycles))
	}

	var keys []int
	for index := 0; len(keys) < 64; index++ {
		cur_hash := hashes[0]

		hashes = append(hashes, hash(input, index+1000, cycles))
		hashes = hashes[1:]

		if char := chk_triple(cur_hash); char != "" {
			pattern := regexp.MustCompile(fmt.Sprintf("[%s]{5}", char))
			if pattern.MatchString(strings.Join(hashes, ",")) {
				keys = append(keys, index)
			}
		}
	}

	return keys[len(keys)-1]
}
