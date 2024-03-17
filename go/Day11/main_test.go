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
	assert.Equal(t, 374, part1(file))
}

func TestPart1Input(t *testing.T) {
	file, err := os.Open("input.txt")
	require.NoError(t, err)
	assert.Equal(t, 10077850, part1(file))
}

func TestPart2Input(t *testing.T) {
	file, err := os.Open("input.txt")
	require.NoError(t, err)
	assert.Equal(t, 504715068438, part2(file))
}
