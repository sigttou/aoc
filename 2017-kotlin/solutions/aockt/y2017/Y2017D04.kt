package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D04 : Solution {

    private fun parse(input: String): List<List<String>> {
        return input.lines().map { it.split("\\s+".toRegex()).toList() }
    }

    override fun partOne(input: String) : Int {
        return parse(input).count {it.size == it.toSet().size}
    }

    override fun partTwo(input: String) : Int {
        return parse(input).map{it.map { inner -> String(inner.toCharArray().sortedArray()) }}.count{it.size == it.toSet().size}
    }
}
