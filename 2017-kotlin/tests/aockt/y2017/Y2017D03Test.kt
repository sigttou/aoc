package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 3, "Spiral Memory")
class Y2017D03Test : AdventSpec<Y2017D03>({
    partOne {
        "1" shouldOutput 0
        "12" shouldOutput 3
        "23" shouldOutput 2
        "1024" shouldOutput 31
    }

    partTwo {
        "1" shouldOutput 2
        "2" shouldOutput 4
        "3" shouldOutput 4
        "4" shouldOutput 5
        "5" shouldOutput 10
    }

})
