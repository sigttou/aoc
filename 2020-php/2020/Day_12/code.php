<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $x = 0; // East/West position (positive = east)
    $y = 0; // North/South position (positive = north)
    $direction = 90; // Facing direction in degrees (0 = north, 90 = east, 180 = south, 270 = west)

    foreach ($input as $line) {
        $action = $line[0];
        $value = (int)substr($line, 1);

        switch ($action) {
            case 'N':
                $y += $value;
                break;
            case 'S':
                $y -= $value;
                break;
            case 'E':
                $x += $value;
                break;
            case 'W':
                $x -= $value;
                break;
            case 'L':
                $direction = ($direction - $value + 360) % 360;
                break;
            case 'R':
                $direction = ($direction + $value) % 360;
                break;
            case 'F':
                // Move forward in the current direction
                switch ($direction) {
                    case 0: // North
                        $y += $value;
                        break;
                    case 90: // East
                        $x += $value;
                        break;
                    case 180: // South
                        $y -= $value;
                        break;
                    case 270: // West
                        $x -= $value;
                        break;
                }
                break;
        }
    }

    return abs($x) + abs($y);
}

function part02(array $input)
{
    $shipX = 0; // Ship East/West position
    $shipY = 0; // Ship North/South position
    $waypointX = 10; // Waypoint relative to ship (starts 10 east)
    $waypointY = 1; // Waypoint relative to ship (starts 1 north)

    foreach ($input as $line) {
        $action = $line[0];
        $value = (int)substr($line, 1);

        switch ($action) {
            case 'N':
                $waypointY += $value;
                break;
            case 'S':
                $waypointY -= $value;
                break;
            case 'E':
                $waypointX += $value;
                break;
            case 'W':
                $waypointX -= $value;
                break;
            case 'L':
                // Rotate waypoint counter-clockwise around ship
                $rotations = $value / 90;
                for ($i = 0; $i < $rotations; $i++) {
                    $temp = $waypointX;
                    $waypointX = -$waypointY;
                    $waypointY = $temp;
                }
                break;
            case 'R':
                // Rotate waypoint clockwise around ship
                $rotations = $value / 90;
                for ($i = 0; $i < $rotations; $i++) {
                    $temp = $waypointX;
                    $waypointX = $waypointY;
                    $waypointY = -$temp;
                }
                break;
            case 'F':
                // Move ship to waypoint N times
                $shipX += $waypointX * $value;
                $shipY += $waypointY * $value;
                break;
        }
    }

    return abs($shipX) + abs($shipY);
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
    [1186, 47806], // Expected
    [$result01, $result02], // Result
);
