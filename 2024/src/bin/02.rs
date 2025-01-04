advent_of_code::solution!(2);

fn parse(input: &str) -> Vec<Vec<i32>> {
    let mut ret = vec![];

    for line in input.lines() {
        ret.push(
            line.split_whitespace()
                .map(|x| x.parse().unwrap())
                .collect(),
        );
    }

    ret
}

fn chk_vec(input: &[i32]) -> bool {
    let chk = input.windows(2).map(|x| x[1] - x[0]).collect::<Vec<_>>();
    if (chk.iter().all(|x| *x > 0) || chk.iter().all(|x| *x < 0))
        && (chk.iter().all(|x| x.abs() < 4))
    {
        return true;
    }
    false
}

pub fn part_one(input: &str) -> Option<u32> {
    let reports = parse(input);
    let mut ret = 0;

    for report in reports.iter() {
        if chk_vec(report) {
            ret += 1;
        }
    }

    Some(ret)
}

pub fn part_two(input: &str) -> Option<u32> {
    let reports = parse(input);
    let mut ret = 0;

    for report in reports.iter() {
        if chk_vec(report) {
            ret += 1;
            continue;
        }
        for i in 0..report.len() {
            let mut tmp = report.to_vec();
            tmp.remove(i);
            if chk_vec(&tmp) {
                ret += 1;
                break;
            }
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
        assert_eq!(result, Some(2));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(4));
    }
}
