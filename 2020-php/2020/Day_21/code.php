<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

// Task code
function part01(array $input)
{
    $foods = [];
    $all_ingredients = [];
    $allergen_map = [];
    foreach ($input as $line) {
        if (preg_match('/^([a-z ]+) \(contains ([a-z, ]+)\)$/', $line, $m)) {
            $ingredients = preg_split('/\s+/', trim($m[1]));
            $allergens = array_map('trim', explode(',', $m[2]));
            $foods[] = [
                'ingredients' => $ingredients,
                'allergens' => $allergens
            ];
            $all_ingredients = array_merge($all_ingredients, $ingredients);
            foreach ($allergens as $a) {
                if (!isset($allergen_map[$a])) {
                    $allergen_map[$a] = [];
                }
                $allergen_map[$a][] = $ingredients;
            }
        }
    }
    // For each allergen, get possible ingredients (intersection)
    $possible = [];
    foreach ($allergen_map as $a => $lists) {
        $possible[$a] = array_values(array_intersect(...$lists));
    }
    // All ingredients that could contain any allergen
    $maybe = array_unique(array_merge(...array_values($possible)));
    // Safe ingredients are those not in any possible allergen set
    $safe = array_diff($all_ingredients, $maybe);
    // Count appearances
    $count = 0;
    foreach ($all_ingredients as $ing) {
        if (in_array($ing, $safe, true)) {
            $count++;
        }
    }
    return $count;
}

function part02(array $input)
{
    $foods = [];
    $allergen_map = [];
    foreach ($input as $line) {
        if (preg_match('/^([a-z ]+) \(contains ([a-z, ]+)\)$/', $line, $m)) {
            $ingredients = preg_split('/\s+/', trim($m[1]));
            $allergens = array_map('trim', explode(',', $m[2]));
            $foods[] = [
                'ingredients' => $ingredients,
                'allergens' => $allergens
            ];
            foreach ($allergens as $a) {
                if (!isset($allergen_map[$a])) {
                    $allergen_map[$a] = [];
                }
                $allergen_map[$a][] = $ingredients;
            }
        }
    }
    // For each allergen, get possible ingredients (intersection)
    $possible = [];
    foreach ($allergen_map as $a => $lists) {
        $possible[$a] = array_values(array_intersect(...$lists));
    }
    // Resolve mapping
    $resolved = [];
    while (count($resolved) < count($possible)) {
        foreach ($possible as $a => $ings) {
            $ings = array_diff($ings, $resolved);
            if (count($ings) === 1) {
                $ingredient = reset($ings);
                $resolved[$a] = $ingredient;
                // Remove from all other lists
                foreach ($possible as $b => $ings2) {
                    if ($a !== $b) {
                        $possible[$b] = array_diff($ings2, [$ingredient]);
                    }
                }
            }
        }
    }
    ksort($resolved);
    return implode(',', $resolved);
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
    [2734, "kbmlt,mrccxm,lpzgzmk,ppj,stj,jvgnc,gxnr,plrlg"], // Expected
    [$result01, $result02], // Result
);
