use itertools::Itertools;
use std::collections::HashSet;

advent_of_code::solution!(14);

type Robot = (i32, i32, i32, i32);

fn parse(input: &str) -> Vec<Robot> {
    let mut ret = Vec::new();

    for line in input.lines() {
        ret.push(
            line.split(&[',', ' '][..])
                .map(|e| {
                    e.chars()
                        .filter(|c| c.is_ascii_digit() || *c == '-')
                        .collect::<String>()
                        .parse::<i32>()
                })
                .filter_map(|e| e.ok())
                .collect_tuple()
                .expect("parse failed"),
        );
    }

    ret
}

pub fn part_one(input: &str) -> Option<usize> {
    let mut width = 101;
    let mut length = 103;
    let mut robots = parse(input);
    if robots.len() == 12 {
        width = 11;
        length = 7;
    }

    for _ in 0..100 {
        for robot in &mut robots {
            robot.0 = (robot.0 + robot.2).rem_euclid(width);
            robot.1 = (robot.1 + robot.3).rem_euclid(length);
        }
    }

    let mut ret = 1;
    for x in [0, (width / 2 + 1)] {
        for y in [0, (length / 2 + 1)] {
            ret *= robots
                .iter()
                .filter(|e| e.0 >= x && e.0 < x + width / 2 && e.1 >= y && e.1 < y + length / 2)
                .count();
        }
    }
    Some(ret)
}

pub fn part_two(input: &str) -> Option<u32> {
    let mut width = 101;
    let mut length = 103;
    let mut robots = parse(input);
    if robots.len() == 12 {
        width = 11;
        length = 7;
    }

    let mut ret = 0;
    loop {
        ret += 1;
        let mut seen = HashSet::<(i32, i32)>::new();
        for robot in &mut robots {
            robot.0 = (robot.0 + robot.2).rem_euclid(width);
            robot.1 = (robot.1 + robot.3).rem_euclid(length);
            seen.insert((robot.0, robot.1));
        }
        if seen.len() == robots.len() {
            break;
        }
    }

    Some(ret)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(12));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(1));
    }
}
