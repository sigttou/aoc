package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 12, "Digital Plumber")
class Y2017D12Test : AdventSpec<Y2017D12>({
    val example = """
        0 <-> 2
        1 <-> 1
        2 <-> 0, 3, 4
        3 <-> 2, 4
        4 <-> 2, 3, 6
        5 <-> 6
        6 <-> 4, 5
    """.trimIndent()
    partOne {
        example shouldOutput 6
    }

    partTwo {
        example shouldOutput 2
    }

})
