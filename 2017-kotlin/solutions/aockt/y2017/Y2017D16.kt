package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D16 : Solution {

    private fun parse(input: String): List<String> {
        return input.split(",")
    }

    private fun spin(programs: MutableList<Char>, x: Int) {
        val size = programs.size
        val cut = programs.subList(size - x, size).toList()
        programs.removeAll(cut)
        programs.addAll(0, cut)
    }

    private fun exchange(programs: MutableList<Char>, a: Int, b: Int) {
        val temp = programs[a]
        programs[a] = programs[b]
        programs[b] = temp
    }

    private fun partner(programs: MutableList<Char>, a: Char, b: Char) {
        val indexA = programs.indexOf(a)
        val indexB = programs.indexOf(b)
        exchange(programs, indexA, indexB)
    }

    private fun dance(programs: MutableList<Char>, moves: List<String>) {
        for (move in moves) {
            when {
                move.startsWith("s") -> {
                    val x = move.substring(1).toInt()
                    spin(programs, x)
                }
                move.startsWith("x") -> {
                    val (a, b) = move.substring(1).split("/").map { it.toInt() }
                    exchange(programs, a, b)
                }
                move.startsWith("p") -> {
                    val a = move[1]
                    val b = move[3]
                    partner(programs, a, b)
                }
            }
        }
    }

    override fun partOne(input: String): String {
        var programs = ('a'..'p').toMutableList()
        if (input.length < 100)
            programs = ('a'..'e').toMutableList()

        val moves = parse(input)
        dance(programs, moves)
        return programs.joinToString("")
    }

    override fun partTwo(input: String): String {
        var programs = ('a'..'p').toMutableList()
        var target = 1_000_000_000
        if (input.length < 100)
        {
            programs = ('a'..'e').toMutableList()
            target = 2
        }
        val moves = parse(input)
        val seen = mutableMapOf<String, Int>()

        for (i in 0 until target) {
            val current = programs.joinToString("")
            if (current in seen) {
                val cycleLength = i - seen[current]!!
                val remaining = (target - i) % cycleLength
                repeat(remaining) {
                    dance(programs, moves)
                }
                break
            }
            seen[current] = i
            dance(programs, moves)
        }

        return programs.joinToString("")
    }
}