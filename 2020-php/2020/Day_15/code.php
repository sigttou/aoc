<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $nums = array_map('intval', explode(',', trim($input[0])));
    $spoken = [];
    $turn = 1;
    foreach ($nums as $n) {
        $spoken[$n] = $turn++;
        $last = $n;
    }
    unset($spoken[$last]); // last is not 'previously spoken' yet
    for (; $turn <= 2020; $turn++) {
        $prev = isset($spoken[$last]) ? $turn - 1 - $spoken[$last] : 0;
        $spoken[$last] = $turn - 1;
        $last = $prev;
    }
    return $last;
}

function part02(array $input)
{
    $nums = array_map('intval', explode(',', trim($input[0])));
    $spoken = [];
    $turn = 1;
    foreach ($nums as $n) {
        $spoken[$n] = $turn++;
        $last = $n;
    }
    unset($spoken[$last]);
    for (; $turn <= 30000000; $turn++) {
        $prev = isset($spoken[$last]) ? $turn - 1 - $spoken[$last] : 0;
        $spoken[$last] = $turn - 1;
        $last = $prev;
    }
    return $last;
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
    [700, 51358], // Expected
    [$result01, $result02], // Result
);
