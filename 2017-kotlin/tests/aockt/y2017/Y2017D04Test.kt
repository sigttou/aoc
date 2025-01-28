package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 4, "Inverse Captcha")
class Y2017D04Test : AdventSpec<Y2017D04>({
    val example = """
        aa bb cc dd ee
        aa bb cc dd aa
        aa bb cc dd aaa
    """.trimIndent()
    partOne {
        example shouldOutput 2
    }

    val example2 = """
        abcde fghij
        abcde xyz ecdab
        a ab abc abd abf abj
        iiii oiii ooii oooi oooo
        oiii ioii iioi iiio
    """.trimIndent()
    partTwo {
        example2 shouldOutput 3
    }

})
