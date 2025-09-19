<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $cups = array_map('intval', str_split(trim($input[0])));
    $max = max($cups);
    $min = min($cups);
    $current = 0;
    for ($move = 0; $move < 100; $move++) {
        $cur = $cups[$current];
        $pick = [];
        $pick_idx = [];
        for ($i = 1; $i <= 3; $i++) {
            $idx = ($current + $i) % count($cups);
            $pick[] = $cups[$idx];
            $pick_idx[] = $idx;
        }
        // Remove picked up cups
        rsort($pick_idx);
        foreach ($pick_idx as $idx) {
            array_splice($cups, $idx, 1);
        }
        // Find destination
        $dest = $cur - 1;
        while (in_array($dest, $pick) || $dest < $min) {
            $dest--;
            if ($dest < $min) {
                $dest = $max;
            }
        }
        $dest_idx = array_search($dest, $cups);
        array_splice($cups, $dest_idx + 1, 0, $pick);
        $current = (array_search($cur, $cups) + 1) % count($cups);
    }
    // Find cup 1
    $idx1 = array_search(1, $cups);
    $out = '';
    for ($i = 1; $i < count($cups); $i++) {
        $out .= $cups[($idx1 + $i) % count($cups)];
    }
    return (int)$out;
}

function part02(array $input)
{
    $start = array_map('intval', str_split(trim($input[0])));
    $max = 1000000;
    $moves = 10000000;
    // Build linked list: $next[$label] = next label
    $next = [];
    $n = count($start);
    for ($i = 0; $i < $n - 1; $i++) {
        $next[$start[$i]] = $start[$i + 1];
    }
    $next[$start[$n - 1]] = $n + 1;
    for ($i = $n + 1; $i < $max; $i++) {
        $next[$i] = $i + 1;
    }
    $next[$max] = $start[0];
    $cur = $start[0];
    for ($move = 0; $move < $moves; $move++) {
        $pick1 = $next[$cur];
        $pick2 = $next[$pick1];
        $pick3 = $next[$pick2];
        $after = $next[$pick3];
        $dest = $cur - 1;
        if ($dest < 1) {
            $dest = $max;
        }
        while ($dest == $pick1 || $dest == $pick2 || $dest == $pick3) {
            $dest--;
            if ($dest < 1) {
                $dest = $max;
            }
        }
        $next[$cur] = $after;
        $next[$pick3] = $next[$dest];
        $next[$dest] = $pick1;
        $cur = $next[$cur];
    }
    $a = $next[1];
    $b = $next[$a];
    return $a * $b;
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
    [34952786, 505334281774], // Expected
    [$result01, $result02], // Result
);
