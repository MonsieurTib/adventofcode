package main

import (
	"bufio"
	"fmt"
	"io"
	"math"
)

type Point struct {
	Y int
	X int
}

func part1(input io.Reader) int {
	m := getMap(input)
	startingY := 0
	startingX := 0

	for i, row := range m {
		for j, col := range row {
			if col == 'S' {
				startingY = i
				startingX = j
			}
		}
	}
	position := Point{
		Y: startingY,
		X: startingX,
	}
	fmt.Printf("Y %d X %d", startingY, startingX)

	right, _ := traverse(m, startingY, startingX+1, position)
	left, _ := traverse(m, startingY, startingX-1, position)
	bottom, _ := traverse(m, startingY-1, startingX, position)
	top, _ := traverse(m, startingY-1, startingX, position)

	maxDistance := math.Max(right, left)
	maxDistance = math.Max(maxDistance, bottom)
	maxDistance = math.Max(maxDistance, top)
	return int(maxDistance)
}

// 573 TO HIGH
// 392 TO HIGH
// 371 GOOD
func part2(input io.Reader) int {
	m := getMap(input)
	startingY := 0
	startingX := 0

	for i, row := range m {
		for j, col := range row {
			if col == 'S' {
				startingY = i
				startingX = j
			}
		}
	}

	position := Point{
		Y: startingY,
		X: startingX,
	}

	maxDistance := 0.0
	var loopPoints []Point
	right, pointsR := traverse(m, startingY, startingX+1, position)
	left, pointsL := traverse(m, startingY, startingX-1, position)
	bottom, pointsB := traverse(m, startingY+1, startingX, position)
	top, pointsT := traverse(m, startingY-1, startingX, position)

	if right > maxDistance {
		maxDistance = right
		loopPoints = pointsR
	}
	if left > maxDistance {
		maxDistance = left
		loopPoints = pointsL
	}
	if bottom > maxDistance {
		maxDistance = bottom
		loopPoints = pointsB
	}
	if top > maxDistance {
		maxDistance = top
		loopPoints = pointsT
	}
	//fmt.Println(loopPoints)
	//printLoop(m, loopPoints)
	//findAreas(m, loopPoints)

	minY := len(m)
	maxY := 0
	minX := len(m[0])
	maxX := 0

	for _, point := range loopPoints {
		minY = int(math.Min(float64(point.Y), float64(minY)))
		maxY = int(math.Max(float64(point.Y), float64(maxY)))
		minX = int(math.Min(float64(point.X), float64(minX)))
		maxX = int(math.Max(float64(point.X), float64(maxX)))
	}

	enclosedTiles := 0
	for y := minY; y < maxY; y++ {
		toggle := false
		for x := minX; x < maxX; x++ {
			if pointInLoop(loopPoints, y, x) && (m[y][x] == '|' || m[y][x] == 'L' || m[y][x] == 'J' || m[y][x] == 'S') {
				toggle = !toggle
				if toggle {
					//fmt.Printf("debug \n%c", m[y][x])
				}
			}
			if !pointInLoop(loopPoints, y, x) && toggle {
				enclosedTiles++
			}
		}
	}
	fmt.Printf("\n enclosed titles %d", enclosedTiles)
	return enclosedTiles
}

func printLoop(m [][]byte, loop []Point) {
	for y := 0; y < len(m); y++ {
		line := ""
		for x := 0; x < len(m[0]); x++ {
			if pointInLoop(loop, y, x) {
				line += "x"
			} else {
				line += string(m[y][x])
			}
		}
		fmt.Println(line)
	}
}

func pointInLoop(loop []Point, y int, x int) bool {
	for _, point := range loop {
		if point.Y == y && point.X == x {
			return true
		}
	}
	return false
}

func traverse(m [][]byte, y int, x int, previous Point) (float64, []Point) {

	height := len(m)
	width := len(m[0])

	if y < 0 || y > height {
		return 0, nil
	}

	if x < 0 || x > width {
		return 0, nil
	}

	connectMap := make(map[byte]map[byte][]byte)
	connectMap['|'] = map[byte][]byte{
		'T': {'F', '7', 'S', '|'},
		'B': {'L', 'J', 'S', '|'},
	}
	connectMap['-'] = map[byte][]byte{
		'L': {'L', 'F', 'S', '|', '-'},
		'R': {'J', '7', 'S', '|', '-'},
	}
	connectMap['L'] = map[byte][]byte{
		'T': {'7', 'F', 'S', '|'},
		'R': {'J', '7', 'S', '-'},
	}
	connectMap['J'] = map[byte][]byte{
		'T': {'F', '7', 'S', '|'},
		'L': {'F', 'L', 'S', '-'},
	}
	connectMap['7'] = map[byte][]byte{
		'L': {'F', 'L', '-', 'S'},
		'B': {'J', 'L', 'S', '|'},
	}
	connectMap['F'] = map[byte][]byte{
		'R': {'7', 'J', '-', 'S'},
		'B': {'J', 'L', 'S', '|'},
	}
	distance := 0.0

	var points []Point
	for {
		current := m[y][x]

		if current == '.' {
			break
		}
		if current == 'S' {
			points = append(points, Point{X: x, Y: y})
			break
		}
		//fmt.Printf("%f %c\n", distance, current)

		if current == 'J' {
			if previous.Y == y {
				if y-1 >= 0 && canConnect(connectMap, current, 'T', m[y-1][x]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					y--
				}
			} else {
				if x-1 >= 0 && canConnect(connectMap, current, 'L', m[y][x-1]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					x--
				}
			}
		} else if current == 'F' {
			if previous.Y == y {
				if y+1 < height && canConnect(connectMap, current, 'B', m[y+1][x]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					y++
				}
			} else {
				if x+1 < width && canConnect(connectMap, current, 'R', m[y][x+1]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					x++
				}
			}
		} else if current == '7' {
			if previous.Y == y {
				if y+1 < height && canConnect(connectMap, current, 'B', m[y+1][x]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					y++
				}
			} else {
				if x-1 >= 0 && canConnect(connectMap, current, 'L', m[y][x-1]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					x--
				}
			}
		} else if current == '|' {
			if previous.Y == y-1 {
				if y+1 < height && canConnect(connectMap, current, 'B', m[y+1][x]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					y++
				}
			} else {
				if y-1 >= 0 && canConnect(connectMap, current, 'T', m[y-1][x]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					y--
				}
			}
		} else if current == 'L' {
			if previous.Y == y-1 {
				if x+1 < width && canConnect(connectMap, current, 'R', m[y][x+1]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					x++
				}
			} else {
				if y-1 >= 0 && canConnect(connectMap, current, 'T', m[y-1][x]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					y--
				}
			}
		} else if current == '-' {
			if previous.X == x-1 {
				if x+1 < width && canConnect(connectMap, current, 'R', m[y][x+1]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					x++
				}
			} else {
				if x-1 >= 0 && canConnect(connectMap, current, 'L', m[y][x-1]) {
					points = append(points, Point{X: x, Y: y})
					previous = Point{X: x, Y: y}
					distance++
					x--
				}
			}
		}
	}
	return math.Ceil(distance / 2), points
}

func canConnect(connectMap map[byte]map[byte][]byte, key byte, subKey byte, c byte) bool {

	found := false
	for _, value := range connectMap[key][subKey] {
		if value == c {
			found = true
			break
		}
	}
	return found
}

func getMap(input io.Reader) [][]byte {
	scanner := bufio.NewScanner(input)
	var m [][]byte

	for scanner.Scan() {
		m = append(m, []byte(scanner.Text()))
	}
	return m
}
