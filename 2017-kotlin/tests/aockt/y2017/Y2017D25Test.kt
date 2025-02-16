package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 25, "The Halting Problem")
class Y2017D25Test : AdventSpec<Y2017D25>({

    val example = """
        Begin in state A.
        Perform a diagnostic checksum after 6 steps.

        In state A:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state B.
          If the current value is 1:
            - Write the value 0.
            - Move one slot to the left.
            - Continue with state B.

        In state B:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the left.
            - Continue with state A.
          If the current value is 1:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state A.
    """.trimIndent()

    partOne {
        example shouldOutput 3
    }

    partTwo {
    }

})
