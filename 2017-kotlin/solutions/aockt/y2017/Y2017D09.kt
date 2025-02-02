package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D09 : Solution {


    private fun parse(input: String): String {
        return input
    }

    override fun partOne(input: String): Int {
        var score = 0
        var level = 0
        var inGarbage = false
        var skipNext = false

        for (char in parse(input)) {
            if (skipNext) {
                skipNext = false
                continue
            }

            when (char) {
                '!' -> skipNext = true
                '>' -> inGarbage = false
                '<' -> if (!inGarbage) inGarbage = true
                '{' -> if (!inGarbage) level++
                '}' -> if (!inGarbage) {
                    score += level
                    level--
                }
            }
        }

        return score
    }

    override fun partTwo(input: String): Int {
        var inGarbage = false
        var skipNext = false
        var garbageCount = 0

        for (char in parse(input)) {
            if (skipNext) {
                skipNext = false
                continue
            }

            when (char) {
                '!' -> skipNext = true
                '>' -> inGarbage = false
                '<' -> if (!inGarbage) inGarbage = true else garbageCount++
                else -> if (inGarbage) garbageCount++
            }
        }

        return garbageCount
    }
}