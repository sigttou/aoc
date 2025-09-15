<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $contains = [];
    foreach ($input as $line) {
        if (!preg_match('/^([a-z ]+) bags contain (.+)\.$/', $line, $m)) {
            continue;
        }
        $outer = $m[1];
        $inner = $m[2];
        $contains[$outer] = [];
        if ($inner !== 'no other bags') {
            foreach (explode(',', $inner) as $desc) {
                if (preg_match('/\d+ ([a-z ]+) bag/', $desc, $mm)) {
                    $contains[$outer][] = $mm[1];
                }
            }
        }
    }
    $canContain = [];
    foreach ($contains as $outer => $_) {
        if (canContainGold($outer, $contains)) {
            $canContain[] = $outer;
        }
    }
    return count($canContain);
}

function canContainGold($bag, $contains, &$memo = [])
{
    if (isset($memo[$bag])) {
        return $memo[$bag];
    }
    if (!isset($contains[$bag])) {
        return false;
    }
    if (in_array('shiny gold', $contains[$bag])) {
        return $memo[$bag] = true;
    }
    foreach ($contains[$bag] as $inner) {
        if (canContainGold($inner, $contains, $memo)) {
            return $memo[$bag] = true;
        }
    }
    return $memo[$bag] = false;
}

function part02(array $input)
{
    $contains = [];
    foreach ($input as $line) {
        if (!preg_match('/^([a-z ]+) bags contain (.+)\.$/', $line, $m)) {
            continue;
        }
        $outer = $m[1];
        $inner = $m[2];
        $contains[$outer] = [];
        if ($inner !== 'no other bags') {
            foreach (explode(',', $inner) as $desc) {
                if (preg_match('/(\d+) ([a-z ]+) bag/', $desc, $mm)) {
                    $contains[$outer][$mm[2]] = (int)$mm[1];
                }
            }
        }
    }
    return countBags('shiny gold', $contains);
}

function countBags($bag, $contains, &$memo = [])
{
    if (isset($memo[$bag])) {
        return $memo[$bag];
    }
    if (!isset($contains[$bag]) || empty($contains[$bag])) {
        return $memo[$bag] = 0;
    }
    $sum = 0;
    foreach ($contains[$bag] as $inner => $num) {
        $sum += $num * (1 + countBags($inner, $contains, $memo));
    }
    return $memo[$bag] = $sum;
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
    [177, 34988], // Expected
    [$result01, $result02], // Result
);
