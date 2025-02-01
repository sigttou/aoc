package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D08 : Solution {

    private data class Instruction(
        val register: String,
        val operation: String,
        val amount: Int,
        val conditionRegister: String,
        val condition: String,
        val conditionValue: Int
    )

    private fun parse(input: String): List<Instruction> {
        val regex = Regex("""(\w+) (inc|dec) (-?\d+) if (\w+) ([!=><]=?) (-?\d+)""")
        return input.lines().map { line ->
            val match = regex.matchEntire(line) ?: throw IllegalArgumentException("Invalid input format")
            val (register, operation, amount, conditionRegister, condition, conditionValue) = match.destructured
            Instruction(register, operation, amount.toInt(), conditionRegister, condition, conditionValue.toInt())
        }
    }

    private fun solve(instructions: List<Instruction>, part2: Boolean): Int {
        val registers = mutableMapOf<String, Int>().withDefault { 0 }
        var maxEverValue = Int.MIN_VALUE

        for (instruction in instructions) {
            val conditionRegisterValue = registers.getValue(instruction.conditionRegister)
            val conditionMet = when (instruction.condition) {
                ">" -> conditionRegisterValue > instruction.conditionValue
                "<" -> conditionRegisterValue < instruction.conditionValue
                ">=" -> conditionRegisterValue >= instruction.conditionValue
                "<=" -> conditionRegisterValue <= instruction.conditionValue
                "==" -> conditionRegisterValue == instruction.conditionValue
                "!=" -> conditionRegisterValue != instruction.conditionValue
                else -> throw IllegalArgumentException("Invalid condition operator: ${instruction.condition}")
            }

            if (conditionMet) {
                val currentValue = registers.getValue(instruction.register)
                val newValue = when (instruction.operation) {
                    "inc" -> currentValue + instruction.amount
                    "dec" -> currentValue - instruction.amount
                    else -> throw IllegalArgumentException("Invalid operation: ${instruction.operation}")
                }
                registers[instruction.register] = newValue
                if (newValue > maxEverValue) {
                    maxEverValue = newValue
                }
            }
        }

        val maxRegisterValue = registers.values.maxOrNull() ?: 0
        if (part2) {
            return maxEverValue
        }
        return maxRegisterValue
    }

    override fun partOne(input: String): Int {
        return solve(parse(input), false)
    }

    override fun partTwo(input: String): Int {
        return solve(parse(input), true)
    }
}