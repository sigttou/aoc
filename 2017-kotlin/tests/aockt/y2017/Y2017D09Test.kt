package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 9, "Stream Processing")
class Y2017D09Test : AdventSpec<Y2017D09>({
    partOne {
        "{}" shouldOutput 1
        "{{{}}}" shouldOutput 6
        "{{},{}}" shouldOutput 5
        "{{{},{},{{}}}}" shouldOutput 16
        "{<a>,<a>,<a>,<a>}" shouldOutput 1
        "{{<ab>},{<ab>},{<ab>},{<ab>}}" shouldOutput 9
        "{{<!!>},{<!!>},{<!!>},{<!!>}}" shouldOutput 9
        "{{<a!>},{<a!>},{<a!>},{<ab>}}" shouldOutput 3
    }

    partTwo {
        "<>" shouldOutput 0
        "<random characters>" shouldOutput 17
        "<<<<>" shouldOutput 3
        "<{!>}>" shouldOutput 2
        "<!!>" shouldOutput 0
        "<!!!>>" shouldOutput 0
        "<{o\"i!a,<{i<a>" shouldOutput 10
    }

})
