package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 8, "I Heard You Like Registers")
class Y2017D08Test : AdventSpec<Y2017D08>({
    val example = """
        b inc 5 if a > 1
        a inc 1 if b < 5
        c dec -10 if a >= 1
        c inc -20 if c == 10
    """.trimIndent()
    partOne {
        example shouldOutput 1
    }

    partTwo {
        example shouldOutput 10
    }

})
