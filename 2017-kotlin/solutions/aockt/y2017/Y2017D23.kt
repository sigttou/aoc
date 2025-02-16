package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D23 : Solution {

    private fun String.toValue(registers: MutableMap<Char, Long>): Long {
        return this.toLongOrNull() ?: registers.getOrDefault(this[0], 0)
    }

    private fun parse(input: String): List<String> {
        return input.lines()
    }

    private fun executeInstructions(instructions: List<String>): Int {
        val registers = mutableMapOf<Char, Long>()
        var pointer = 0
        var mulCount = 0

        while (pointer in instructions.indices) {
            val parts = instructions[pointer].split(" ")
            val instruction = parts[0]
            val x = parts[1]
            val y = parts.getOrNull(2)

            when (instruction) {
                "set" -> registers[x[0]] = y!!.toValue(registers)
                "sub" -> registers[x[0]] = registers.getOrDefault(x[0], 0) - y!!.toValue(registers)
                "mul" -> {
                    registers[x[0]] = registers.getOrDefault(x[0], 0) * y!!.toValue(registers)
                    mulCount++
                }
                "jnz" -> if (x.toValue(registers) != 0L) pointer += y!!.toValue(registers).toInt() - 1
            }

            pointer++
        }

        return mulCount
    }

    private fun analyzeProgram(lines: List<String>): Long {
        // Extract the key values from the program
        val b = lines[0].split(" ")[2].toLong() * 100 + 100000 // Initial value of b after the first few instructions
        val c = b + 17000 // Upper bound value set in register c

        // The program is checking for non-prime numbers between b and c, stepping by 17
        var h = 0L
        for (n in b..c step 17) {
            if (!isPrime(n)) {
                h++
            }
        }
        return h
    }

    private fun isPrime(n: Long): Boolean {
        if (n <= 1) return false
        if (n <= 3) return true
        if (n % 2 == 0L) return false

        val sqrtN = kotlin.math.sqrt(n.toDouble()).toLong()
        for (i in 3..sqrtN step 2) {
            if (n % i == 0L) return false
        }
        return true
    }

    override fun partOne(input: String): Int {
        return executeInstructions(parse(input))
    }

    override fun partTwo(input: String): Long {
        return analyzeProgram(parse(input))
    }
}