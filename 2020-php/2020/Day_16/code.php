<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

function parseInput(array $input)
{
    $fields = [];
    $myTicket = [];
    $nearbyTickets = [];

    $section = 'fields';
    foreach ($input as $line) {
        if (trim($line) === '') {
            continue;
        }

        if ($line === 'your ticket:') {
            $section = 'my';
            continue;
        }
        if ($line === 'nearby tickets:') {
            $section = 'nearby';
            continue;
        }

        if ($section === 'fields') {
            if (preg_match('/^(.+): (\d+)-(\d+) or (\d+)-(\d+)$/', $line, $matches)) {
                $fields[$matches[1]] = [
                    [(int)$matches[2], (int)$matches[3]],
                    [(int)$matches[4], (int)$matches[5]]
                ];
            }
        } elseif ($section === 'my') {
            $myTicket = array_map('intval', explode(',', $line));
        } elseif ($section === 'nearby') {
            $nearbyTickets[] = array_map('intval', explode(',', $line));
        }
    }

    return [$fields, $myTicket, $nearbyTickets];
}

function isValidForAnyField($value, $fields)
{
    foreach ($fields as $ranges) {
        foreach ($ranges as $range) {
            if ($value >= $range[0] && $value <= $range[1]) {
                return true;
            }
        }
    }
    return false;
}

function isValidForField($value, $ranges)
{
    foreach ($ranges as $range) {
        if ($value >= $range[0] && $value <= $range[1]) {
            return true;
        }
    }
    return false;
}

// Task code
function part01(array $input)
{
    [$fields, $myTicket, $nearbyTickets] = parseInput($input);

    $errorRate = 0;
    foreach ($nearbyTickets as $ticket) {
        foreach ($ticket as $value) {
            if (!isValidForAnyField($value, $fields)) {
                $errorRate += $value;
            }
        }
    }

    return $errorRate;
}

function part02(array $input)
{
    [$fields, $myTicket, $nearbyTickets] = parseInput($input);

    // Filter out invalid tickets
    $validTickets = [];
    foreach ($nearbyTickets as $ticket) {
        $valid = true;
        foreach ($ticket as $value) {
            if (!isValidForAnyField($value, $fields)) {
                $valid = false;
                break;
            }
        }
        if ($valid) {
            $validTickets[] = $ticket;
        }
    }

    // Determine possible fields for each position
    $numPositions = count($myTicket);
    $possibleFields = [];

    for ($pos = 0; $pos < $numPositions; $pos++) {
        $possibleFields[$pos] = [];

        foreach ($fields as $fieldName => $ranges) {
            $validForAllTickets = true;

            // Check if this field is valid for this position in all valid tickets
            foreach ($validTickets as $ticket) {
                if (!isValidForField($ticket[$pos], $ranges)) {
                    $validForAllTickets = false;
                    break;
                }
            }

            if ($validForAllTickets) {
                $possibleFields[$pos][] = $fieldName;
            }
        }
    }

    // Resolve field assignments using constraint propagation
    $fieldAssignments = [];
    $assigned = [];

    while (count($fieldAssignments) < $numPositions) {
        // Find positions with only one possible field
        for ($pos = 0; $pos < $numPositions; $pos++) {
            if (isset($fieldAssignments[$pos])) {
                continue;
            }

            $available = array_diff($possibleFields[$pos], $assigned);
            if (count($available) === 1) {
                $field = reset($available);
                $fieldAssignments[$pos] = $field;
                $assigned[] = $field;
            }
        }
    }

    // Calculate product of departure fields
    $product = 1;
    for ($pos = 0; $pos < $numPositions; $pos++) {
        if (strpos($fieldAssignments[$pos], 'departure') === 0) {
            $product *= $myTicket[$pos];
        }
    }

    return $product;
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
    [21071, 3429967441937], // Expected
    [$result01, $result02], // Result
);
