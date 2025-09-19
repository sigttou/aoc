<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $card = (int)trim($input[0]);
    $door = (int)trim($input[1]);
    $subject = 7;
    $value = 1;
    $loop = 0;
    while ($value !== $card) {
        $value = ($value * $subject) % 20201227;
        $loop++;
    }
    // Now transform door's public key with card's loop size
    $value = 1;
    for ($i = 0; $i < $loop; $i++) {
        $value = ($value * $door) % 20201227;
    }
    return $value;
}

function part02(array $input)
{
    // Free star
    return 0;
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
    [1890859, 0], // Expected
    [$result01, $result02], // Result
);
