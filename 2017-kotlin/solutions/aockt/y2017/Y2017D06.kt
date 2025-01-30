package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D06 : Solution {

    private fun parse(input: String): IntArray {
        return input.split("\\s+".toRegex()).map { it.toInt() }.toIntArray()
    }

    private tailrec fun reallocate(memory: IntArray, seen: Set<String>) : Int{
        val hash = memory.joinToString()
        if (hash in seen)
            return seen.size

        val (idx, cnt) = memory.withIndex().maxBy { it.value }
        memory[idx] = 0
        repeat(cnt) { i ->
            memory[(idx + i + 1) % memory.size] +=1
        }
        return reallocate(memory, seen + hash)
    }

    override fun partOne(input: String) : Int {
        return reallocate(parse(input), mutableSetOf())
    }

    override fun partTwo(input: String) : Int {
        val memory = parse(input)
        reallocate(memory, mutableSetOf())
        return reallocate(memory, mutableSetOf<String>())
    }
}
