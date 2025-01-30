package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 6, "Memory Reallocation")
class Y2017D06Test : AdventSpec<Y2017D06>({
    val example = """
        0 2 7 0
    """.trimIndent()
    partOne {
        example shouldOutput 5
    }

    partTwo {
        example shouldOutput 4
    }

})
