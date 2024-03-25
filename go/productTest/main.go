package main

import (
	"fmt"
	"strings"
)

//var cache = make(map[string]int)

func count(line string, groups []int) int {

	if len(groups) == 0 {
		if strings.Contains(line, "#") {
			return 0
		}
		return 1
	}

	group := groups[0]
	ways := 0

	for i := 0; i <= len(line)-group; i++ {

		candidate := line[i : i+group]

		if strings.Replace(candidate, "?", "#", -1) == strings.Repeat("#", group) && (i+group == len(line) || line[i+group] == '.' || line[i+group] == '?') {
			s := i + group + 1
			if s > len(line) {
				s = len(line)
			}
			ways += count(line[s:], groups[1:])
		}

		if strings.HasPrefix(candidate, "#") {
			break
		}
	}

	return ways
}

func main() {
	total := 0
	pattern := "?###????????"
	groups := []int{3, 2, 1}

	total += count(pattern, groups)

	fmt.Println(total)
}
