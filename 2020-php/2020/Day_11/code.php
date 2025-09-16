<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $grid = array_map('str_split', $input);
    $height = count($grid);
    $width = count($grid[0]);
    $changed = true;
    while ($changed) {
        $changed = false;
        $new = $grid;
        for ($y = 0; $y < $height; $y++) {
            for ($x = 0; $x < $width; $x++) {
                if ($grid[$y][$x] === '.') {
                    continue;
                }
                $occ = 0;
                for ($dy = -1; $dy <= 1; $dy++) {
                    for ($dx = -1; $dx <= 1; $dx++) {
                        if ($dx === 0 && $dy === 0) {
                            continue;
                        }
                        $ny = $y + $dy;
                        $nx = $x + $dx;
                        if ($ny >= 0 && $ny < $height && $nx >= 0 && $nx < $width && $grid[$ny][$nx] === '#') {
                            $occ++;
                        }
                    }
                }
                if ($grid[$y][$x] === 'L' && $occ === 0) {
                    $new[$y][$x] = '#';
                    $changed = true;
                } elseif ($grid[$y][$x] === '#' && $occ >= 4) {
                    $new[$y][$x] = 'L';
                    $changed = true;
                }
            }
        }
        $grid = $new;
    }
    $count = 0;
    foreach ($grid as $row) {
        foreach ($row as $c) {
            if ($c === '#') {
                $count++;
            }
        }
    }
    return $count;
}

function part02(array $input)
{
    $grid = array_map('str_split', $input);
    $height = count($grid);
    $width = count($grid[0]);
    $changed = true;
    $dirs = [];
    for ($dy = -1; $dy <= 1; $dy++) {
        for ($dx = -1; $dx <= 1; $dx++) {
            if ($dx === 0 && $dy === 0) {
                continue;
            }
            $dirs[] = [$dx, $dy];
        }
    }
    while ($changed) {
        $changed = false;
        $new = $grid;
        for ($y = 0; $y < $height; $y++) {
            for ($x = 0; $x < $width; $x++) {
                if ($grid[$y][$x] === '.') {
                    continue;
                }
                $occ = 0;
                foreach ($dirs as [$dx, $dy]) {
                    $nx = $x + $dx;
                    $ny = $y + $dy;
                    while ($nx >= 0 && $nx < $width && $ny >= 0 && $ny < $height) {
                        if ($grid[$ny][$nx] === 'L') {
                            break;
                        }
                        if ($grid[$ny][$nx] === '#') {
                            $occ++;
                            break;
                        }
                        $nx += $dx;
                        $ny += $dy;
                    }
                }
                if ($grid[$y][$x] === 'L' && $occ === 0) {
                    $new[$y][$x] = '#';
                    $changed = true;
                } elseif ($grid[$y][$x] === '#' && $occ >= 5) {
                    $new[$y][$x] = 'L';
                    $changed = true;
                }
            }
        }
        $grid = $new;
    }
    $count = 0;
    foreach ($grid as $row) {
        foreach ($row as $c) {
            if ($c === '#') {
                $count++;
            }
        }
    }
    return $count;
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
    [2243, 2027], // Expected
    [$result01, $result02], // Result
);
