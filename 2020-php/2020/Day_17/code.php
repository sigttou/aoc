<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $active = [];
    foreach ($input as $y => $line) {
        $line = trim($line);
        for ($x = 0; $x < strlen($line); $x++) {
            if ($line[$x] === '#') {
                $active["$x,$y,0"] = true;
            }
        }
    }
    for ($cycle = 0; $cycle < 6; $cycle++) {
        $new = [];
        $counts = [];
        foreach ($active as $pos => $_) {
            [$x, $y, $z] = array_map('intval', explode(',', $pos));
            for ($dx = -1; $dx <= 1; $dx++) {
                for ($dy = -1; $dy <= 1; $dy++) {
                    for ($dz = -1; $dz <= 1; $dz++) {
                        if ($dx === 0 && $dy === 0 && $dz === 0) {
                            continue;
                        }
                        $np = ($x + $dx) . ',' . ($y + $dy) . ',' . ($z + $dz);
                        $counts[$np] = ($counts[$np] ?? 0) + 1;
                    }
                }
            }
        }
        foreach ($counts as $pos => $cnt) {
            if (!empty($active[$pos]) && ($cnt === 2 || $cnt === 3)) {
                $new[$pos] = true;
            }
            if (empty($active[$pos]) && $cnt === 3) {
                $new[$pos] = true;
            }
        }
        $active = $new;
    }
    return count($active);
}

function part02(array $input)
{
    $active = [];
    foreach ($input as $y => $line) {
        $line = trim($line);
        for ($x = 0; $x < strlen($line); $x++) {
            if ($line[$x] === '#') {
                $active["$x,$y,0,0"] = true;
            }
        }
    }
    for ($cycle = 0; $cycle < 6; $cycle++) {
        $new = [];
        $counts = [];
        foreach ($active as $pos => $_) {
            [$x, $y, $z, $w] = array_map('intval', explode(',', $pos));
            for ($dx = -1; $dx <= 1; $dx++) {
                for ($dy = -1; $dy <= 1; $dy++) {
                    for ($dz = -1; $dz <= 1; $dz++) {
                        for ($dw = -1; $dw <= 1; $dw++) {
                            if ($dx === 0 && $dy === 0 && $dz === 0 && $dw === 0) {
                                continue;
                            }
                            $np = ($x + $dx) . ',' . ($y + $dy) . ',' . ($z + $dz) . ',' . ($w + $dw);
                            $counts[$np] = ($counts[$np] ?? 0) + 1;
                        }
                    }
                }
            }
        }
        foreach ($counts as $pos => $cnt) {
            if (!empty($active[$pos]) && ($cnt === 2 || $cnt === 3)) {
                $new[$pos] = true;
            }
            if (empty($active[$pos]) && $cnt === 3) {
                $new[$pos] = true;
            }
        }
        $active = $new;
    }
    return count($active);
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
    [273, 1504], // Expected
    [$result01, $result02], // Result
);
