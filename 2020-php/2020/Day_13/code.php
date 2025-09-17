<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $earliest = (int)trim($input[0]);
    $buses = array_filter(explode(',', trim($input[1])), fn ($v) => $v !== 'x');
    $buses = array_map('intval', $buses);
    $minWait = PHP_INT_MAX;
    $bestBus = null;
    foreach ($buses as $bus) {
        $wait = $bus - ($earliest % $bus);
        if ($wait < $minWait) {
            $minWait = $wait;
            $bestBus = $bus;
        }
    }
    return $bestBus * $minWait;
}

function part02(array $input)
{
    $buses = explode(',', trim($input[1]));
    $constraints = [];
    foreach ($buses as $i => $bus) {
        if ($bus === 'x') {
            continue;
        }
        $bus = (int)$bus;
        $constraints[] = [$bus, $i];
    }
    $t = 0;
    $step = 1;
    foreach ($constraints as [$bus, $offset]) {
        while (($t + $offset) % $bus !== 0) {
            $t += $step;
        }
        $step *= $bus;
    }
    return $t;
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
    [205, 803025030761664], // Expected
    [$result01, $result02], // Result
);
