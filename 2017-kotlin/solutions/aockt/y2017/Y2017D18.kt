package aockt.y2017

import io.github.jadarma.aockt.core.Solution
import java.util.*

/** A solution to a fictitious puzzle used for testing. */
object Y2017D18 : Solution {

    private fun parse(input: String): List<String> {
        return input.lines()
    }

    private fun String.toValue(registers: MutableMap<Char, Long>): Long {
        return this.toLongOrNull() ?: registers.getOrDefault(this[0], 0)
    }

    private fun executeInstructions(instructions: List<String>): Long {
        val registers = mutableMapOf<Char, Long>()
        var lastSound = 0L
        var pointer = 0

        while (pointer in instructions.indices) {
            val parts = instructions[pointer].split(" ")
            val instruction = parts[0]
            val x = parts[1]
            val y = parts.getOrNull(2)

            when (instruction) {
                "snd" -> lastSound = x.toValue(registers)
                "set" -> registers[x[0]] = y!!.toValue(registers)
                "add" -> registers[x[0]] = registers.getOrDefault(x[0], 0) + y!!.toValue(registers)
                "mul" -> registers[x[0]] = registers.getOrDefault(x[0], 0) * y!!.toValue(registers)
                "mod" -> registers[x[0]] = registers.getOrDefault(x[0], 0) % y!!.toValue(registers)
                "rcv" -> if (x.toValue(registers) != 0L) return lastSound
                "jgz" -> if (x.toValue(registers) > 0) pointer += y!!.toValue(registers).toInt() - 1
            }

            pointer++
        }

        throw IllegalStateException("No rcv instruction executed with a non-zero value")
    }

    override fun partOne(input: String): Long {
        val instructions = parse(input)
        return executeInstructions(instructions)
    }

    private fun executeInstructionsPartTwo(
        instructions: List<String>,
        programId: Int,
        sendQueue: Queue<Long>,
        receiveQueue: Queue<Long>,
        sendCount: MutableMap<Int, Int>,
        pointer: Int,
        registers: MutableMap<Char, Long>
    ): Pair<Int, Boolean> {
        var currentPointer = pointer
        var isWaiting = false

        while (currentPointer in instructions.indices) {
            val parts = instructions[currentPointer].split(" ")
            val instruction = parts[0]
            val x = parts[1]
            val y = parts.getOrNull(2)

            when (instruction) {
                "snd" -> {
                    sendQueue.add(x.toValue(registers))
                    sendCount[programId] = sendCount.getOrDefault(programId, 0) + 1
                }
                "set" -> registers[x[0]] = y!!.toValue(registers)
                "add" -> registers[x[0]] = registers.getValue(x[0]) + y!!.toValue(registers)
                "mul" -> registers[x[0]] = registers.getValue(x[0]) * y!!.toValue(registers)
                "mod" -> registers[x[0]] = registers.getValue(x[0]) % y!!.toValue(registers)
                "rcv" -> {
                    if (receiveQueue.isEmpty()) {
                        isWaiting = true
                        return currentPointer to isWaiting
                    }
                    registers[x[0]] = receiveQueue.poll()
                }
                "jgz" -> if (x.toValue(registers) > 0) currentPointer += y!!.toValue(registers).toInt() - 1
            }

            currentPointer++
        }

        return currentPointer to isWaiting
    }



    override fun partTwo(input: String): Int {
        val instructions = input.lines()
        val queue0 = ArrayDeque<Long>()
        val queue1 = ArrayDeque<Long>()
        val sendCount = mutableMapOf(0 to 0, 1 to 0)

        val registers0 = mutableMapOf<Char, Long>().withDefault { 0 }
        val registers1 = mutableMapOf<Char, Long>().withDefault { 0 }
        registers0['p'] = 0
        registers1['p'] = 1

        var pointer0 = 0
        var pointer1 = 0

        var waiting0 : Boolean
        var waiting1 : Boolean

        while (true) {
            val (newPointer0, newWaiting0) = executeInstructionsPartTwo(
                instructions,
                0,
                queue1,
                queue0,
                sendCount,
                pointer0,
                registers0
            )
            pointer0 = newPointer0
            waiting0 = newWaiting0

            val (newPointer1, newWaiting1) = executeInstructionsPartTwo(
                instructions,
                1,
                queue0,
                queue1,
                sendCount,
                pointer1,
                registers1
            )
            pointer1 = newPointer1
            waiting1 = newWaiting1

            if (waiting0 && waiting1 && queue0.isEmpty() && queue1.isEmpty()) break
        }

        return sendCount[1] ?: 0
    }
}