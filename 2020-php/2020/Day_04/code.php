<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $required = ['byr','iyr','eyr','hgt','hcl','ecl','pid'];
    $valid = 0;
    $passport = '';
    foreach ($input as $line) {
        if (trim($line) === '') {
            $fields = array();
            foreach (preg_split('/\s+/', trim($passport)) as $pair) {
                list($key, $value) = explode(':', $pair);
                $fields[$key] = $value;
            }
            $missing = array_diff($required, array_keys($fields));
            if (count($missing) === 0) {
                $valid++;
            }
            $passport = '';
        } else {
            $passport .= ' ' . $line;
        }
    }
    // Check last passport
    if (trim($passport) !== '') {
        $fields = array();
        foreach (preg_split('/\s+/', trim($passport)) as $pair) {
            list($key, $value) = explode(':', $pair);
            $fields[$key] = $value;
        }
        $missing = array_diff($required, array_keys($fields));
        if (count($missing) === 0) {
            $valid++;
        }
    }
    return $valid;
}

function part02(array $input)
{
    $required = ['byr','iyr','eyr','hgt','hcl','ecl','pid'];
    $valid = 0;
    $passport = '';
    foreach ($input as $line) {
        if (trim($line) === '') {
            if (isValidPassport($passport, $required)) {
                $valid++;
            }
            $passport = '';
        } else {
            $passport .= ' ' . $line;
        }
    }
    if (trim($passport) !== '') {
        if (isValidPassport($passport, $required)) {
            $valid++;
        }
    }
    return $valid;
}

function isValidPassport($passport, $required)
{
    $fields = array();
    foreach (preg_split('/\s+/', trim($passport)) as $pair) {
        list($key, $value) = explode(':', $pair);
        $fields[$key] = $value;
    }
    $missing = array_diff($required, array_keys($fields));
    if (count($missing) !== 0) {
        return false;
    }
    // byr
    if (!preg_match('/^\d{4}$/', $fields['byr']) || $fields['byr'] < 1920 || $fields['byr'] > 2002) {
        return false;
    }
    // iyr
    if (!preg_match('/^\d{4}$/', $fields['iyr']) || $fields['iyr'] < 2010 || $fields['iyr'] > 2020) {
        return false;
    }
    // eyr
    if (!preg_match('/^\d{4}$/', $fields['eyr']) || $fields['eyr'] < 2020 || $fields['eyr'] > 2030) {
        return false;
    }
    // hgt
    if (!preg_match('/^(\d+)(cm|in)$/', $fields['hgt'], $m)) {
        return false;
    }
    $h = (int)$m[1];
    if ($m[2] === 'cm') {
        if ($h < 150 || $h > 193) {
            return false;
        }
    } else {
        if ($h < 59 || $h > 76) {
            return false;
        }
    }
    // hcl
    if (!preg_match('/^#[0-9a-f]{6}$/', $fields['hcl'])) {
        return false;
    }
    // ecl
    if (!in_array($fields['ecl'], ['amb','blu','brn','gry','grn','hzl','oth'])) {
        return false;
    }
    // pid
    if (!preg_match('/^\d{9}$/', $fields['pid'])) {
        return false;
    }
    return true;
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
    [200, 116], // Expected
    [$result01, $result02], // Result
);
