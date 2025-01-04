advent_of_code::solution!(1);

fn parse(input: &str) -> (Vec<u32>, Vec<u32>) {
    let mut ls = vec![];
    let mut rs = vec![];

    for line in input.lines() {
        let split: Vec<&str> = line.split_whitespace().collect();
        let l: u32 = split[0].parse().unwrap();
        let r: u32 = split[1].parse().unwrap();
        ls.push(l);
        rs.push(r);
    }

    ls.sort();
    rs.sort();
    (ls, rs)
}

pub fn part_one(input: &str) -> Option<u32> {
    let (ls, rs) = parse(input);
    let mut sum = 0;
    for (l, r) in ls.iter().zip(rs.iter()) {
        sum += l.abs_diff(*r);
    }
    Some(sum)
}

pub fn part_two(input: &str) -> Option<u32> {
    let (ls, rs) = parse(input);
    let mut sum = 0;
    for r in rs.iter() {
        sum += r * ls.iter().filter(|&n| *n == *r).count() as u32;
    }
    Some(sum)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(11));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(31));
    }
}
