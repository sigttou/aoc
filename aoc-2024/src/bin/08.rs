use itertools::Itertools;
use std::collections::{HashMap, HashSet};

type Coord = (usize, usize);

advent_of_code::solution!(8);

fn parse(input: &str) -> (Coord, HashMap<char, HashSet<Coord>>) {
    let mut antennas = HashMap::<char, HashSet<Coord>>::new();
    let mut dims = (0, 0);

    for (y, line) in input.lines().enumerate() {
        for (x, e) in line.chars().enumerate() {
            dims = (x + 1, y + 1);
            if e != '.' {
                antennas.entry(e).or_default().insert((x, y));
            }
        }
    }

    (dims, antennas)
}

pub fn part_one(input: &str) -> Option<u32> {
    let (dims, antennas) = parse(input);
    let mut anti_nodes = HashSet::<Coord>::new();

    for v in antennas.values() {
        for e in v.iter().permutations(2) {
            let (x1, y1) = (e[0].0 as i32, e[0].1 as i32);
            let (x2, y2) = (e[1].0 as i32, e[1].1 as i32);
            let ax: i32 = x2 + (x2 - x1);
            let ay: i32 = y2 + (y2 - y1);

            if ax >= 0 && ay >= 0 && ax < dims.0 as i32 && ay < dims.1 as i32 {
                anti_nodes.insert((ax as usize, ay as usize));
            }
        }
    }

    Some(anti_nodes.len() as u32)
}

pub fn part_two(input: &str) -> Option<u32> {
    let (dims, antennas) = parse(input);
    let mut anti_nodes = HashSet::<Coord>::new();

    for v in antennas.values() {
        for e in v.iter().permutations(2) {
            let (x1, y1) = (e[0].0 as i32, e[0].1 as i32);
            let (x2, y2) = (e[1].0 as i32, e[1].1 as i32);
            let mut ax: i32 = x2;
            let mut ay: i32 = y2;

            while ax >= 0 && ay >= 0 && ax < dims.0 as i32 && ay < dims.1 as i32 {
                anti_nodes.insert((ax as usize, ay as usize));
                ax += x2 - x1;
                ay += y2 - y1;
            }
        }
    }

    Some(anti_nodes.len() as u32)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(14));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(34));
    }
}
