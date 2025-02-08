package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 15, "Dueling Generators")
class Y2017D15Test : AdventSpec<Y2017D15>({
    val example = """
Generator A starts with 65
Generator B starts with 8921
    """.trimIndent()
    partOne {
        example shouldOutput 588
    }

    partTwo {
        example shouldOutput 309
    }

})
