package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 18, "Duet")
class Y2017D18Test : AdventSpec<Y2017D18>({

    val example = """
        set a 1
        add a 2
        mul a a
        mod a 5
        snd a
        set a 0
        rcv a
        jgz a -1
        set a 1
        jgz a -2
    """.trimIndent()

    partOne {
        example shouldOutput 4
    }

    partTwo {
    }

})
