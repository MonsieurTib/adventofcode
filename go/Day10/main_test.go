package main

import (
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
	"os"
	"testing"
)

func TestPart1Sample(t *testing.T) {
	file, err := os.Open("sample.txt")
	require.NoError(t, err)
	assert.Equal(t, 8, part1(file))
}

func TestPart1Input(t *testing.T) {
	file, err := os.Open("input.txt")
	require.NoError(t, err)
	assert.Equal(t, 6886, part1(file))
}

func TestPart2Sample(t *testing.T) {
	file, err := os.Open("sample.txt")
	require.NoError(t, err)
	assert.Equal(t, 10, part2(file))
}

func TestPart2Input(t *testing.T) {
	file, err := os.Open("input.txt")
	require.NoError(t, err)
	assert.Equal(t, 371, part2(file))
}
