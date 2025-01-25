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

func chk_abba(s string) bool {
	for i := 0; i < len(s)-3; i++ {
		if s[i] == s[i+3] && s[i+1] == s[i+2] && s[i] != s[i+1] {
			return true
		}
	}
	return false
}

func chk_aba_bab(a string, b string) bool {
	for i := 0; i < len(a)-2; i++ {
		if a[i] == a[i+2] && a[i] != a[i+1] {
			for j := 0; j < len(b)-2; j++ {
				if b[j] == b[j+2] && b[j] == a[i+1] && b[j+1] == a[i] {
					return true
				}
			}
		}
	}
	return false
}

func run(part2 bool, input string) any {
	tls_cnt := 0
	ssl_cnt := 0
	for _, line := range strings.Split(input, "\n") {
		outside := false
		inside := false

		split := strings.Split(line, "[")
		out := []string{}
		in := []string{}

		outside = chk_abba(split[0])
		out = append(out, split[0])

		for _, e := range split[1:] {
			entry := strings.Split(e, "]")

			in = append(in, entry[0])
			out = append(out, entry[1])

			if !inside {
				inside = chk_abba(entry[0])
			}
			if !outside {
				outside = chk_abba(entry[1])
			}
		}

		if !inside && outside {
			tls_cnt += 1
		}

	out_loop:
		for _, o := range out {
			for _, i := range in {
				if chk_aba_bab(o, i) {
					ssl_cnt += 1
					break out_loop
				}
			}
		}
	}

	if part2 {
		return ssl_cnt
	}
	return tls_cnt
}
