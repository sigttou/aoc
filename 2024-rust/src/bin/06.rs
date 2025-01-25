use std::collections::HashSet;

advent_of_code::solution!(6);

static DIRECTIONS: &[(i32, i32)] = &[(0, -1), (1, 0), (0, 1), (-1, 0)];

fn parse(input: &str) -> (Vec<Vec<char>>, (usize, usize)) {
    let grid: Vec<Vec<char>> = input
        .lines()
        .map(|x| x.chars().collect::<Vec<_>>())
        .collect();

    for (y, xs) in grid.iter().enumerate() {
        for (x, e) in xs.iter().enumerate() {
            if *e == '^' {
                return (grid, (x, y));
            }
        }
    }
    panic!();
}

pub fn chk_loop(grid: &[Vec<char>], start: (usize, usize), dir: usize) -> bool {
    let (mut x, mut y) = (start.0 as i32, start.1 as i32);
    let mut direction = dir;
    let mut visited = HashSet::<(usize, usize, usize)>::new();
    loop {
        if visited.contains(&(x as usize, y as usize, direction)) {
            break;
        }
        visited.insert((x as usize, y as usize, direction));
        let nx = x + DIRECTIONS[direction].0;
        let ny = y + DIRECTIONS[direction].1;
        if nx < 0 || nx >= grid[0].len() as i32 || ny < 0 || ny >= grid.len() as i32 {
            return false;
        }
        if grid[ny as usize][nx as usize] == '#' {
            direction = (direction + 1) % 4;
        } else {
            x = nx;
            y = ny;
        }
    }
    true
}

pub fn get_visited(
    grid: &[Vec<char>],
    start: (usize, usize),
    dir: usize,
) -> HashSet<(usize, usize)> {
    let (mut x, mut y) = (start.0 as i32, start.1 as i32);
    let mut direction = dir;
    let mut visited = HashSet::<(usize, usize)>::new();
    loop {
        visited.insert((x as usize, y as usize));
        let nx = x + DIRECTIONS[direction].0;
        let ny = y + DIRECTIONS[direction].1;
        if nx < 0 || nx >= grid[0].len() as i32 || ny < 0 || ny >= grid.len() as i32 {
            break;
        }
        if grid[ny as usize][nx as usize] == '#' {
            direction = (direction + 1) % 4;
        } else {
            x = nx;
            y = ny;
        }
    }
    visited
}

pub fn part_one(input: &str) -> Option<u32> {
    let (grid, start) = parse(input);
    Some(get_visited(&grid, start, 0).len() as u32)
}

pub fn part_two(input: &str) -> Option<u32> {
    let (mut grid, start) = parse(input);
    let mut positions = get_visited(&grid, start, 0);
    positions.remove(&start);
    let mut cnt = 0;
    for (x, y) in positions {
        grid[y][x] = '#';
        if chk_loop(&grid, start, 0) {
            cnt += 1;
        }
        grid[y][x] = '.';
    }
    Some(cnt)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(41));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(6));
    }
}
