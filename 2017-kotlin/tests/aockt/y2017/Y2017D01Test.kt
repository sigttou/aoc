package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 1, "Inverse Captcha")
class Y2017D01Test : AdventSpec<Y2017D01>({

    partOne {
        "1122" shouldOutput 3
        "1111" shouldOutput 4
        "1234" shouldOutput 0
        "91212129" shouldOutput 9
    }

    partTwo {
        "1212" shouldOutput 6
        "1221" shouldOutput 0
        "123425" shouldOutput 4
        "123123" shouldOutput 12
        "12131415" shouldOutput 4
    }

})
