<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $maxId = 0;
    foreach ($input as $line) {
        $line = trim($line);
        if ($line === '') {
            continue;
        }
        $row = bindec(strtr(substr($line, 0, 7), 'FB', '01'));
        $col = bindec(strtr(substr($line, 7, 3), 'LR', '01'));
        $seatId = $row * 8 + $col;
        if ($seatId > $maxId) {
            $maxId = $seatId;
        }
    }
    return $maxId;
}

function part02(array $input)
{
    $ids = [];
    foreach ($input as $line) {
        $line = trim($line);
        if ($line === '') {
            continue;
        }
        $row = bindec(strtr(substr($line, 0, 7), 'FB', '01'));
        $col = bindec(strtr(substr($line, 7, 3), 'LR', '01'));
        $ids[] = $row * 8 + $col;
    }
    sort($ids);
    for ($i = 1; $i < count($ids) - 1; $i++) {
        if ($ids[$i] + 1 !== $ids[$i + 1]) {
            return $ids[$i] + 1;
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
    [850, 599], // Expected
    [$result01, $result02], // Result
);
