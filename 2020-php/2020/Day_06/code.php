<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $sum = 0;
    $group = '';
    foreach ($input as $line) {
        if (trim($line) === '') {
            $sum += count(array_unique(str_split(str_replace("\n", '', $group))));
            $group = '';
        } else {
            $group .= $line;
        }
    }
    if ($group !== '') {
        $sum += count(array_unique(str_split(str_replace("\n", '', $group))));
    }
    return $sum;
}

function part02(array $input)
{
    $sum = 0;
    $group = [];
    foreach ($input as $line) {
        if (trim($line) === '') {
            if (count($group) > 0) {
                $common = array_count_values(str_split(implode('', $group)));
                $all_yes = 0;
                foreach ($common as $q => $cnt) {
                    if ($cnt === count($group)) {
                        $all_yes++;
                    }
                }
                $sum += $all_yes;
            }
            $group = [];
        } else {
            $group[] = $line;
        }
    }
    if (count($group) > 0) {
        $common = array_count_values(str_split(implode('', $group)));
        $all_yes = 0;
        foreach ($common as $q => $cnt) {
            if ($cnt === count($group)) {
                $all_yes++;
            }
        }
        $sum += $all_yes;
    }
    return $sum;
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
    [7120, 3570], // Expected
    [$result01, $result02], // Result
);
