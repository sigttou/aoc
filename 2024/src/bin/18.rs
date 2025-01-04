use std::collections::{HashMap, HashSet, VecDeque};

advent_of_code::solution!(18);
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

fn parse(input: &str, byte_cnt: usize) -> Grid {
    // grid size from puzzle or example
    let mut grid: Grid = (71, 71, HashMap::<Point, char>::new());
    if input.lines().count() < 100 {
        grid.0 = 7;
        grid.1 = 7;
    }

    for x in 0..grid.0 {
        for y in 0..grid.1 {
            grid.2.insert((x, y), '.');
        }
    }

    for (x_str, y_str) in input
        .lines()
        .take(byte_cnt)
        .map(|line| line.split_once(",").unwrap())
    {
        let x: i32 = x_str.parse().unwrap();
        let y: i32 = y_str.parse().unwrap();
        grid.2.insert((x, y), '#');
    }

    grid
}

fn solve_maze(grid: Grid) -> Option<u32> {
    let mut queue = VecDeque::<(i32, i32, u32)>::new();
    queue.push_back((0, 0, 0));
    let mut seen = HashSet::<(i32, i32)>::new();

    while let Some((x, y, steps)) = queue.pop_front() {
        if (x, y) == (grid.0 - 1, grid.1 - 1) {
            return Some(steps);
        }

        for (dx, dy) in DIRECTIONS {
            let (nx, ny) = (x + dx, y + dy);
            if grid.2.contains_key(&(nx, ny))
                && *grid.2.get(&(nx, ny)).unwrap() == '.'
                && !seen.contains(&(nx, ny))
            {
                seen.insert((nx, ny));
                queue.push_back((nx, ny, steps + 1));
            }
        }
    }

    None
}

pub fn part_one(input: &str) -> Option<u32> {
    let mut byte_cnt = 1024;
    if input.lines().count() < 100 {
        byte_cnt = 12;
    }
    let grid = parse(input, byte_cnt);
    solve_maze(grid)
}

pub fn part_two(input: &str) -> Option<String> {
    let mut byte_cnt = 1024;
    let mut mx_cnt = input.lines().count();
    if mx_cnt < 100 {
        byte_cnt = 12;
    }
    while byte_cnt < mx_cnt {
        let try_cnt = (byte_cnt + mx_cnt) / 2;
        let grid = parse(input, try_cnt);
        if solve_maze(grid).is_some() {
            byte_cnt = try_cnt + 1;
        } else {
            mx_cnt = try_cnt;
        }
    }
    Some(
        input
            .lines()
            .collect::<Vec<_>>()
            .get(byte_cnt - 1)
            .unwrap()
            .to_string(),
    )
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(22));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(String::from("6,1")));
    }
}
