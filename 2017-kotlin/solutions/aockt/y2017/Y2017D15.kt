package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D15 : Solution {

    private fun parse(input: String): List<Long> {
        return input.lines().map{it.split(" ").last().toLong()}
    }

    private const val FACTOR_A: Long = 16807
    private const val FACTOR_B: Long = 48271
    private const val DIVISOR = 2147483647
    private const val PAIRS_COUNT = 40_000_000
    private const val PAIRS_COUNT_PART_TWO = 5_000_000

    private fun nextValue(value: Long, factor: Long): Long {
        return (value * factor) % DIVISOR
    }

    private fun lowest16BitsMatch(valueA: Long, valueB: Long): Boolean {
        return (valueA and 0xFFFF) == (valueB and 0xFFFF)
    }

    override fun partOne(input: String): Int {
        val inLong = parse(input)
        var valueA = inLong[0]
        var valueB = inLong[1]
        var matchCount = 0

        repeat(PAIRS_COUNT) {
            valueA = nextValue(valueA, FACTOR_A)
            valueB = nextValue(valueB, FACTOR_B)
            if (lowest16BitsMatch(valueA, valueB)) {
                matchCount++
            }
        }

        return matchCount
    }

    private fun generateValues(start: Long, factor: Long, criteria: Int): Sequence<Long> = sequence {
        var value = start
        while (true) {
            value = nextValue(value, factor)
            if (value % criteria == 0L) yield(value)
        }
    }

    override fun partTwo(input: String): Int {
        val inLong = parse(input)
        val generatorA = generateValues(inLong[0], FACTOR_A, 4).iterator()
        val generatorB = generateValues(inLong[1], FACTOR_B, 8).iterator()
        var matchCount = 0

        repeat(PAIRS_COUNT_PART_TWO) {
            val valueA = generatorA.next()
            val valueB = generatorB.next()
            if (lowest16BitsMatch(valueA, valueB)) {
                matchCount++
            }
        }

        return matchCount
    }
}