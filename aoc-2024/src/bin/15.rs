use std::collections::{HashMap, HashSet};

use itertools::Itertools;

advent_of_code::solution!(15);

type Point = (i32, i32);
type Move = (i32, i32);
type Grid = (i32, i32, HashMap<Point, char>);

#[allow(dead_code)]
fn print_grid(grid: Grid) {
    for y in 0..grid.1 {
        for x in 0..grid.0 {
            print!("{}", grid.2.get(&(x, y)).unwrap());
        }
        println!();
    }
}

fn parse(input: &str, part: u32) -> (Point, Grid, Vec<Move>) {
    let mut grid: Grid = (0, 0, HashMap::<Point, char>::new());
    let mut start: Point = (0, 0);

    let (grid_str_tmp, moves_str) = input.split_once("\n\n").unwrap();
    let grid_str = match part {
        2 => grid_str_tmp
            .chars()
            .filter_map(|c| match c {
                '.' | '#' => Some([c, c].iter().collect::<String>()),
                'O' => Some("[]".to_string()),
                '@' => Some("@.".to_string()),
                '\n' => Some("\n".to_string()),
                _ => None,
            })
            .join(""),
        _ => grid_str_tmp.to_string(),
    };

    for (y, line) in grid_str.lines().enumerate() {
        for (x, mut e) in line.chars().enumerate() {
            if e == '@' {
                start.0 = x as i32;
                start.1 = y as i32;
                e = '.';
            }
            grid.2.insert((x as i32, y as i32), e);
            grid.0 = x as i32;
            grid.1 = y as i32;
        }
    }

    let moves = moves_str
        .chars()
        .filter_map(|e| match e {
            '<' => Some((-1, 0)),
            '>' => Some((1, 0)),
            '^' => Some((0, -1)),
            'v' => Some((0, 1)),
            _ => None,
        })
        .collect();

    grid.0 += 1;
    grid.1 += 1;
    (start, grid, moves)
}

fn walk_grid(start: Point, mut grid: Grid, moves: Vec<Move>, part: u32) -> Grid {
    let mut robot = start;
    for mv in moves {
        let mut todos = Vec::<(Point, char)>::new();
        let mut chk = robot;
        let mut move_robot = false;

        if part == 2 && mv.0 == 0 {
            let mut fields = Vec::<Point>::new();
            fields.push(chk);
            'outer: loop {
                if fields
                    .iter()
                    .all(|e| *grid.2.get(&(e.0 + mv.0, e.1 + mv.1)).unwrap() == '.')
                {
                    move_robot = true;
                    break 'outer;
                }
                if fields
                    .iter()
                    .any(|e| *grid.2.get(&(e.0 + mv.0, e.1 + mv.1)).unwrap() == '#')
                {
                    break 'outer;
                }

                let mut boxes = HashSet::<Point>::new();
                for field in &fields {
                    if *grid.2.get(&(field.0 + mv.0, field.1 + mv.1)).unwrap() == '[' {
                        boxes.insert((field.0 + mv.0, field.1 + mv.1));
                        boxes.insert((field.0 + mv.0 + 1, field.1 + mv.1));
                    }
                    if *grid.2.get(&(field.0 + mv.0, field.1 + mv.1)).unwrap() == ']' {
                        boxes.insert((field.0 + mv.0, field.1 + mv.1));
                        boxes.insert((field.0 + mv.0 - 1, field.1 + mv.1));
                    }
                }

                fields.clear();
                for bx in boxes {
                    fields.push(bx);
                    todos.push((bx, *grid.2.get(&bx).unwrap()));
                }
            }
        } else {
            chk = (robot.0 + mv.0, robot.1 + mv.1);
            loop {
                match grid.2.get(&chk) {
                    Some('.') => {
                        move_robot = true;
                        break;
                    }
                    Some('O') | Some('[') | Some(']') => {
                        todos.push((chk, *grid.2.get(&chk).unwrap()))
                    }
                    _ => break,
                }
                chk = (chk.0 + mv.0, chk.1 + mv.1);
            }
        }

        if move_robot {
            robot = (robot.0 + mv.0, robot.1 + mv.1);
            for todo in &todos {
                grid.2.entry(todo.0).and_modify(|e| *e = '.');
            }
            for todo in &todos {
                grid.2
                    .entry((todo.0 .0 + mv.0, todo.0 .1 + mv.1))
                    .and_modify(|e| *e = todo.1);
            }
        }
    }

    grid
}

fn calc_score(grid: Grid) -> u32 {
    grid.2
        .iter()
        .filter_map(|(k, v)| match v {
            'O' | '[' => Some(k.1 * 100 + k.0),
            _ => None,
        })
        .collect::<Vec<i32>>()
        .iter()
        .sum::<i32>() as u32
}

pub fn part_one(input: &str) -> Option<u32> {
    let (start, grid, moves) = parse(input, 1);
    Some(calc_score(walk_grid(start, grid, moves, 1)))
}

pub fn part_two(input: &str) -> Option<u32> {
    let (start, grid, moves) = parse(input, 2);
    Some(calc_score(walk_grid(start, grid, moves, 2)))
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let mut result = part_one(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(2028));
        result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(10092));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(9021));
    }
}
