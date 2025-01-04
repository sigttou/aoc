use std::collections::{HashMap, HashSet};

advent_of_code::solution!(20);
type Point = (i32, i32);
type Grid = (i32, i32, HashMap<Point, char>);

static DIRECTIONS: &[(i32, i32)] = &[(0, -1), (1, 0), (0, 1), (-1, 0)];

#[allow(dead_code)]
fn print_grid(grid: Grid) {
    for y in 0..grid.1 {
        for x in 0..grid.0 {
            print!("{}", grid.2.get(&(x, y)).unwrap());
        }
        println!();
    }
}

fn parse(input: &str) -> (Point, Point, Grid) {
    let mut start = (0, 0);
    let mut end = (0, 0);
    let mut grid: Grid = (0, 0, HashMap::<Point, char>::new());

    for (y, line) in input.lines().enumerate() {
        for (x, mut c) in line.chars().enumerate() {
            if c == 'S' {
                start.0 = x as i32;
                start.1 = y as i32;
                c = '.';
            }
            if c == 'E' {
                end.0 = x as i32;
                end.1 = y as i32;
                c = '.';
            }
            grid.2.insert((x as i32, y as i32), c);
            grid.0 = x as i32;
            grid.1 = y as i32;
        }
    }

    grid.0 += 1;
    grid.1 += 1;
    (start, end, grid)
}

fn cheat_targets(path: &HashMap<Point, u32>, pos: Point) -> HashSet<Point> {
    let mut ret = HashSet::<Point>::new();

    for dx in -20..21_i32 {
        for dy in (-(20 - dx.abs()))..(20 - dx.abs() + 1) {
            let chk = (pos.0 + dx, pos.1 + dy);
            if path.contains_key(&chk) {
                ret.insert(chk);
            }
        }
    }

    ret
}

fn manhattan_distance(a: Point, b: Point) -> i32 {
    (a.0 - b.0).abs() + (a.1 - b.1).abs()
}

fn solve_maze(start: Point, end: Point, grid: Grid, p: u32) -> u32 {
    let mut path = HashMap::<Point, u32>::new();
    path.insert(start, 0);
    let mut pos = start;
    let mut step_cnt = 0;

    while pos != end {
        step_cnt += 1;
        for dir in DIRECTIONS {
            let nxt = (pos.0 + dir.0, pos.1 + dir.1);
            if !path.contains_key(&nxt) && *grid.2.get(&nxt).unwrap() == '.' {
                pos = nxt;
                path.insert(nxt, step_cnt);
                break;
            }
        }
    }

    let mut ret = 0;
    if p == 2 {
        for chk in path.keys() {
            for cheat in cheat_targets(&path, *chk) {
                if *path.get(&cheat).unwrap() as i32
                    - *path.get(chk).unwrap() as i32
                    - manhattan_distance(*chk, cheat)
                    >= 100
                {
                    ret += 1;
                }
            }
        }
    } else {
        for chk in path.keys() {
            for dir in DIRECTIONS {
                if !path.contains_key(&(chk.0 + dir.0, chk.1 + dir.1))
                    && path.contains_key(&(chk.0 + 2 * dir.0, chk.1 + 2 * dir.1))
                    && *path.get(&(chk.0 + 2 * dir.0, chk.1 + 2 * dir.1)).unwrap() as i32
                        - *path.get(chk).unwrap() as i32
                        >= 102
                {
                    ret += 1;
                }
            }
        }
    }

    ret
}

pub fn part_one(input: &str) -> Option<u32> {
    let (start, end, grid) = parse(input);
    Some(solve_maze(start, end, grid, 1))
}

pub fn part_two(input: &str) -> Option<u32> {
    let (start, end, grid) = parse(input);
    Some(solve_maze(start, end, grid, 2))
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(0));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(0));
    }
}
