use itertools::Itertools;

advent_of_code::solution!(17);

fn parse(input: &str) -> (u64, u64, u64, Vec<u64>) {
    let (reg_str, prog_str) = input.split_once("\n\n").unwrap();

    let (a, b, c) = reg_str
        .lines()
        .map(|line| {
            line.chars()
                .filter(|c| c.is_ascii_digit())
                .collect::<String>()
                .parse::<u64>()
                .unwrap()
        })
        .collect_tuple()
        .unwrap();

    let prog = prog_str
        .chars()
        .filter_map(|c| match c.is_ascii_digit() {
            true => Some(c.to_digit(10).unwrap() as u64),
            false => None,
        })
        .collect::<Vec<u64>>();

    (a, b, c, prog)
}

fn eval(a: u64, b: u64, c: u64, ip: usize, prog: Vec<u64>) -> (Option<u64>, u64, u64, u64, usize) {
    let op = prog.get(ip).unwrap();
    let val = *prog.get(ip + 1).unwrap();
    let combo = match val {
        val if val < 4 => val,
        4 => a,
        5 => b,
        6 => c,
        _ => val,
    };

    match op {
        0 => (None, a / u64::pow(2, combo as u32), b, c, ip + 2),
        1 => (None, a, b ^ val, c, ip + 2),
        2 => (None, a, combo % 8, c, ip + 2),
        3 => {
            if a == 0 {
                (None, a, b, c, ip + 2)
            } else {
                (None, a, b, c, val as usize)
            }
        }
        4 => (None, a, b ^ c, c, ip + 2),
        5 => (Some(combo % 8), a, b, c, ip + 2),
        6 => (None, a, a / u64::pow(2, combo as u32), c, ip + 2),
        7 => (None, a, b, a / u64::pow(2, combo as u32), ip + 2),
        _ => (None, 0, 0, 0, 0),
    }
}

fn run_program(mut a: u64, mut b: u64, mut c: u64, prog: Vec<u64>) -> Vec<u64> {
    let mut ip: usize = 0;
    let mut out;
    let mut ret = Vec::new();
    let len = prog.len();

    while ip < len - 1 {
        (out, a, b, c, ip) = eval(a, b, c, ip, prog.clone());
        if let Some(out) = out {
            ret.push(out);
        }
    }
    ret
}

fn solve(prog: &Vec<u64>, pos: usize, brute: u64) -> Option<u64> {
    for tmp in 0..8 {
        if run_program(brute * 8 + tmp, 0, 0, prog.to_vec()) == prog[pos..] {
            if pos == 0 {
                return Some(brute * 8 + tmp);
            }
            if let Some(ret) = solve(prog, pos - 1, brute * 8 + tmp) {
                return Some(ret);
            }
        }
    }

    None
}

pub fn part_one(input: &str) -> Option<String> {
    let (a, b, c, prog) = parse(input);
    Some(run_program(a, b, c, prog).iter().join(","))
}

pub fn part_two(input: &str) -> Option<u64> {
    let (_, _, _, prog) = parse(input);
    let len = prog.len();
    solve(&prog, len - 1, 0)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(String::from("4,6,3,5,6,3,5,2,1,0")));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        // doesn't solve for example
        assert_eq!(result, None);
    }
}
