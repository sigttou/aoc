package aockt.y2017

import io.github.jadarma.aockt.core.Solution
import kotlin.math.abs

/** A solution to a fictitious puzzle used for testing. */
object Y2017D11 : Solution {

    private fun parse(input: String): List<String> {
        return input.trim().split(",")
    }

    private data class Hex(val x: Int, val y: Int, val z: Int) {
        fun distanceTo(other: Hex): Int {
            return (abs(this.x - other.x) + abs(this.y - other.y) + abs(this.z - other.z)) / 2
        }

        fun step(direction: String): Hex {
            return when (direction) {
                "n" -> Hex(x, y + 1, z - 1)
                "ne" -> Hex(x + 1, y, z - 1)
                "se" -> Hex(x + 1, y - 1, z)
                "s" -> Hex(x, y - 1, z + 1)
                "sw" -> Hex(x - 1, y, z + 1)
                "nw" -> Hex(x - 1, y + 1, z)
                else -> this
            }
        }
    }

    override fun partOne(input: String): Int {
        val directions = parse(input)
        var position = Hex(0, 0, 0)

        for (direction in directions) {
            position = position.step(direction)
        }

        return position.distanceTo(Hex(0, 0, 0))
    }

    override fun partTwo(input: String): Int {
        val directions = parse(input)
        var position = Hex(0, 0, 0)
        var maxDistance = 0

        for (direction in directions) {
            position = position.step(direction)
            val distance = position.distanceTo(Hex(0, 0, 0))
            if (distance > maxDistance) {
                maxDistance = distance
            }
        }

        return maxDistance
    }
}