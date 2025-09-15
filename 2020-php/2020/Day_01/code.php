<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $numbers = array_map('intval', $input);
    $seen = [];
    foreach ($numbers as $num) {
        $target = 2020 - $num;
        if (isset($seen[$target])) {
            return $num * $target;
        }
        $seen[$num] = true;
    }
    return null;
}

function part02(array $input)
{
    $numbers = array_map('intval', $input);
    $count = count($numbers);
    for ($i = 0; $i < $count - 2; $i++) {
        for ($j = $i + 1; $j < $count - 1; $j++) {
            for ($k = $j + 1; $k < $count; $k++) {
                if ($numbers[$i] + $numbers[$j] + $numbers[$k] === 2020) {
                    return $numbers[$i] * $numbers[$j] * $numbers[$k];
                }
            }
        }
    }
    return null;
}

// Execute
calcExecutionTime();
$result01 = part01($input);
$result02 = part02($input);
$executionTime = calcExecutionTime();

writeln('Solution Part 1: ' . $result01);
writeln('Solution Part 2: ' . $result02);
writeln('Execution time: ' . $executionTime);

saveBenchmarkTime($executionTime, __DIR__);

// Task test
testResults(
    [800139, 59885340], // Expected
    [$result01, $result02], // Result
);
