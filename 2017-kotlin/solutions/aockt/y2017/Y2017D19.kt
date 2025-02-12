package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D19 : Solution {

    private enum class Direction(val dx: Int, val dy: Int) {
        UP(0, -1), DOWN(0, 1), LEFT(-1, 0), RIGHT(1, 0);

        fun turnLeft(): Direction = when (this) {
            UP -> LEFT
            DOWN -> RIGHT
            LEFT -> DOWN
            RIGHT -> UP
        }

        fun turnRight(): Direction = when (this) {
            UP -> RIGHT
            DOWN -> LEFT
            LEFT -> UP
            RIGHT -> DOWN
        }
    }

    private fun findStart(diagram: List<String>): Pair<Int, Int> {
        val x = diagram[0].indexOf('|')
        return x to 0
    }

    private fun move(x: Int, y: Int, direction: Direction): Pair<Int, Int> {
        return x + direction.dx to y + direction.dy
    }

    private fun isValidPosition(x: Int, y: Int, diagram: List<String>): Boolean {
        return y in diagram.indices && x in diagram[y].indices && diagram[y][x] != ' '
    }

    private fun followPath(diagram: List<String>): Pair<String, Int> {
        val (startX, startY) = findStart(diagram)
        var x = startX
        var y = startY
        var direction = Direction.DOWN
        val letters = StringBuilder()
        var steps = 0

        while (true) {
            val currentChar = diagram[y][x]
            steps++

            if (currentChar.isLetter()) {
                letters.append(currentChar)
            }

            val (nextX, nextY) = move(x, y, direction)
            if (isValidPosition(nextX, nextY, diagram)) {
                x = nextX
                y = nextY
                continue
            }

            direction = direction.turnLeft()
            val (leftX, leftY) = move(x, y, direction)
            if (isValidPosition(leftX, leftY, diagram)) {
                x = leftX
                y = leftY
                continue
            }

            direction = direction.turnRight().turnRight()
            val (rightX, rightY) = move(x, y, direction)
            if (isValidPosition(rightX, rightY, diagram)) {
                x = rightX
                y = rightY
                continue
            }

            break
        }

        return letters.toString() to steps
    }

    private fun parse(input: String): List<String> {
        return input.lines()
    }

    override fun partOne(input: String): String {
        val (letters, _) = followPath(parse(input))
        return letters
    }

    override fun partTwo(input: String): Int {
        val (_, cnt) = followPath(parse(input))
        return cnt
    }
}