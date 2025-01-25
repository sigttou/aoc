use regex::Regex;

advent_of_code::solution!(3);

pub fn part_one(input: &str) -> Option<u32> {
    let rgx = Regex::new(r"mul\((\d{1,3}),(\d{1,3})\)").unwrap();
    let mut sum = 0;
    for e in rgx.captures_iter(input) {
        sum += e[1].parse::<u32>().unwrap() * e[2].parse::<u32>().unwrap();
    }
    Some(sum)
}

pub fn part_two(input: &str) -> Option<u32> {
    let rgx = Regex::new(r"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)").unwrap();
    let mut sum = 0;
    let mut chk = true;
    for e in rgx.captures_iter(input) {
        match &e[0] {
            "do()" => {
                chk = true;
            }
            "don't()" => {
                chk = false;
            }
            _ => {
                if chk {
                    sum += e[1].parse::<u32>().unwrap() * e[2].parse::<u32>().unwrap();
                }
            }
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
        assert_eq!(result, Some(161));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(48));
    }
}
