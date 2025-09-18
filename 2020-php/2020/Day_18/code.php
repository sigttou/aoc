<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $sum = 0;
    foreach ($input as $expr) {
        $sum += eval_expr1($expr);
    }
    return $sum;
}

function eval_expr1($expr)
{
    $expr = str_replace(' ', '', $expr);
    return eval_expr1_inner($expr, 0)[0];
}

function eval_expr1_inner($expr, $i)
{
    $acc = null;
    $op = null;
    while ($i < strlen($expr)) {
        $c = $expr[$i];
        if (ctype_digit($c)) {
            $num = 0;
            while ($i < strlen($expr) && ctype_digit($expr[$i])) {
                $num = $num * 10 + (int)$expr[$i];
                $i++;
            }
            $i--;
            $acc = ($acc === null) ? $num : ($op === '+' ? $acc + $num : $acc * $num);
        } elseif ($c === '+') {
            $op = '+';
        } elseif ($c === '*') {
            $op = '*';
        } elseif ($c === '(') {
            [$val, $ni] = eval_expr1_inner($expr, $i + 1);
            $acc = ($acc === null) ? $val : ($op === '+' ? $acc + $val : $acc * $val);
            $i = $ni;
        } elseif ($c === ')') {
            return [$acc, $i];
        }
        $i++;
    }
    return [$acc, $i];
}

function part02(array $input)
{
    $sum = 0;
    foreach ($input as $expr) {
        $sum += eval_expr2($expr);
    }
    return $sum;
}

function eval_expr2($expr)
{
    $expr = str_replace(' ', '', $expr);
    return eval_expr2_inner($expr, 0)[0];
}

function eval_expr2_inner($expr, $i)
{
    $vals = [];
    $ops = [];
    while ($i < strlen($expr)) {
        $c = $expr[$i];
        if (ctype_digit($c)) {
            $num = 0;
            while ($i < strlen($expr) && ctype_digit($expr[$i])) {
                $num = $num * 10 + (int)$expr[$i];
                $i++;
            }
            $i--;
            $vals[] = $num;
        } elseif ($c === '+') {
            $ops[] = '+';
        } elseif ($c === '*') {
            $ops[] = '*';
        } elseif ($c === '(') {
            [$val, $ni] = eval_expr2_inner($expr, $i + 1);
            $vals[] = $val;
            $i = $ni;
        } elseif ($c === ')') {
            break;
        }
        $i++;
    }
    // First, resolve all additions
    for ($j = 0; $j < count($ops); $j++) {
        if ($ops[$j] === '+') {
            $vals[$j] = $vals[$j] + $vals[$j + 1];
            array_splice($vals, $j + 1, 1);
            array_splice($ops, $j, 1);
            $j--;
        }
    }
    // Then, resolve multiplications
    $res = $vals[0];
    for ($j = 0; $j < count($ops); $j++) {
        $res *= $vals[$j + 1];
    }
    return [$res, $i];
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
    [53660285675207, 141993988282687], // Expected
    [$result01, $result02], // Result
);
