<?php

require_once __DIR__ . '/../../lib/php/utils.php';

$input = readInput(__DIR__);

function parseTiles(array $input)
{
    $tiles = [];
    $id = null;
    $tile = [];
    foreach ($input as $line) {
        if (preg_match('/^Tile (\d+):$/', $line, $m)) {
            if ($id !== null) {
                $tiles[$id] = $tile;
            }
            $id = (int)$m[1];
            $tile = [];
        } elseif (trim($line) !== '') {
            $tile[] = $line;
        }
    }
    if ($id !== null) {
        $tiles[$id] = $tile;
    }
    return $tiles;
}

function getEdges($tile)
{
    $n = count($tile);
    $top = $tile[0];
    $bottom = $tile[$n - 1];
    $left = '';
    $right = '';
    foreach ($tile as $row) {
        $left .= $row[0];
        $right .= $row[$n - 1];
    }
    return [$top, $right, $bottom, $left];
}

function allOrientations($tile)
{
    $out = [];
    $t = $tile;
    for ($i = 0; $i < 4; $i++) {
        $out[] = $t;
        $out[] = array_map('strrev', $t);
        $t = rotate($t);
    }
    // Remove duplicates
    $unique = [];
    foreach ($out as $o) {
        $h = md5(implode("\n", $o));
        $unique[$h] = $o;
    }
    return array_values($unique);
}

function rotate($tile)
{
    $n = count($tile);
    $rot = [];
    for ($i = 0; $i < $n; $i++) {
        $row = '';
        for ($j = $n - 1; $j >= 0; $j--) {
            $row .= $tile[$j][$i];
        }
        $rot[] = $row;
    }
    return $rot;
}

function findCorners($tiles)
{
    $edgeMap = [];
    foreach ($tiles as $id => $tile) {
        foreach (getEdges($tile) as $e) {
            $edgeMap[$e][] = $id;
            $edgeMap[strrev($e)][] = $id;
        }
    }
    $corners = [];
    foreach ($tiles as $id => $tile) {
        $unique = 0;
        foreach (getEdges($tile) as $e) {
            $ids = array_unique(array_merge($edgeMap[$e] ?? [], $edgeMap[strrev($e)] ?? []));
            if (count($ids) === 1) {
                $unique++;
            }
        }
        if ($unique === 2) {
            $corners[] = $id;
        }
    }
    return $corners;
}

function assemble($tiles)
{
    $N = (int)sqrt(count($tiles));
    $ids = array_keys($tiles);
    $corners = findCorners($tiles);
    foreach ($corners as $cornerId) {
        foreach (allOrientations($tiles[$cornerId]) as $cornerTile) {
            $grid = array_fill(0, $N, array_fill(0, $N, null));
            $used = [$cornerId => true];
            $grid[0][0] = [$cornerId, $cornerTile];
            if ($res = fill($grid, $tiles, $used, $N, 0, 1)) {
                return $res;
            }
        }
    }
    return null;
}

function fill($grid, $tiles, $used, $N, $y, $x)
{
    if ($y === $N) {
        return $grid;
    }
    $nx = ($x + 1) % $N;
    $ny = $y + ($nx === 0 ? 1 : 0);
    foreach ($tiles as $id => $tile) {
        if (isset($used[$id])) {
            continue;
        }
        foreach (allOrientations($tile) as $variant) {
            $ok = true;
            if ($x > 0) {
                $left = $grid[$y][$x - 1][1];
                if (getEdges($variant)[3] !== getEdges($left)[1]) {
                    $ok = false;
                }
            }
            if ($y > 0) {
                $top = $grid[$y - 1][$x][1];
                if (getEdges($variant)[0] !== getEdges($top)[2]) {
                    $ok = false;
                }
            }
            if ($ok) {
                $grid2 = $grid;
                $grid2[$y][$x] = [$id, $variant];
                $used2 = $used;
                $used2[$id] = true;
                $res = fill($grid2, $tiles, $used2, $N, $ny, $nx);
                if ($res) {
                    return $res;
                }
            }
        }
    }
    return false;
}

function removeBorders($tile)
{
    $out = [];
    for ($i = 1; $i < count($tile) - 1; $i++) {
        $out[] = substr($tile[$i], 1, -1);
    }
    return $out;
}

function buildImage($grid)
{
    $N = count($grid);
    $tileSize = count($grid[0][0][1]);
    $image = [];
    for ($gy = 0; $gy < $N; $gy++) {
        for ($ty = 1; $ty < $tileSize - 1; $ty++) {
            $row = '';
            for ($gx = 0; $gx < $N; $gx++) {
                $row .= substr($grid[$gy][$gx][1][$ty], 1, $tileSize - 2);
            }
            $image[] = $row;
        }
    }
    return $image;
}

function findSeaMonsters($image)
{
    $monster = [
        '                  # ',
        '#    ##    ##    ###',
        ' #  #  #  #  #  #   '
    ];
    $mh = count($monster);
    $mw = strlen($monster[0]);
    $mcoords = [];
    for ($y = 0; $y < $mh; $y++) {
        for ($x = 0; $x < $mw; $x++) {
            if ($monster[$y][$x] === '#') {
                $mcoords[] = [$y, $x];
            }
        }
    }
    foreach (allOrientations($image) as $img) {
        $found = [];
        for ($y = 0; $y <= count($img) - $mh; $y++) {
            for ($x = 0; $x <= strlen($img[0]) - $mw; $x++) {
                $ok = true;
                foreach ($mcoords as [$dy, $dx]) {
                    if ($img[$y + $dy][$x + $dx] !== '#') {
                        $ok = false;
                        break;
                    }
                }
                if ($ok) {
                    foreach ($mcoords as [$dy, $dx]) {
                        $found[($y + $dy).',' . ($x + $dx)] = true;
                    }
                }
            }
        }
        if ($found) {
            $rough = 0;
            foreach ($img as $y => $row) {
                for ($x = 0; $x < strlen($row); $x++) {
                    if ($row[$x] === '#' && !isset($found[$y.','.$x])) {
                        $rough++;
                    }
                }
            }
            return $rough;
        }
    }
    return -1;
}

function part01(array $input)
{
    $tiles = parseTiles($input);
    $corners = findCorners($tiles);
    return array_product($corners);
}

function part02(array $input)
{
    $tiles = parseTiles($input);
    $grid = assemble($tiles);
    if (!$grid) {
        return null;
    }
    $image = buildImage($grid);
    return findSeaMonsters($image);
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
    [7901522557967, 2476], // Expected
    [$result01, $result02], // Result
);
