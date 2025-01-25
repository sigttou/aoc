use std::collections::HashMap;

advent_of_code::solution!(11);

fn parse(input: &str) -> HashMap<usize, usize> {
    input
        .split_ascii_whitespace()
        .map(|e| (e.parse::<usize>().unwrap(), 1))
        .collect::<HashMap<usize, usize>>()
}

fn blink(stones: HashMap<usize, usize>) -> HashMap<usize, usize> {
    let mut ret = HashMap::<usize, usize>::new();
    for (stone, cnt) in stones {
        match stone {
            0 => *ret.entry(1).or_default() += cnt,
            _ => {
                let digits = stone.ilog10() + 1;
                if digits % 2 == 0 {
                    *ret.entry(stone / 10_usize.pow(digits / 2)).or_default() += cnt;
                    *ret.entry(stone % 10_usize.pow(digits / 2)).or_default() += cnt;
                } else {
                    *ret.entry(stone * 2024).or_default() += cnt;
                }
            }
        }
    }
    ret
}

pub fn part_one(input: &str) -> Option<usize> {
    let mut stones = parse(input);
    for _ in 0..25 {
        stones = blink(stones);
    }
    Some(stones.values().sum())
}

pub fn part_two(input: &str) -> Option<usize> {
    let mut stones = parse(input);
    for _ in 0..75 {
        stones = blink(stones);
    }
    Some(stones.values().sum())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(55312));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(65601038650482));
    }
}
