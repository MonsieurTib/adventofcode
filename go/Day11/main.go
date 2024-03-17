package main

import (
	"bufio"
	"io"
	"math"
	"slices"
)

type Point struct {
	x, y int
}

type Universe struct {
	m               [][]byte
	galaxies        []Point
	expandedCol     []int
	expandedRow     []int
	factorExpansion int
}

// 10021550 to low
// 10021435 to low
func part1(input io.Reader) int {
	u := NewUniverse(input, 2)
	u.expand()
	u.findGalaxies()
	distanceSum := 0
	for i := 0; i < len(u.galaxies); i++ {
		for j := i + 1; j < len(u.galaxies); j++ {
			galaxy := u.galaxies[i]
			other := u.galaxies[j]
			distanceSum += int(math.Abs(float64(galaxy.y)-float64(other.y)) + math.Abs(float64(galaxy.x)-float64(other.x)))
		}
	}
	return distanceSum
}

func part2(input io.Reader) int {
	u := NewUniverse(input, 1000000)
	u.expand()
	u.findGalaxies()
	distanceSum := 0
	for i := 0; i < len(u.galaxies); i++ {
		for j := i + 1; j < len(u.galaxies); j++ {
			galaxy := u.galaxies[i]
			other := u.galaxies[j]
			distanceSum += int(math.Abs(float64(galaxy.y)-float64(other.y)) + math.Abs(float64(galaxy.x)-float64(other.x)))
		}
	}
	return distanceSum
}

func NewUniverse(input io.Reader, factorExpansion int) *Universe {
	u := new(Universe)
	u.factorExpansion = factorExpansion
	u.getMap(input)
	return u
}

func (universe *Universe) findGalaxies() {
	deltaY := 0
	for i := 0; i < len(universe.m); i++ {
		if slices.Index(universe.expandedRow, i) != -1 {
			deltaY += universe.factorExpansion - 1
		}
		deltaX := 0
		for j := 0; j < len(universe.m[i]); j++ {
			if slices.Index(universe.expandedCol, j) != -1 {
				deltaX += universe.factorExpansion - 1
			}
			if universe.m[i][j] == '#' {
				universe.galaxies = append(universe.galaxies, Point{y: deltaY + i, x: deltaX + j})
			}
		}
	}
}

func (universe *Universe) expand() {
	width := len(universe.m[0])
	height := len(universe.m)

	for row := 0; row < height; row++ {
		RowNoGalaxy := true
		for col := 0; col < width; col++ {
			if universe.m[row][col] == '#' {
				RowNoGalaxy = false
			}
			if row == 0 {
				ColNoGalaxy := true
				for i := 0; i < height; i++ {
					if universe.m[i][col] == '#' {
						ColNoGalaxy = false
					}
				}
				if ColNoGalaxy {
					universe.expandedCol = append(universe.expandedCol, col)
				}
			}
		}
		if RowNoGalaxy {
			universe.expandedRow = append(universe.expandedRow, row)
		}
	}
}

func (universe *Universe) getMap(input io.Reader) {
	scanner := bufio.NewScanner(input)
	for scanner.Scan() {
		universe.m = append(universe.m, []byte(scanner.Text()))
	}
}

func (universe *Universe) printMap() string {
	var s string
	for _, row := range universe.m {
		s += string(row) + "\n"
	}
	println(s)
	return s
}
