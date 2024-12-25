advent_of_code::solution!(25);

fn parse(input: &str) -> Vec<Vec<Vec<char>>> {
    input
        .split("\n\n")
        .map(|block| {
            block
                .lines()
                .map(|line| line.chars().collect::<Vec<_>>())
                .collect::<Vec<_>>()
        })
        .collect::<Vec<_>>()
}

fn chk_fit(a: &[Vec<char>], b: &[Vec<char>]) -> bool {
    for i in 0..a.len() {
        for j in 0..a[i].len() {
            if a[i][j] == '#' && a[i][j] == b[i][j] {
                return false;
            }
        }
    }
    true
}

pub fn part_one(input: &str) -> Option<u32> {
    let schematics = parse(input);
    let mut ret = 0;

    for i in 0..schematics.len() {
        for j in i + 1..schematics.len() {
            let a = &schematics[i];
            let b = &schematics[j];
            if chk_fit(a, b) {
                ret += 1;
            }
        }
    }

    Some(ret)
}

pub fn part_two(_input: &str) -> Option<String> {
    Some("Click Button".to_string())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(3));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some("Click Button".to_string()));
    }
}
