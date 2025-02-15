package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D22 : Solution {

    private enum class Direction {
        UP, DOWN, LEFT, RIGHT
    }

    private data class Point(var x: Int, var y: Int) {
        operator fun plusAssign(direction: Direction) {
            when (direction) {
                Direction.UP -> y--
                Direction.DOWN -> y++
                Direction.LEFT -> x--
                Direction.RIGHT -> x++
            }
        }
    }

    private enum class NodeState {
        CLEAN, WEAKENED, INFECTED, FLAGGED
    }

    private fun parseInput(input: String): MutableMap<Point, NodeState> {
        val grid = mutableMapOf<Point, NodeState>()
        val lines = input.lines()
        val offset = lines.size / 2
        for (y in lines.indices) {
            for (x in lines[y].indices) {
                if (lines[y][x] == '#') {
                    grid[Point(x - offset, y - offset)] = NodeState.INFECTED
                }
            }
        }
        return grid
    }

    private fun simulateVirusPartOne(grid: MutableMap<Point, NodeState>, bursts: Int): Int {
        val position = Point(0, 0)
        var direction = Direction.UP
        var infectionCount = 0
        repeat(bursts) {
            val currentState = grid.getOrDefault(position.copy(), NodeState.CLEAN)
            direction = if (currentState == NodeState.CLEAN) turnLeft(direction) else turnRight(direction)
            grid[position.copy()] = if (currentState == NodeState.CLEAN) {
                infectionCount++
                NodeState.INFECTED
            } else {
                NodeState.CLEAN
            }
            position += direction
        }
        return infectionCount
    }

    private fun simulateVirusPartTwo(grid: MutableMap<Point, NodeState>, bursts: Int): Int {
        val position = Point(0, 0)
        var direction = Direction.UP
        var infectionCount = 0

        repeat(bursts) {
            val point = position.copy()
            val currentState = grid.getOrDefault(point, NodeState.CLEAN)

            // Update direction based on current state
            direction = when (currentState) {
                NodeState.CLEAN -> turnLeft(direction)
                NodeState.WEAKENED -> direction
                NodeState.INFECTED -> turnRight(direction)
                NodeState.FLAGGED -> reverse(direction)
            }

            // Update node state
            val newState = when (currentState) {
                NodeState.CLEAN -> NodeState.WEAKENED
                NodeState.WEAKENED -> {
                    infectionCount++
                    NodeState.INFECTED
                }

                NodeState.INFECTED -> NodeState.FLAGGED
                NodeState.FLAGGED -> NodeState.CLEAN
            }

            if (newState == NodeState.CLEAN) {
                grid.remove(point)
            } else {
                grid[point] = newState
            }

            position += direction
        }
        return infectionCount
    }

    private fun turnLeft(direction: Direction): Direction = when (direction) {
        Direction.UP -> Direction.LEFT
        Direction.DOWN -> Direction.RIGHT
        Direction.LEFT -> Direction.DOWN
        Direction.RIGHT -> Direction.UP
    }

    private fun turnRight(direction: Direction): Direction = when (direction) {
        Direction.UP -> Direction.RIGHT
        Direction.DOWN -> Direction.LEFT
        Direction.LEFT -> Direction.UP
        Direction.RIGHT -> Direction.DOWN
    }

    private fun reverse(direction: Direction): Direction = when (direction) {
        Direction.UP -> Direction.DOWN
        Direction.DOWN -> Direction.UP
        Direction.LEFT -> Direction.RIGHT
        Direction.RIGHT -> Direction.LEFT
    }

    override fun partOne(input: String): Int {
        val grid = parseInput(input)
        return simulateVirusPartOne(grid, 10000)
    }

    override fun partTwo(input: String): Int {
        val grid = parseInput(input)
        return simulateVirusPartTwo(grid, 10000000)
    }

}