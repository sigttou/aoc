<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $valid = 0;
    foreach ($input as $line) {
        if (!preg_match('/^(\d+)-(\d+) ([a-z]): ([a-z]+)$/', trim($line), $matches)) {
            continue;
        }
        $min = (int)$matches[1];
        $max = (int)$matches[2];
        $char = $matches[3];
        $password = $matches[4];
        $count = substr_count($password, $char);
        if ($count >= $min && $count <= $max) {
            $valid++;
        }
    }
    return $valid;
}

function part02(array $input)
{
    $valid = 0;
    foreach ($input as $line) {
        if (!preg_match('/^(\d+)-(\d+) ([a-z]): ([a-z]+)$/', trim($line), $matches)) {
            continue;
        }
        $pos1 = (int)$matches[1] - 1;
        $pos2 = (int)$matches[2] - 1;
        $char = $matches[3];
        $password = $matches[4];
        $match1 = isset($password[$pos1]) && $password[$pos1] === $char;
        $match2 = isset($password[$pos2]) && $password[$pos2] === $char;
        if ($match1 xor $match2) {
            $valid++;
        }
    }
    return $valid;
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
    [474, 745], // Expected
    [$result01, $result02], // Result
);
