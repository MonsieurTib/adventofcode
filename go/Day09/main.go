package main

import (
	"bufio"
	"io"
	"strconv"
	"strings"
)

func getHistory(input io.Reader) [][]int {
	scanner := bufio.NewScanner(input)
	var history [][]int
	for scanner.Scan() {
		parts := strings.Split(scanner.Text(), " ")
		var numbers = make([]int, len(parts))
		for i, part := range parts {
			if n, err := strconv.Atoi(part); err == nil {
				numbers[i] = n
			}
		}
		history = append(history, numbers)
	}

	return history
}

func part1(input io.Reader) int {
	history := getHistory(input)
	sum := 0
	for _, numbers := range history {
		sum += parseHistoryLogPart1(numbers)
	}
	return sum
}

func part2(input io.Reader) int {
	history := getHistory(input)
	sum := 0
	for _, numbers := range history {
		sum += parseHistoryLogPart2(numbers)
	}
	return sum
}

func parseHistoryLogPart1(numbers []int) int {

	var history [][]int
	history = append(history, numbers)
	current := numbers
	for {
		var newLine []int
		for i := 0; i < len(current)-1; i++ {
			diff := current[i+1] - current[i]
			newLine = append(newLine, diff)
		}

		history = append(history, newLine)
		if allZero(history[len(history)-1:][0]) {
			break
		}
		current = newLine
	}

	history[len(history)-1] = append(history[len(history)-1], 0)
	for i := len(history) - 2; i >= 0; i-- {
		sum := history[i+1][len(history[i+1])-1] + history[i][len(history[i])-1]
		history[i] = append(history[i], sum)
	}
	var result = history[0][len(history[0])-1]
	return result
}

func parseHistoryLogPart2(numbers []int) int {

	var history [][]int
	history = append(history, numbers)
	current := numbers
	for {
		var newLine []int
		for i := 0; i < len(current)-1; i++ {
			diff := current[i+1] - current[i]
			newLine = append(newLine, diff)
		}

		history = append(history, newLine)
		if allZero(history[len(history)-1:][0]) {
			break
		}
		current = newLine
	}

	history[len(history)-1] = append([]int{0}, history[len(history)-1]...)
	for i := len(history) - 2; i >= 0; i-- {
		sum := history[i][0] - history[i+1][0]
		history[i] = append([]int{sum}, history[i]...)
	}
	var result = history[0][0]
	return result
}

func allZero(numbers []int) bool {
	for _, num := range numbers {
		if num != 0 {
			return false
		}
	}
	return true
}
