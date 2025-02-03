package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D10 : Solution {

    private fun parse(input: String): List<Int> {
        return input.split(",").map { it.trim().toInt() }
    }

    private fun parse2(input: String): List<Int> {
        return input.trim().map { it.code } + listOf(17, 31, 73, 47, 23)
    }

    private fun knotHash(lengths: List<Int>, size: Int = 256, rounds: Int = 64): List<Int> {
        val list = (0 until size).toMutableList()
        var position = 0
        var skipSize = 0

        repeat(rounds) {
            for (length in lengths) {
                if (length > list.size) continue

                // Reverse the section of the list
                val sublist = (0 until length).map { list[(position + it) % list.size] }.reversed()
                for (i in 0 until length) {
                    list[(position + i) % list.size] = sublist[i]
                }

                // Move position forward and increase skip size
                position = (position + length + skipSize) % list.size
                skipSize++
            }
        }

        return list
    }

    override fun partOne(input: String): Int {
        val lengths = parse(input)
        var size = 256
        if (lengths[0] == 3)
            size = 5
        val sparseHash = knotHash(lengths, size, 1)
        return sparseHash[0] * sparseHash[1]
    }

    private fun denseHash(sparseHash: List<Int>): List<Int> {
        return sparseHash.chunked(16).map { block ->
            block.reduce { acc, num -> acc xor num }
        }
    }

    private fun toHexString(denseHash: List<Int>): String {
        return denseHash.joinToString("") { it.toString(16).padStart(2, '0') }
    }

    override fun partTwo(input: String): String {
        val lengths = parse2(input)
        val sparseHash = knotHash(lengths)
        val denseHash = denseHash(sparseHash)
        return toHexString(denseHash)
    }
}