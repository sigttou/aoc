package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 5, "A Maze of Twisty Trampolines, All Alike")
class Y2017D05Test : AdventSpec<Y2017D05>({
    val example = """
        0
        3
        0
        1
        -3
    """.trimIndent()
    partOne {
        example shouldOutput 5
    }

    partTwo {
        example shouldOutput 10
    }

})
