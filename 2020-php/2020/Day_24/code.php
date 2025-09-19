<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    // Axial coordinates: e=(1,0), se=(0,1), sw=(-1,1), w=(-1,0), nw=(0,-1), ne=(1,-1)
    $dirs = [
        'e' => [1, 0], 'se' => [0, 1], 'sw' => [-1, 1],
        'w' => [-1, 0], 'nw' => [0, -1], 'ne' => [1, -1]
    ];
    $black = [];
    foreach ($input as $line) {
        $x = $y = 0;
        $i = 0;
        while ($i < strlen($line)) {
            if ($line[$i] === 'e' || $line[$i] === 'w') {
                $d = $line[$i];
                $i++;
            } else {
                $d = $line[$i] . $line[$i + 1];
                $i += 2;
            }
            $x += $dirs[$d][0];
            $y += $dirs[$d][1];
        }
        $key = "$x,$y";
        $black[$key] = !($black[$key] ?? false);
    }
    return array_sum($black);
}

function part02(array $input)
{
    $dirs = [
        'e' => [1, 0], 'se' => [0, 1], 'sw' => [-1, 1],
        'w' => [-1, 0], 'nw' => [0, -1], 'ne' => [1, -1]
    ];
    $black = [];
    foreach ($input as $line) {
        $x = $y = 0;
        $i = 0;
        while ($i < strlen($line)) {
            if ($line[$i] === 'e' || $line[$i] === 'w') {
                $d = $line[$i];
                $i++;
            } else {
                $d = $line[$i] . $line[$i + 1];
                $i += 2;
            }
            $x += $dirs[$d][0];
            $y += $dirs[$d][1];
        }
        $key = "$x,$y";
        $black[$key] = !($black[$key] ?? false);
    }
    for ($day = 1; $day <= 100; $day++) {
        $next = [];
        $to_check = [];
        foreach ($black as $k => $v) {
            if ($v) {
                [$x, $y] = explode(',', $k);
                $x = (int)$x;
                $y = (int)$y;
                $to_check[$k] = true;
                foreach ($dirs as $d) {
                    $nx = $x + $d[0];
                    $ny = $y + $d[1];
                    $to_check["$nx,$ny"] = true;
                }
            }
        }
        foreach ($to_check as $k => $_) {
            [$x, $y] = explode(',', $k);
            $x = (int)$x;
            $y = (int)$y;
            $cnt = 0;
            foreach ($dirs as $d) {
                $nx = $x + $d[0];
                $ny = $y + $d[1];
                if ($black["$nx,$ny"] ?? false) {
                    $cnt++;
                }
            }
            if ($black[$k] ?? false) {
                $next[$k] = ($cnt === 1 || $cnt === 2);
            } else {
                $next[$k] = ($cnt === 2);
            }
        }
        $black = $next;
    }
    return array_sum($black);
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
    [523, 4225], // Expected
    [$result01, $result02], // Result
);
