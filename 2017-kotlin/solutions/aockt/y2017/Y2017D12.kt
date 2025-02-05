package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D12 : Solution {

    private fun parse(input: String): Map<Int, List<Int>> {
        val regex = Regex("""(\d+) <-> (.+)""")
        val graph = mutableMapOf<Int, List<Int>>()

        input.lines().forEach { line ->
            val match = regex.matchEntire(line) ?: throw IllegalArgumentException("Invalid input format")
            val (node, neighbors) = match.destructured
            graph[node.toInt()] = neighbors.split(", ").map { it.toInt() }
        }

        return graph
    }

    private fun findGroup(graph: Map<Int, List<Int>>, start: Int, visited: MutableSet<Int>) {
        val stack = mutableListOf(start)

        while (stack.isNotEmpty()) {
            val node = stack.removeAt(stack.size - 1)
            if (node !in visited) {
                visited.add(node)
                stack.addAll(graph[node] ?: emptyList())
            }
        }
    }


    override fun partOne(input: String): Int {
        val graph = parse(input)
        val visited = mutableSetOf<Int>()
        findGroup(graph, 0, visited)
        return visited.size
    }

    override fun partTwo(input: String): Int {
        val graph = parse(input)
        val visited = mutableSetOf<Int>()
        var groupCount = 0

        for (node in graph.keys) {
            if (node !in visited) {
                findGroup(graph, node, visited)
                groupCount++
            }
        }

        return groupCount
    }
}