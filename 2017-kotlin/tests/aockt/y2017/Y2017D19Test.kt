package aockt.y2017

import io.github.jadarma.aockt.test.AdventDay
import io.github.jadarma.aockt.test.AdventSpec

@AdventDay(2017, 19, "A Series of Tubes")
class Y2017D19Test : AdventSpec<Y2017D19>({

    val example = """
     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ 
    """.trimIndent()

    partOne {
        example shouldOutput "ABCDEF"
    }

    partTwo {
        example shouldOutput 38
    }

})
