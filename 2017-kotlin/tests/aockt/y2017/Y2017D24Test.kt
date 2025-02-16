package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 24, "Electromagnetic Moat")
class Y2017D24Test : AdventSpec<Y2017D24>({

    val example = """
        0/2
        2/2
        2/3
        3/4
        3/5
        0/1
        10/1
        9/10
    """.trimIndent()

    partOne {
        example shouldOutput 31
    }

    partTwo {
        example shouldOutput 19
    }

})
