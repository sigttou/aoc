package aockt.y2017

import io.github.jadarma.aockt.core.Solution
import kotlin.math.*

/** A solution to a fictitious puzzle used for testing. */
object Y2017D03 : Solution {

    private fun parse(input: String): Int {
        return input.toInt()
    }

    override fun partOne(input: String) : Int {
        val inputVal = parse(input)
        val layer = floor(ceil(sqrt(inputVal.toDouble())) / 2) + 1
        val maxVal = 2*layer - 1
        val squaredMax = maxVal.pow(2)

        var distToEdge = maxVal
        for (i in 0..5) {
            if (abs(squaredMax - i*(maxVal-1) - inputVal) < distToEdge) {
                distToEdge = abs(squaredMax - i * (maxVal -1) - inputVal)
            }
        }
        return (maxVal - 1 - distToEdge).toInt()
    }

    private fun nxtCoordinates(coordinates : Pair<Int,Int>): Pair<Int, Int> {
        val x = coordinates.first
        val y = coordinates.second

        if (x == 0 && y == 0)
            return Pair(1, 0)
        if (y > -x && x > y)
            return Pair(x, y+1)
        if (y > -x && y >= x)
            return Pair(x-1, y)
        if (y <= -x && x < y)
            return Pair(x, y-1)
        if (y <= -x && x >= y)
            return Pair(x+1, y)

        return Pair(0, 0)
    }

    override fun partTwo(input: String) : Int {
        var coordinates = Pair(0, 0)
        val spiral = mutableMapOf<Pair<Int, Int>, Int>().withDefault { 0 }
        spiral[coordinates] = 1

        while (spiral.getValue(coordinates) <= parse(input)) {
            coordinates = nxtCoordinates(coordinates)
            spiral[coordinates] = (-1..1).sumOf { i->
                listOf(0, 1, -1).sumOf { j -> spiral.getValue(Pair(coordinates.first+i, coordinates.second+j)) }
            }
        }
        return spiral.getValue(coordinates)
    }
}
