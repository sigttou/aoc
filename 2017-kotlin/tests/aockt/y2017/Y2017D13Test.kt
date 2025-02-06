package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 13, "Packet Scanners")
class Y2017D13Test : AdventSpec<Y2017D13>({
    val example = """
        0: 3
        1: 2
        4: 4
        6: 4
    """.trimIndent()
    partOne {
        example shouldOutput 24
    }

    partTwo {
        example shouldOutput 10
    }

})
