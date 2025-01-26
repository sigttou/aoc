package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D02 : Solution {

    private fun parse(input: String): List<List<Int>> {
        return input.lines().map{it.split("\\s+".toRegex()).map{it2 -> it2.toInt()}}
    }

    override fun partOne(input: String) : Int {
        return parse(input).sumOf { it.max() - it.min() }
    }

    override fun partTwo(input: String) : Int {
        return parse(input).sumOf {it.map{a -> it.map { b -> Pair(a, b)}}.flatten()
            .filter {inner -> inner.first != inner.second && inner.first % inner.second == 0 }
            .map {inner -> inner.first / inner.second }[0]}
    }
}
