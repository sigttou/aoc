package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D17 : Solution {

    private fun parse(input: String): Int {
        return input.toInt()
    }

    private fun spinlock(steps: Int, iterations: Int): List<Int> {
        val buffer = mutableListOf(0)
        var currentPosition = 0

        for (value in 1..iterations) {
            currentPosition = (currentPosition + steps) % buffer.size + 1
            buffer.add(currentPosition, value)
        }

        return buffer
    }

    override fun partOne(input: String): Int {
        val buffer = spinlock(parse(input), 2017)
        val index = buffer.indexOf(2017)
        return buffer[(index + 1) % buffer.size]
    }

    override fun partTwo(input: String): Int {
        val steps = parse(input)
        var currentPosition = 0
        var valueAfterZero = 0

        for (value in 1..50_000_000) {
            currentPosition = (currentPosition + steps) % value + 1
            if (currentPosition == 1) {
                valueAfterZero = value
            }
        }

        return valueAfterZero

    }
}