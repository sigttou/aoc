<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $mem = [];
    $mask = '';
    foreach ($input as $line) {
        if (preg_match('/mask = ([X01]{36})/', $line, $m)) {
            $mask = $m[1];
        } elseif (preg_match('/mem\[(\d+)\] = (\d+)/', $line, $m)) {
            $addr = (int)$m[1];
            $val = (int)$m[2];
            $valBin = str_pad(decbin($val), 36, '0', STR_PAD_LEFT);
            $masked = '';
            for ($i = 0; $i < 36; $i++) {
                $masked .= ($mask[$i] === 'X') ? $valBin[$i] : $mask[$i];
            }
            $mem[$addr] = bindec($masked);
        }
    }
    return array_sum($mem);
}

function part02(array $input)
{
    $mem = [];
    $mask = '';
    foreach ($input as $line) {
        if (preg_match('/mask = ([X01]{36})/', $line, $m)) {
            $mask = $m[1];
        } elseif (preg_match('/mem\[(\d+)\] = (\d+)/', $line, $m)) {
            $addr = (int)$m[1];
            $val = (int)$m[2];
            $addrBin = str_pad(decbin($addr), 36, '0', STR_PAD_LEFT);
            $floating = '';
            for ($i = 0; $i < 36; $i++) {
                if ($mask[$i] === '0') {
                    $floating .= $addrBin[$i];
                } elseif ($mask[$i] === '1') {
                    $floating .= '1';
                } else {
                    $floating .= 'X';
                }
            }
            $addrs = expandFloating($floating);
            foreach ($addrs as $a) {
                $mem[bindec($a)] = $val;
            }
        }
    }
    return array_sum($mem);
}

function expandFloating($addr)
{
    $results = [''];
    for ($i = 0; $i < strlen($addr); $i++) {
        $next = [];
        foreach ($results as $r) {
            if ($addr[$i] === 'X') {
                $next[] = $r . '0';
                $next[] = $r . '1';
            } else {
                $next[] = $r . $addr[$i];
            }
        }
        $results = $next;
    }
    return $results;
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
    [5055782549997, 4795970362286], // Expected
    [$result01, $result02], // Result
);
