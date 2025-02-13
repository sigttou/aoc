package aockt.y2017

import io.github.jadarma.aockt.core.Solution
import kotlin.math.abs

/** A solution to a fictitious puzzle used for testing. */
object Y2017D20 : Solution {

    data class Vector3(val x: Long, val y: Long, val z: Long) {
        operator fun plus(other: Vector3) = Vector3(x + other.x, y + other.y, z + other.z)
        fun manhattanDistance() = abs(x) + abs(y) + abs(z)
    }

    data class Particle(val id: Int, var position: Vector3, var velocity: Vector3, val acceleration: Vector3)

    private fun parse(input: String): List<Particle> {
        val regex = Regex("""p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>""")
        return input.lines().mapIndexed { index, line ->
            val match = regex.matchEntire(line) ?: error("Invalid input format")
            val (px, py, pz, vx, vy, vz, ax, ay, az) = match.destructured
            Particle(
                id = index,
                position = Vector3(px.toLong(), py.toLong(), pz.toLong()),
                velocity = Vector3(vx.toLong(), vy.toLong(), vz.toLong()),
                acceleration = Vector3(ax.toLong(), ay.toLong(), az.toLong())
            )
        }
    }

    private fun updateParticle(particle: Particle) {
        particle.velocity += particle.acceleration
        particle.position += particle.velocity
    }

    private fun findClosestParticle(particles: List<Particle>, ticks: Int): Int {
        repeat(ticks) {
            particles.forEach(::updateParticle)
        }
        return particles.minByOrNull { it.position.manhattanDistance() }?.id ?: -1
    }

    private fun resolveCollisions(particles: MutableList<Particle>, ticks: Int): Int {
        repeat(ticks) {
            particles.forEach(::updateParticle)
            val positions = particles.groupBy { it.position }
            val collided = positions.filter { it.value.size > 1 }.values.flatten()
            particles.removeAll(collided.toSet())
        }
        return particles.size
    }

    override fun partOne(input: String): Int {
        return findClosestParticle(parse(input), input.lines().size)
    }

    override fun partTwo(input: String): Int {
        return resolveCollisions(parse(input).toMutableList(), input.lines().size)
    }
}