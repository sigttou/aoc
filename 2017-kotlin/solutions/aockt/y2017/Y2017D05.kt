package aockt.y2017

import io.github.jadarma.aockt.core.Solution
import kotlin.math.abs

/** A solution to a fictitious puzzle used for testing. */
object Y2017D05 : Solution {

    private fun parse(input: String): List<Int> {
        return input.lines().map { it.toInt() }
    }

    private fun countSteps(instr: MutableList<Int>, part2: Boolean): Int {
        var pc = 0
        var steps = 0
        while (pc in instr.indices) {
            val jump = instr[pc]
            if (part2 && jump >= 3) {
                instr[pc] -= 1
            } else {
                instr[pc] += 1
            }
            pc += jump
            steps++
        }
        return steps
    }

    override fun partOne(input: String) : Int {
        return countSteps(parse(input).toMutableList(), false)
    }

    override fun partTwo(input: String) : Int {
        return countSteps(parse(input).toMutableList(), true)
    }
}
