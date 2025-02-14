package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D21 : Solution {

    private const val INITIAL_GRID = ".#./..#/###"

    private fun parse(input: String): Map<String, String> {
        val rules = mutableMapOf<String, String>()
        input.lines().forEach { line ->
            val (pattern, result) = line.split(" => ")
            val patterns = generatePatterns(pattern)
            patterns.forEach { rules[it] = result }
        }
        return rules
    }


    private fun generatePatterns(pattern: String): List<String> {
        val grid = pattern.split("/").map { it.toCharArray() }
        val patterns = mutableListOf<String>()
        repeat(4) {
            patterns.add(grid.joinToString("/") { String(it) })
            patterns.add(grid.reversed().joinToString("/") { String(it) })
            rotateGrid(grid)
        }
        return patterns
    }

    private fun rotateGrid(grid: List<CharArray>) {
        val n = grid.size
        val newGrid = Array(n) { CharArray(n) }
        for (i in 0 until n) {
            for (j in 0 until n) {
                newGrid[j][n - 1 - i] = grid[i][j]
            }
        }
        for (i in 0 until n) {
            for (j in 0 until n) {
                grid[i][j] = newGrid[i][j]
            }
        }
    }

    private fun enhanceGrid(grid: List<String>, rules: Map<String, String>): List<String> {
        val size = grid.size
        val blockSize = if (size % 2 == 0) 2 else 3
        val newSize = size / blockSize * (blockSize + 1)
        val newGrid = MutableList(newSize) { CharArray(newSize) { '.' } }

        for (i in grid.indices step blockSize) {
            for (j in grid[i].indices step blockSize) {
                val block = List(blockSize) { grid[i + it].substring(j, j + blockSize) }
                val newBlock = rules[block.joinToString("/")]!!.split("/")
                for (x in newBlock.indices) {
                    for (y in newBlock[x].indices) {
                        newGrid[i / blockSize * (blockSize + 1) + x][j / blockSize * (blockSize + 1) + y] = newBlock[x][y]
                    }
                }
            }
        }

        return newGrid.map { String(it) }
    }

    private fun countOnPixels(grid: List<String>): Int {
        return grid.sumOf { row -> row.count { it == '#' } }
    }

    private fun iterateEnhancement(grid: List<String>, rules: Map<String, String>, iterations: Int): List<String> {
        var currentGrid = grid
        repeat(iterations) {
            currentGrid = enhanceGrid(currentGrid, rules)
        }
        return currentGrid
    }

    override fun partOne(input: String): Int {
        val rules = parse(input)
        val initialGrid = INITIAL_GRID.split("/")
        val finalGrid = iterateEnhancement(initialGrid, rules, 5)
        return countOnPixels(finalGrid)
    }

    override fun partTwo(input: String): Int {
        val rules = parse(input)
        val initialGrid = INITIAL_GRID.split("/")
        val finalGrid = iterateEnhancement(initialGrid, rules, 18)
        return countOnPixels(finalGrid)
    }
}