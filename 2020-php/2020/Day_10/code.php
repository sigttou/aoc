<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $adapters = array_map('intval', $input);
    sort($adapters);
    array_unshift($adapters, 0); // charging outlet
    $adapters[] = max($adapters) + 3; // device
    $diffs = [1 => 0, 2 => 0, 3 => 0];
    for ($i = 1; $i < count($adapters); $i++) {
        $diff = $adapters[$i] - $adapters[$i - 1];
        $diffs[$diff]++;
    }
    return $diffs[1] * $diffs[3];
}

function part02(array $input)
{
    $adapters = array_map('intval', $input);
    sort($adapters);
    array_unshift($adapters, 0); // charging outlet
    $adapters[] = max($adapters) + 3; // device
    $ways = [0 => 1];
    foreach ($adapters as $adapter) {
        for ($j = 1; $j <= 3; $j++) {
            if (isset($ways[$adapter - $j])) {
                $ways[$adapter] = ($ways[$adapter] ?? 0) + $ways[$adapter - $j];
            }
        }
    }
    return $ways[max($adapters)];
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
    [3000, 193434623148032], // Expected
    [$result01, $result02], // Result
);
