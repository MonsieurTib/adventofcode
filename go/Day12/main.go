package main

import (
	"bufio"
	"fmt"
	"io"
	"math"
	"strconv"
	"strings"
)

func part1(input io.Reader) int {
	sum := 0
	scanner := bufio.NewScanner(input)
	for scanner.Scan() {
		parts := strings.Split(scanner.Text(), " ")
		partSizes := strings.Split(parts[1], ",")
		sizes := make([]int, len(partSizes))

		for i, v := range partSizes {
			sizes[i], _ = strconv.Atoi(v)
		}

		possibleCombinations := generateCombinations(parts[0], sizes)
		sum += int(math.Max(1, float64(possibleCombinations)))
	}
	return sum
}

func generateCombinations(str string, groups []int) int64 {

	sumGroup := sum(groups)
	indices := make([]int, 0)
	for i, v := range str {
		if v == '?' {
			indices = append(indices, i)
		}
	}
	possibleCombinations := int64(0)

	init := strings.Count(str, "#")
	for i := 0; i < 1<<len(indices); i++ {
		combination := []byte(str)
		n := init
		for j, v := range indices {
			if i&(1<<j) != 0 {
				combination[v] = '#'
				n++
				//debug := string(combination)
				//fmt.Println(debug)
				if n > sumGroup {
					break
				}
			}
		}

		if n == sumGroup {

			s := strings.Replace(string(combination), "?", ".", -1)
			if areSlicesEqual(splitString(s), groups) {
				possibleCombinations += 1
			}
		}
	}

	return possibleCombinations
}

func areSlicesEqual(a, b []int) bool {
	if len(a) != len(b) {
		return false
	}
	for i, v := range a {
		if v != b[i] {
			return false
		}
	}
	return true
}

func sum(arr []int) int {
	total := 0
	for _, num := range arr {
		total += num
	}
	return total
}

func splitString(s string) []int {
	split := strings.Split(s, ".")
	result := make([]int, 0)
	for i := 0; i < len(split); i++ {
		n := strings.Count(split[i], "#")
		if n > 0 {
			result = append(result, n)
		}
	}
	return result
}

func part2(input io.Reader) int {

	sum := 0
	scanner := bufio.NewScanner(input)
	for scanner.Scan() {
		parts := strings.Split(scanner.Text(), " ")

		record := strings.Repeat(parts[0]+"?", 5)
		record = record[:len(record)-1]

		fSizes := strings.Repeat(parts[1]+",", 5)
		fSizes = fSizes[:len(fSizes)-1]

		partSizes := strings.Split(fSizes, ",")
		sizes := make([]int, len(partSizes))

		for i, v := range partSizes {
			sizes[i], _ = strconv.Atoi(v)
		}

		sum += process(record, sizes, make(map[string]int))
	}

	return sum
}

// because part 1 implementation is not working for part 2 ( hours processing..) I have to implement a new one
func process(line string, groups []int, m map[string]int) int {

	key := fmt.Sprintf("%s-%v", line, groups)
	if val, ok := m[key]; ok {
		return val
	}

	if len(groups) == 0 {
		if strings.Contains(line, "#") {
			m[key] = 0
			return 0
		}
		m[key] = 1
		return 1
	}

	group := groups[0]
	combinations := 0
	for i := 0; i <= len(line)-group; i++ {
		n := i + group
		current := line[i:n]
		s := int(math.Min(float64(len(line)), float64(i+group+1)))

		if strings.Replace(current, "?", "#", -1) == strings.Repeat("#", group) && (n == len(line) || strings.Contains(".?", string(line[n]))) {
			combinations += process(line[s:], groups[1:], m)
		}

		if strings.HasPrefix(current, "#") {
			break
		}
	}
	m[key] = combinations
	return combinations
}
