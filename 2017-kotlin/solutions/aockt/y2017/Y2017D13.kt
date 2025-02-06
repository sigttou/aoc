package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D13 : Solution {

    private fun parse(input: String): Map<Int, Int> {
        return input.lines().associate { line ->
            val (depth, range) = line.split(": ").map { it.toInt() }
            depth to range
        }
    }

    private fun severityOfTrip(layers: Map<Int, Int>): Int {
        var severity = 0

        for ((depth, range) in layers) {
            if (depth % ((range - 1) * 2) == 0) {
                severity += depth * range
            }
        }

        return severity
    }

    override fun partOne(input: String): Int {
        val layers = parse(input)
        return severityOfTrip(layers)
    }

    private fun caughtWithDelay(layers: Map<Int, Int>, delay: Int): Boolean {
        for ((depth, range) in layers) {
            if ((depth + delay) % ((range - 1) * 2) == 0) {
                return true
            }
        }
        return false
    }

    override fun partTwo(input: String): Int {
        val layers = parse(input)
        var delay = 0

        while (caughtWithDelay(layers, delay)) {
            delay++
        }

        return delay
    }
}