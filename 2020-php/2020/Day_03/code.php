<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $trees = 0;
    $x = 0;
    $width = strlen($input[0]);
    foreach ($input as $y => $line) {
        if ($y === 0) {
            continue;
        }
        $x = ($x + 3) % $width;
        if (isset($line[$x]) && $line[$x] === '#') {
            $trees++;
        }
    }
    return $trees;
}

function part02(array $input)
{
    $slopes = [
        [1, 1],
        [3, 1],
        [5, 1],
        [7, 1],
        [1, 2],
    ];
    $width = strlen($input[0]);
    $height = count($input);
    $result = 1;
    foreach ($slopes as [$dx, $dy]) {
        $x = 0;
        $trees = 0;
        for ($y = 0; $y < $height; $y += $dy) {
            if ($y === 0) {
                continue;
            }
            $x = ($x + $dx) % $width;
            if (isset($input[$y][$x]) && $input[$y][$x] === '#') {
                $trees++;
            }
        }
        $result *= $trees;
    }
    return $result;
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
    [294, 5774564250], // Expected
    [$result01, $result02], // Result
);
