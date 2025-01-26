package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D01 : Solution {

    override fun partOne(input: String) : Int {
        var ret = 0

        for (i in  input.indices) {
            val c = input[i]
            val nxt = if (i + 1 >= input.length) input[0] else input[i+1]
            if (c == nxt) ret += c.toString().toInt()
        }
        return ret
    }

    override fun partTwo(input: String) : Int {
        var ret = 0

        for (i in input.indices) {
            val c = input[i]
            val nxtIndex = input.length / 2
            val nxt = if (i + nxtIndex >= input.length) {
                input[i + nxtIndex - input.length]
            } else {
                input[i + nxtIndex]
            }
            if (c == nxt) ret += c.toString().toInt()
        }
        return ret
    }
}
