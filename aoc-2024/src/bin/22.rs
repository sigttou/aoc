use itertools::{max, Itertools};
use std::collections::{HashMap, VecDeque};

advent_of_code::solution!(22);

fn parse(input: &str) -> Vec<i64> {
    input
        .lines()
        .map(|line| line.parse::<i64>().unwrap())
        .collect::<Vec<_>>()
}

fn step1(num: i64) -> i64 {
    ((num << 6) ^ num) % 16777216
}

fn step2(num: i64) -> i64 {
    ((num / 32) ^ num) % 16777216
}

fn step3(num: i64) -> i64 {
    ((num << 11) ^ num) % 16777216
}

pub fn part_one(input: &str) -> Option<i64> {
    Some(
        parse(input)
            .into_iter()
            .map(|e| {
                let mut ret = e;
                for _ in 0..2000 {
                    ret = step3(step2(step1(ret)))
                }
                ret
            })
            .sum(),
    )
}

pub fn part_two(input: &str) -> Option<i64> {
    let numbers = parse(input);
    let mut sequences = HashMap::<(i64, i64, i64, i64), (i64, i64)>::new();

    let mut s = VecDeque::<i64>::new();
    for (i, num) in numbers.iter().enumerate() {
        let mut chk = *num;
        for _ in 0..2000 {
            let tmp = step3(step2(step1(chk)));
            s.push_back((tmp % 10) - (chk % 10));
            if s.len() == 5 {
                s.pop_front();
            }
            if s.len() == 4 {
                let key = s
                    .clone()
                    .into_iter()
                    .collect_tuple::<(i64, i64, i64, i64)>()
                    .unwrap();
                let val = sequences.entry(key).or_insert((0, -1));
                if val.1 < i as i64 {
                    val.1 = i as i64;
                    val.0 += tmp % 10;
                }
            }
            chk = tmp;
        }
    }

    max(sequences.values().map(|e| e.0))
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(37327623));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(23));
    }
}
