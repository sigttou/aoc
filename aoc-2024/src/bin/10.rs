use std::collections::{HashMap, HashSet};

advent_of_code::solution!(10);

static DIRECTIONS: &[(i32, i32)] = &[(0, -1), (1, 0), (0, 1), (-1, 0)];

fn parse(input: &str) -> ((usize, usize), HashMap<(i32, i32), i32>) {
    let mut grid = HashMap::new();
    let mut xmax = 0;
    let mut ymax = 0;
    for (y, line) in input.lines().enumerate() {
        for (x, c) in line.chars().enumerate() {
            grid.insert((x as i32, y as i32), c.to_digit(10).unwrap() as i32);
            xmax = x;
            ymax = y;
        }
    }
    ((xmax + 1, ymax + 1), grid)
}

fn get_trails(
    size: (usize, usize),
    grid: HashMap<(i32, i32), i32>,
) -> Vec<((i32, i32), (i32, i32))> {
    let mut starts = Vec::<(i32, i32)>::new();
    for x in 0..size.0 {
        for y in 0..size.1 {
            if *grid.get(&(x as i32, y as i32)).unwrap() == 0_i32 {
                starts.push((x as i32, y as i32));
            }
        }
    }

    let mut ret = Vec::new();
    for start in starts {
        let mut todo = Vec::<((i32, i32), usize)>::new();
        todo.push((start, 1));
        while let Some((pos, cnt)) = todo.pop() {
            if cnt == 10 {
                ret.push((start, pos));
            }
            for dir in DIRECTIONS {
                let nxt = (pos.0 + dir.0, pos.1 + dir.1);
                let val = grid.get(&nxt);
                if val.is_some() && *val.unwrap() == cnt as i32 {
                    todo.push((nxt, cnt + 1));
                }
            }
        }
    }
    ret
}

pub fn part_one(input: &str) -> Option<u32> {
    let (size, grid) = parse(input);
    Some(
        HashSet::<((i32, i32), (i32, i32))>::from_iter(get_trails(size, grid).iter().cloned()).len()
            as u32,
    )
}

pub fn part_two(input: &str) -> Option<u32> {
    let (size, grid) = parse(input);
    Some(get_trails(size, grid).len() as u32)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(36));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(81));
    }
}
