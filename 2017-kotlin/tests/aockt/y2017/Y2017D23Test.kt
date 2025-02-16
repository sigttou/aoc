package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 23, "Coprocessor Conflagration")
class Y2017D23Test : AdventSpec<Y2017D23>({

    val example = """
        set a 10
        mul b 10
    """.trimIndent()

    partOne {
        example shouldOutput 1
    }

    partTwo {
    }

})
