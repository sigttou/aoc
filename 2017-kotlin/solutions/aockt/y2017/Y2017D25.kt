package aockt.y2017

import io.github.jadarma.aockt.core.Solution

/** A solution to a fictitious puzzle used for testing. */
object Y2017D25 : Solution {

    private data class Action(
        val writeValue: Int,
        val moveDirection: Int, // 1 for right, -1 for left
        val nextState: Char
    )

    private data class State(
        val zeroAction: Action,
        val oneAction: Action
    )

    private class TuringMachine(
        private val states: Map<Char, State>,
        private val initialState: Char,
        private val checksumSteps: Int
    ) {
        private val tape = mutableMapOf<Int, Int>().withDefault { 0 }
        private var cursor = 0
        private var currentState = initialState
        private var steps = 0

        fun run(): Int {
            while (steps < checksumSteps) {
                step()
            }
            return tape.values.count { it == 1 }
        }

        private fun step() {
            val currentValue = tape.getValue(cursor)
            val state = states[currentState]!!
            val action = if (currentValue == 0) state.zeroAction else state.oneAction

            tape[cursor] = action.writeValue
            cursor += action.moveDirection
            currentState = action.nextState
            steps++
        }
    }

    private fun parse(input: String): TuringMachine {
        val lines = input.lines()

        // Parse initial state and steps
        val initialState = lines[0].split("state ")[1].trim('.').single()
        val checksumSteps = lines[1].split("after ")[1].split(" ")[0].toInt()

        // Parse states
        val states = mutableMapOf<Char, State>()
        var i = 3
        while (i < lines.size) {
            if (lines[i].startsWith("In state")) {
                val stateName = lines[i].split("state ")[1].trim(':').single()

                // Parse zero condition
                val zeroWrite = lines[i + 2].split("value ")[1].trim('.').toInt()
                val zeroMove = if (lines[i + 3].contains("right")) 1 else -1
                val zeroNext = lines[i + 4].split("state ")[1].trim('.').single()

                // Parse one condition
                val oneWrite = lines[i + 6].split("value ")[1].trim('.').toInt()
                val oneMove = if (lines[i + 7].contains("right")) 1 else -1
                val oneNext = lines[i + 8].split("state ")[1].trim('.').single()

                states[stateName] = State(
                    Action(zeroWrite, zeroMove, zeroNext),
                    Action(oneWrite, oneMove, oneNext)
                )

                i += 9
            } else {
                i++
            }
        }

        return TuringMachine(states, initialState, checksumSteps)
    }

    override fun partOne(input: String): Int {
        return parse(input).run()
    }

    override fun partTwo(input: String): Int {
        return 0
    }
}