package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 11, "Hex Ed")
class Y2017D11Test : AdventSpec<Y2017D11>({
    partOne {
        "ne,ne,ne" shouldOutput 3
        "ne,ne,sw,sw" shouldOutput 0
        "ne,ne,s,s" shouldOutput 2
        "se,sw,se,sw,sw" shouldOutput 3
    }

    partTwo {
        "ne,ne,ne" shouldOutput 3
        "ne,ne,sw,sw" shouldOutput 2
        "ne,ne,s,s" shouldOutput 2
        "se,sw,se,sw,sw" shouldOutput 3
    }

})
