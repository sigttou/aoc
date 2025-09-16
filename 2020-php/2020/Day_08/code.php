<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $acc = 0;
    $ip = 0;
    $visited = [];
    $instructions = array_map('trim', $input);
    while ($ip < count($instructions)) {
        if (isset($visited[$ip])) {
            break;
        }
        $visited[$ip] = true;
        list($op, $arg) = explode(' ', $instructions[$ip]);
        $arg = (int)$arg;
        if ($op === 'acc') {
            $acc += $arg;
            $ip++;
        } elseif ($op === 'jmp') {
            $ip += $arg;
        } else {
            $ip++;
        }
    }
    return $acc;
}

function part02(array $input)
{
    $instructions = array_map('trim', $input);
    for ($i = 0; $i < count($instructions); $i++) {
        $orig = $instructions[$i];
        if (strpos($orig, 'jmp') === 0) {
            $instructions[$i] = 'nop' . substr($orig, 3);
        } elseif (strpos($orig, 'nop') === 0) {
            $instructions[$i] = 'jmp' . substr($orig, 3);
        } else {
            continue;
        }
        $acc = 0;
        $ip = 0;
        $visited = [];
        while ($ip < count($instructions)) {
            if (isset($visited[$ip])) {
                break;
            }
            $visited[$ip] = true;
            list($op, $arg) = explode(' ', $instructions[$ip]);
            $arg = (int)$arg;
            if ($op === 'acc') {
                $acc += $arg;
                $ip++;
            } elseif ($op === 'jmp') {
                $ip += $arg;
            } else {
                $ip++;
            }
        }
        if ($ip === count($instructions)) {
            return $acc;
        }
        $instructions[$i] = $orig;
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
    [1797, 1036], // Expected
    [$result01, $result02], // Result
);
