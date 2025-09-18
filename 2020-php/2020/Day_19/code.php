<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    // Separate rules and messages
    $splitIndex = array_search('', $input);
    $ruleLines = array_slice($input, 0, $splitIndex);
    $messages = array_slice($input, $splitIndex + 1);

    // Parse rules
    $rules = [];
    foreach ($ruleLines as $line) {
        if (preg_match('/^(\d+): (.+)$/', $line, $m)) {
            $num = (int)$m[1];
            $rule = $m[2];
            if (preg_match('/^"([ab])"$/', $rule, $mm)) {
                $rules[$num] = $mm[1];
            } else {
                $parts = explode('|', $rule);
                $subrules = [];
                foreach ($parts as $part) {
                    $subrules[] = array_map('intval', preg_split('/\s+/', trim($part)));
                }
                $rules[$num] = $subrules;
            }
        }
    }

    // Recursive matcher: returns array of possible remainders after matching
    function match_rule($rules, $ruleNum, $message)
    {
        $rule = $rules[$ruleNum];
        if (is_string($rule)) {
            if (isset($message[0]) && $message[0] === $rule) {
                return [substr($message, 1)];
            } else {
                return [];
            }
        }
        $results = [];
        foreach ($rule as $option) {
            $remainders = [$message];
            foreach ($option as $subrule) {
                $newRemainders = [];
                foreach ($remainders as $rem) {
                    $matches = match_rule($rules, $subrule, $rem);
                    foreach ($matches as $m) {
                        $newRemainders[] = $m;
                    }
                }
                $remainders = $newRemainders;
                if (empty($remainders)) {
                    break;
                }
            }
            foreach ($remainders as $r) {
                $results[] = $r;
            }
        }
        return $results;
    }

    // Count messages that match rule 0 exactly
    $count = 0;
    foreach ($messages as $msg) {
        $matched = false;
        foreach (match_rule($rules, 0, $msg) as $rem) {
            if ($rem === '') {
                $matched = true;
                break;
            }
        }
        if ($matched) {
            $count++;
        }
    }
    return $count;
}

function part02(array $input)
{
    // Separate rules and messages
    $splitIndex = array_search('', $input);
    $ruleLines = array_slice($input, 0, $splitIndex);
    $messages = array_slice($input, $splitIndex + 1);

    // Parse rules
    $rules = [];
    foreach ($ruleLines as $line) {
        if (preg_match('/^(\d+): (.+)$/', $line, $m)) {
            $num = (int)$m[1];
            $rule = $m[2];
            if (preg_match('/^"([ab])"$/', $rule, $mm)) {
                $rules[$num] = $mm[1];
            } else {
                $parts = explode('|', $rule);
                $subrules = [];
                foreach ($parts as $part) {
                    $subrules[] = array_map('intval', preg_split('/\s+/', trim($part)));
                }
                $rules[$num] = $subrules;
            }
        }
    }

    // Patch rules 8 and 11 for recursion
    // 8: 42 | 42 8  =>  [ [42], [42, 8] ]
    // 11: 42 31 | 42 11 31  =>  [ [42, 31], [42, 11, 31] ]
    $rules[8] = [ [42], [42, 8] ];
    $rules[11] = [ [42, 31], [42, 11, 31] ];

    // Recursive matcher: returns array of possible remainders after matching
    function match_rule2($rules, $ruleNum, $message)
    {
        $rule = $rules[$ruleNum];
        if (is_string($rule)) {
            if (isset($message[0]) && $message[0] === $rule) {
                return [substr($message, 1)];
            } else {
                return [];
            }
        }
        $results = [];
        foreach ($rule as $option) {
            $remainders = [$message];
            foreach ($option as $subrule) {
                $newRemainders = [];
                foreach ($remainders as $rem) {
                    $matches = match_rule2($rules, $subrule, $rem);
                    foreach ($matches as $m) {
                        $newRemainders[] = $m;
                    }
                }
                $remainders = $newRemainders;
                if (empty($remainders)) {
                    break;
                }
            }
            foreach ($remainders as $r) {
                $results[] = $r;
            }
        }
        return $results;
    }

    // Count messages that match rule 0 exactly
    $count = 0;
    foreach ($messages as $msg) {
        $matched = false;
        foreach (match_rule2($rules, 0, $msg) as $rem) {
            if ($rem === '') {
                $matched = true;
                break;
            }
        }
        if ($matched) {
            $count++;
        }
    }
    return $count;
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
    [162, 267], // Expected
    [$result01, $result02], // Result
);
