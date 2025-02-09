package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 16, "Permutation Promenade")
class Y2017D16Test : AdventSpec<Y2017D16>({

    val example = """
        s1,x3/4,pe/b
    """.trimIndent()

    partOne {
        example shouldOutput "baedc"
    }

    partTwo {
        example shouldOutput "ceadb"
    }

})
