use itertools::Itertools;

type XY = (i64, i64);
type Entry = (XY, XY, XY);
advent_of_code::solution!(13);

fn parse(input: &str) -> Vec<Entry> {
    let mut ret = Vec::new();

    for entry in input.split("\n\n") {
        ret.push(
            entry
                .lines()
                .map(|line| {
                    line.split_ascii_whitespace()
                        .map(|e| {
                            e.chars()
                                .filter(|c| c.is_ascii_digit())
                                .collect::<String>()
                                .parse::<i64>()
                        })
                        .filter_map(|e| e.ok())
                        .collect_tuple::<(i64, i64)>()
                        .unwrap()
                })
                .collect_tuple::<Entry>()
                .unwrap(),
        )
    }

    ret
}

fn solve(entry: Entry) -> i64 {
    let ((ax, ay), (bx, by), (px, py)) = entry;

    let m = (px * by - py * bx) / (ax * by - ay * bx);
    let n = (py - ay * m) / by;
    if m * (ax * by - ay * bx) != (px * by - py * bx) || n * by != (py - ay * m) {
        return 0;
    }

    3 * m + n
}

pub fn part_one(input: &str) -> Option<i64> {
    Some(parse(input).into_iter().map(solve).sum())
}

pub fn part_two(input: &str) -> Option<i64> {
    Some(
        parse(input)
            .into_iter()
            .map(|e| solve((e.0, e.1, (e.2 .0 + 10000000000000, e.2 .1 + 10000000000000))))
            .sum(),
    )
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(480));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(875318608908));
    }
}
