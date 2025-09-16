<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $preamble = 25;
    $numbers = array_map('intval', $input);
    for ($i = $preamble; $i < count($numbers); $i++) {
        $valid = false;
        for ($j = $i - $preamble; $j < $i - 1; $j++) {
            for ($k = $j + 1; $k < $i; $k++) {
                if ($numbers[$j] + $numbers[$k] === $numbers[$i]) {
                    $valid = true;
                    break 2;
                }
            }
        }
        if (!$valid) {
            return $numbers[$i];
        }
    }
    return null;
}

function part02(array $input)
{
    $target = part01($input);
    $numbers = array_map('intval', $input);
    for ($start = 0; $start < count($numbers); $start++) {
        $sum = 0;
        for ($end = $start; $end < count($numbers); $end++) {
            $sum += $numbers[$end];
            if ($sum === $target && $end > $start) {
                $range = array_slice($numbers, $start, $end - $start + 1);
                return min($range) + max($range);
            }
            if ($sum > $target) {
                break;
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
    [1930745883, 268878261], // Expected
    [$result01, $result02], // Result
);
