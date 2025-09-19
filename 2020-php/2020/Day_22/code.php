<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    // Parse decks
    $decks = [[], []];
    $current = -1;
    foreach ($input as $line) {
        if (strpos($line, 'Player') === 0) {
            $current++;
        } elseif (trim($line) !== '') {
            $decks[$current][] = (int)$line;
        }
    }
    $p1 = $decks[0];
    $p2 = $decks[1];
    while (count($p1) > 0 && count($p2) > 0) {
        $c1 = array_shift($p1);
        $c2 = array_shift($p2);
        if ($c1 > $c2) {
            $p1[] = $c1;
            $p1[] = $c2;
        } else {
            $p2[] = $c2;
            $p2[] = $c1;
        }
    }
    $winner = count($p1) > 0 ? $p1 : $p2;
    $score = 0;
    $n = count($winner);
    foreach ($winner as $i => $card) {
        $score += $card * ($n - $i);
    }
    return $score;
}

function part02(array $input)
{
    // Parse decks
    $decks = [[], []];
    $current = -1;
    foreach ($input as $line) {
        if (strpos($line, 'Player') === 0) {
            $current++;
        } elseif (trim($line) !== '') {
            $decks[$current][] = (int)$line;
        }
    }
    // Recursive combat
    [$winner, $deck] = play_recursive($decks[0], $decks[1]);
    $score = 0;
    $n = count($deck);
    foreach ($deck as $i => $card) {
        $score += $card * ($n - $i);
    }
    return $score;
}

function play_recursive($p1, $p2)
{
    $seen = [];
    while (count($p1) > 0 && count($p2) > 0) {
        $state = implode(',', $p1) . '|' . implode(',', $p2);
        if (isset($seen[$state])) {
            return [1, $p1];
        }
        $seen[$state] = true;
        $c1 = array_shift($p1);
        $c2 = array_shift($p2);
        if (count($p1) >= $c1 && count($p2) >= $c2) {
            [$w, ] = play_recursive(array_slice($p1, 0, $c1), array_slice($p2, 0, $c2));
        } else {
            $w = $c1 > $c2 ? 1 : 2;
        }
        if ($w === 1) {
            $p1[] = $c1;
            $p1[] = $c2;
        } else {
            $p2[] = $c2;
            $p2[] = $c1;
        }
    }
    return count($p1) > 0 ? [1, $p1] : [2, $p2];
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
    [35397, 31120], // Expected
    [$result01, $result02], // Result
);
