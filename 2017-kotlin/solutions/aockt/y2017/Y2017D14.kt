package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D14 : Solution {

    private fun parse(input: String): String {
        return input
    }

    private fun knotHash(input: String): String {
        val lengths = input.map { it.code } + listOf(17, 31, 73, 47, 23)
        val sparseHash = (0 until 256).toMutableList()
        var position = 0
        var skipSize = 0

        repeat(64) {
            lengths.forEach { length ->
                if (length > sparseHash.size) return@forEach

                val sublist = (0 until length).map { sparseHash[(position + it) % sparseHash.size] }.reversed()
                sublist.forEachIndexed { index, value ->
                    sparseHash[(position + index) % sparseHash.size] = value
                }

                position = (position + length + skipSize) % sparseHash.size
                skipSize++
            }
        }

        return sparseHash.chunked(16)
            .joinToString("") { it.reduce { acc, num -> acc xor num }.toString(16).padStart(2, '0') }
    }

    private fun hexToBinary(hex: String): String {
        return hex.map { it.toString().toInt(16).toString(2).padStart(4, '0') }.joinToString("")
    }

    private fun generateGrid(key: String): List<String> {
        return (0 until 128).map { row ->
            val hash = knotHash("$key-$row")
            hexToBinary(hash)
        }
    }

    override fun partOne(input: String): Int {
        val grid = generateGrid(parse(input))
        return grid.sumOf { row -> row.count { it == '1' } }
    }

    private fun countRegions(grid: List<String>): Int {
        val visited = Array(128) { BooleanArray(128) }
        var regionCount = 0

        fun dfs(x: Int, y: Int) {
            if (x !in 0 until 128 || y !in 0 until 128 || grid[x][y] == '0' || visited[x][y]) return
            visited[x][y] = true
            dfs(x + 1, y)
            dfs(x - 1, y)
            dfs(x, y + 1)
            dfs(x, y - 1)
        }

        for (x in 0 until 128) {
            for (y in 0 until 128) {
                if (grid[x][y] == '1' && !visited[x][y]) {
                    regionCount++
                    dfs(x, y)
                }
            }
        }

        return regionCount
    }

    override fun partTwo(input: String): Int {
        val grid = generateGrid(parse(input))
        return countRegions(grid)
    }
}