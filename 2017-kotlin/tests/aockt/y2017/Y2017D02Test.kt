package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 2, "Inverse Captcha")
class Y2017D02Test : AdventSpec<Y2017D02>({

    val example1 = """
        5 1 9 5
        7 5 3
        2 4 6 8
    """.trimIndent()

    val example2 = """
        5 9 2 8
        9 4 7 3
        3 8 6 5
    """.trimIndent()

    partOne {
        example1 shouldOutput 18
    }

    partTwo {
        example2 shouldOutput 9
    }

})
