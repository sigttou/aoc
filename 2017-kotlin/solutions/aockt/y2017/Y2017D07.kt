package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D07 : Solution {

    private data class Program(val name: String, val weight: Int, val children: List<String>)

    private fun parse(input: String): List<Program> {
        val regex = Regex("""(\w+) \((\d+)\)(?: -> (.*))?""")
        return input.lines().map { line ->
            val match = regex.matchEntire(line) ?: throw IllegalArgumentException("Invalid input format")
            val (name, weight, children) = match.destructured
            Program(name, weight.toInt(), if (children.isEmpty()) emptyList() else children.split(", "))
        }
    }

    private fun buildTree(programs: List<Program>): Map<String, Program> {
        return programs.associateBy { it.name }
    }

    private fun findRoot(programs: List<Program>): String {
        val allChildren = programs.flatMap { it.children }.toSet()
        return programs.first { it.name !in allChildren }.name
    }

    private fun calculateTotalWeight(program: Program, tree: Map<String, Program>, weights: MutableMap<String, Int>): Int {
        if (program.name in weights) return weights[program.name]!!

        val totalWeight = program.weight + program.children.sumOf { calculateTotalWeight(tree[it]!!, tree, weights) }
        weights[program.name] = totalWeight
        return totalWeight
    }

    private fun findUnbalancedProgram(program: Program, tree: Map<String, Program>, weights: MutableMap<String, Int>): Pair<String, Int>? {
        val childWeights = program.children.groupBy { calculateTotalWeight(tree[it]!!, tree, weights) }
        if (childWeights.size == 1) return null // Already balanced

        // Find the unbalanced child
        val (unbalancedWeight, children) = childWeights.entries.first { it.value.size == 1 }
        val unbalancedChild = children.first()
        val unbalancedProgram = tree[unbalancedChild]!!

        // Check if the child's subtree is unbalanced or if the child itself is the problem
        val deeperUnbalance = findUnbalancedProgram(unbalancedProgram, tree, weights)
        if (deeperUnbalance != null) return deeperUnbalance

        // Calculate the weight difference to balance
        val correctWeight = childWeights.entries.first { it.value.size > 1 }.key
        val weightDifference = correctWeight - unbalancedWeight
        return Pair(unbalancedProgram.name, unbalancedProgram.weight + weightDifference)
    }

    override fun partOne(input: String): String {
        val programs = parse(input)
        return findRoot(programs)
    }

    override fun partTwo(input: String): Int {
        val programs = parse(input)
        val tree = buildTree(programs)
        val rootName = findRoot(programs)
        val weights = mutableMapOf<String, Int>()
        val rootProgram = tree[rootName]!!

        val (_, correctedWeight) = findUnbalancedProgram(rootProgram, tree, weights)!!
        return correctedWeight
    }
}