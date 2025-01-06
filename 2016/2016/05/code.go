package main

import (
	"crypto/md5"
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

func run(part2 bool, input string) any {
	password := map[int]string{}

	idx := 0
	for i := 0; len(password) < 8; i++ {
		hash := fmt.Sprintf("%x", md5.Sum([]byte(fmt.Sprintf("%s%d", input, i))))
		if strings.HasPrefix(hash, "00000") {
			if !part2 {
				password[idx] = hash[5:6]
				idx++
			} else {
				idx, err := strconv.Atoi(hash[5:6])
				if err != nil || idx > 7 {
					continue
				}
				if _, ok := password[idx]; !ok {
					password[idx] = hash[6:7]
				}
			}
		}
	}

	ret := ""
	for i := 0; i < 8; i++ {
		ret += password[i]
	}
	return ret
}
