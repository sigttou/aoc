advent_of_code::solution!(7);

fn parse(input: &str) -> Vec<Vec<usize>> {
    let mut ret = Vec::new();
    for line in input.lines() {
        ret.push(
            line.split_ascii_whitespace()
                .map(|x| {
                    x.chars()
                        .filter(|c| c.is_ascii_digit())
                        .collect::<String>()
                        .parse()
                        .unwrap()
                })
                .collect(),
        )
    }
    ret
}

fn usize_concat(a: usize, b: usize) -> usize {
    // https://www.reddit.com/r/rust/comments/191l3ot/concatinate_two_numbers/
    a * 10usize.pow(b.ilog10() + 1) + b
}

fn can_solve(res: usize, tmp: usize, vals: &[usize], p2: bool) -> bool {
    if vals.len() == 1 {
        return res == tmp + vals[0]
            || res == tmp * vals[0]
            || (p2 && res == usize_concat(tmp, vals[0]));
    }
    can_solve(res, tmp + vals[0], &vals[1..], p2)
        || can_solve(res, tmp * vals[0], &vals[1..], p2)
        || (p2 && can_solve(res, usize_concat(tmp, vals[0]), &vals[1..], p2))
}

pub fn part_one(input: &str) -> Option<usize> {
    let mut ret = 0;
    for eq in parse(input) {
        if can_solve(eq[0], 0, &eq[1..], false) {
            ret += eq[0];
        }
    }
    Some(ret)
}

pub fn part_two(input: &str) -> Option<usize> {
    let mut ret = 0;
    for eq in parse(input) {
        if can_solve(eq[0], 0, &eq[1..], true) {
            ret += eq[0];
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
        assert_eq!(result, Some(3749));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(11387));
    }
}
