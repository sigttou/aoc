use std::cmp::Ordering::{Greater, Less};
use std::collections::{HashMap, HashSet};

advent_of_code::solution!(5);

fn parse(input: &str) -> (HashMap<u32, HashSet<u32>>, Vec<Vec<u32>>) {
    let (rules_str, pages_str) = input.split_once("\n\n").unwrap();

    let mut rules = HashMap::<u32, HashSet<u32>>::new();
    for e in rules_str.lines() {
        let (k, v) = e.split_once("|").unwrap();
        rules
            .entry(k.parse().unwrap())
            .or_default()
            .insert(v.parse().unwrap());
    }

    let pages = pages_str
        .split_ascii_whitespace()
        .map(|x| x.split(',').map(|e| e.parse::<u32>().unwrap()).collect())
        .collect();

    (rules, pages)
}

fn check_entry(rules: &HashMap<u32, HashSet<u32>>, numbers: &[u32]) -> bool {
    numbers.is_sorted_by(|a, b| {
        if let Some(e) = rules.get(a) {
            e.contains(b)
        } else {
            false
        }
    })
}

pub fn part_one(input: &str) -> Option<u32> {
    let (rules, pages) = parse(input);
    let mut sum = 0;

    for entry in pages {
        if check_entry(&rules, &entry) {
            sum += entry[entry.len() / 2];
        }
    }

    Some(sum)
}

pub fn part_two(input: &str) -> Option<u32> {
    let (rules, pages) = parse(input);
    let mut sum = 0;

    for mut entry in pages {
        if !check_entry(&rules, &entry) {
            entry.sort_by(|a, b| {
                if let Some(e) = rules.get(a) {
                    if e.contains(b) {
                        Less
                    } else {
                        Greater
                    }
                } else {
                    Greater
                }
            });
            sum += entry[entry.len() / 2];
        }
    }

    Some(sum)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(143));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(123));
    }
}
