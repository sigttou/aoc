package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 22, "Sporifica Virus")
class Y2017D22Test : AdventSpec<Y2017D22>({

    val example = """
        ..#
        #..
        ...
    """.trimIndent()

    partOne {
        example shouldOutput 5587
    }

    partTwo {
        example shouldOutput 2511944
    }

})
