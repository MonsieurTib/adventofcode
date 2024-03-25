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
	assert.Equal(t, 21, part1(file))
}

func TestPart1Input(t *testing.T) {
	file, err := os.Open("input.txt")
	require.NoError(t, err)
	assert.Equal(t, 7025, part1(file))
}

func TestPart2Sample(t *testing.T) {
	file, err := os.Open("sample.txt")
	require.NoError(t, err)
	assert.Equal(t, 525152, part2(file))
}

func TestPart2Input(t *testing.T) {
	file, err := os.Open("input.txt")
	require.NoError(t, err)
	assert.Equal(t, 11461095383315, part2(file))
}
