package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D24 : Solution {

    private data class Component(val port1: Int, val port2: Int) {
        fun strength() = port1 + port2
        fun hasPort(port: Int) = port1 == port || port2 == port
        fun otherPort(port: Int) = if (port1 == port) port2 else port1
    }

    private data class Bridge(val length: Int, val strength: Int)

    private fun parse(input: String): List<Component> {
        return input.lines().map { line ->
            val (port1, port2) = line.split("/").map { it.toInt() }
            Component(port1, port2)
        }
    }

    private fun findStrongestBridge(
        components: List<Component>,
        used: Set<Component> = setOf(),
        currentPort: Int = 0
    ): Int {
        val availableComponents = components.filter {
            it !in used && it.hasPort(currentPort)
        }

        if (availableComponents.isEmpty()) {
            return 0
        }

        return availableComponents.maxOf { component ->
            val nextPort = component.otherPort(currentPort)
            component.strength() + findStrongestBridge(
                components,
                used + component,
                nextPort
            )
        }
    }

    private fun findBestBridge(
        components: List<Component>,
        used: Set<Component> = setOf(),
        currentPort: Int = 0
    ): Bridge {
        val availableComponents = components.filter {
            it !in used && it.hasPort(currentPort)
        }

        if (availableComponents.isEmpty()) {
            return Bridge(used.size, used.sumOf { it.strength() })
        }

        return availableComponents.map { component ->
            val nextPort = component.otherPort(currentPort)
            val subBridge = findBestBridge(
                components,
                used + component,
                nextPort
            )
            Bridge(
                subBridge.length,
                subBridge.strength
            )
        }.maxWith(compareBy({ it.length }, { it.strength }))
    }

    override fun partOne(input: String): Int {
        return findStrongestBridge(parse(input))
    }

    override fun partTwo(input: String): Int {
        return findBestBridge(parse(input)).strength
    }
}