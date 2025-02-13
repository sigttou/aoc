package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 20, "Particle Swarm")
class Y2017D20Test : AdventSpec<Y2017D20>({

    val example = """
        p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>
        p=<4,0,0>, v=<0,0,0>, a=<-2,0,0>
        p=<4,0,0>, v=<1,0,0>, a=<-1,0,0>
        p=<2,0,0>, v=<-2,0,0>, a=<-2,0,0>
        p=<4,0,0>, v=<0,0,0>, a=<-1,0,0>
        p=<-2,0,0>, v=<-4,0,0>, a=<-2,0,0>
        p=<3,0,0>, v=<-1,0,0>, a=<-1,0,0>
        p=<-8,0,0>, v=<-6,0,0>, a=<-2,0,0>
    """.trimIndent()

    partOne {
        example shouldOutput 0
    }

    val example2 = """
        p=<-6,0,0>, v=<3,0,0>, a=<0,0,0>
        p=<-4,0,0>, v=<2,0,0>, a=<0,0,0>
        p=<-2,0,0>, v=<1,0,0>, a=<0,0,0>
        p=<3,0,0>, v=<-1,0,0>, a=<0,0,0>
        p=<-3,0,0>, v=<3,0,0>, a=<0,0,0>
        p=<-2,0,0>, v=<2,0,0>, a=<0,0,0>
        p=<-1,0,0>, v=<1,0,0>, a=<0,0,0>
        p=<2,0,0>, v=<-1,0,0>, a=<0,0,0>
        p=<0,0,0>, v=<3,0,0>, a=<0,0,0>
        p=<0,0,0>, v=<2,0,0>, a=<0,0,0>
        p=<0,0,0>, v=<1,0,0>, a=<0,0,0>
        p=<1,0,0>, v=<-1,0,0>, a=<0,0,0>
        p=<0,0,0>, v=<-1,0,0>, a=<0,0,0>
    """.trimIndent()

    partTwo {
        example2 shouldOutput 1
    }

})
