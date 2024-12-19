use std::collections::HashMap;

advent_of_code::solution!(19);

fn parse(input: &str) -> (Vec<String>, Vec<String>) {
    let (avail_str, target_str) = input.split_once("\n\n").unwrap();

    (
        avail_str
            .split(", ")
            .map(|e| e.to_string())
            .collect::<Vec<_>>(),
        target_str
            .lines()
            .map(|e| e.to_string())
            .collect::<Vec<_>>(),
    )
}

fn solve(cache: &mut HashMap<String, u64>, target: &String, avail: &Vec<String>, p: u32) -> u64 {
    if let Some(ret) = cache.get(target) {
        return *ret;
    }

    let mut chk = 0;
    for start in avail {
        if target.starts_with(start) {
            let val = solve(cache, &target[start.len()..].to_string(), avail, p);
            if val > 0 {
                if p == 1 {
                    cache.insert(target.to_string(), 1);
                    return 1;
                } else {
                    chk += val;
                }
            }
        }
    }

    cache.insert(target.to_string(), chk);
    chk
}

pub fn part_one(input: &str) -> Option<u64> {
    let (avail, targets) = parse(input);
    let mut cache = HashMap::<String, u64>::new();
    cache.insert("".to_string(), 1);
    Some(
        targets
            .iter()
            .map(|t| solve(&mut cache, t, &avail, 1))
            .sum::<u64>(),
    )
}

pub fn part_two(input: &str) -> Option<u64> {
    let (avail, targets) = parse(input);
    let mut cache = HashMap::<String, u64>::new();
    cache.insert("".to_string(), 1);
    Some(
        targets
            .iter()
            .map(|t| solve(&mut cache, t, &avail, 2))
            .sum::<u64>(),
    )
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(6));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(16));
    }
}
