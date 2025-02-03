package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 10, "Knot Hash")
class Y2017D10Test : AdventSpec<Y2017D10>({
    partOne {
        "3, 4, 1, 5" shouldOutput 12
    }

    partTwo {
        // "" shouldOutput "a2582a3a0e66e6e86e3812dcb672a272"
        "AoC 2017" shouldOutput "33efeb34ea91902bb2f59c9920caa6cd"
        "1,2,3" shouldOutput "3efbe78a8d82f29979031a4aa0b16a9d"
        "1,2,4" shouldOutput "63960835bcdc130f0b66d7ff4f6a5a8e"
    }

})
