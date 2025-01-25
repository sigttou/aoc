use cached::proc_macro::cached;
use itertools::Itertools;
use lazy_static::lazy_static;
use std::collections::HashMap;

advent_of_code::solution!(21);

type Point = (i32, i32);

lazy_static! {
    static ref DIRPAD: HashMap<char, Point> = {
        let mut map = HashMap::new();
        map.insert(' ', (0, 0));
        map.insert('^', (1, 0));
        map.insert('A', (2, 0));
        map.insert('<', (0, 1));
        map.insert('v', (1, 1));
        map.insert('>', (2, 1));
        map
    };
    static ref NUMPAD: HashMap<char, Point> = {
        let mut map = HashMap::new();
        map.insert('7', (0, 0));
        map.insert('8', (1, 0));
        map.insert('9', (2, 0));
        map.insert('4', (0, 1));
        map.insert('5', (1, 1));
        map.insert('6', (2, 1));
        map.insert('1', (0, 2));
        map.insert('2', (1, 2));
        map.insert('3', (2, 2));
        map.insert(' ', (0, 3));
        map.insert('0', (1, 3));
        map.insert('A', (2, 3));
        map
    };
}

#[cached]
fn get_dir1_presses(code: String) -> u64 {
    let mut pos = DIRPAD.get(&'A').unwrap();
    let mut ret = 0;

    for c in code.chars() {
        let target = DIRPAD.get(&c).unwrap();
        ret += (target.0 - pos.0).abs() + (target.1 - pos.1).abs() + 1;
        pos = target;
    }

    ret as u64
}

#[cached]
fn get_dir2_presses(code: String, cnt: u64) -> u64 {
    if cnt == 0 {
        return get_dir1_presses(code);
    }

    let mut pos = *DIRPAD.get(&'A').unwrap();
    let mut ret: u64 = 0;

    for e in code.chars() {
        let target = *DIRPAD.get(&e).unwrap();
        let dx = target.0 - pos.0;
        let dy = target.1 - pos.1;

        let mut chk = match dy > 0 {
            true => std::iter::repeat_n('v', dy as usize),
            false => std::iter::repeat_n('^', -dy as usize),
        }
        .collect::<Vec<_>>();
        chk.extend(match dx > 0 {
            true => std::iter::repeat_n('>', dx as usize),
            false => std::iter::repeat_n('<', -dx as usize),
        });

        let mut shortest = u64::MAX;
        for mut perm in chk.iter().permutations(chk.len()) {
            let mut nxt = pos;
            for c in &perm {
                nxt = match c {
                    'v' => (nxt.0, nxt.1 + 1),
                    '^' => (nxt.0, nxt.1 - 1),
                    '>' => (nxt.0 + 1, nxt.1),
                    '<' => (nxt.0 - 1, nxt.1),
                    _ => pos,
                };
                if nxt == *DIRPAD.get(&' ').unwrap() {
                    break;
                }
            }
            if nxt == *DIRPAD.get(&' ').unwrap() {
                continue;
            }
            perm.push(&'A');
            shortest = shortest.min(get_dir2_presses(perm.into_iter().join(""), cnt - 1));
        }
        pos = target;
        ret += shortest;
    }

    ret
}

#[cached]
fn get_num_presses(code: String) -> u64 {
    let mut pos = *NUMPAD.get(&'A').unwrap();
    let mut ret: u64 = 0;

    for c in code.chars() {
        let target = *NUMPAD.get(&c).unwrap();
        let dx = target.0 - pos.0;
        let dy = target.1 - pos.1;

        let mut chk = match dy > 0 {
            true => std::iter::repeat_n('v', dy as usize),
            false => std::iter::repeat_n('^', -dy as usize),
        }
        .collect::<Vec<_>>();
        chk.extend(match dx > 0 {
            true => std::iter::repeat_n('>', dx as usize),
            false => std::iter::repeat_n('<', -dx as usize),
        });

        let mut shortest = u64::MAX;
        for mut perm in chk.iter().permutations(chk.len()) {
            let mut nxt = pos;
            for c in &perm {
                nxt = match c {
                    'v' => (nxt.0, nxt.1 + 1),
                    '^' => (nxt.0, nxt.1 - 1),
                    '>' => (nxt.0 + 1, nxt.1),
                    '<' => (nxt.0 - 1, nxt.1),
                    _ => pos,
                };
                if nxt == *NUMPAD.get(&' ').unwrap() {
                    break;
                }
            }
            if nxt == *NUMPAD.get(&' ').unwrap() {
                continue;
            }
            perm.push(&'A');
            shortest = shortest.min(get_dir2_presses(perm.into_iter().join(""), 1));
        }
        pos = target;
        if ret.checked_add(shortest).is_some() {
            ret += shortest;
        } else {
            ret = shortest;
        }
    }

    ret
}

pub fn part_one(input: &str) -> Option<u64> {
    Some(
        input
            .lines()
            .map(|line| {
                get_num_presses(line.to_string())
                    * (line
                        .chars()
                        .filter(|c| c.is_ascii_digit())
                        .collect::<String>()
                        .parse::<u64>()
                        .unwrap())
            })
            .sum::<u64>(),
    )
}

#[cached]
fn get_25num_presses(code: String) -> u64 {
    let mut pos = *NUMPAD.get(&'A').unwrap();
    let mut ret: u64 = 0;

    for c in code.chars() {
        let target = *NUMPAD.get(&c).unwrap();
        let dx = target.0 - pos.0;
        let dy = target.1 - pos.1;

        let mut chk = match dy > 0 {
            true => std::iter::repeat_n('v', dy as usize),
            false => std::iter::repeat_n('^', -dy as usize),
        }
        .collect::<Vec<_>>();
        chk.extend(match dx > 0 {
            true => std::iter::repeat_n('>', dx as usize),
            false => std::iter::repeat_n('<', -dx as usize),
        });

        let mut shortest = u64::MAX;
        for mut perm in chk.iter().permutations(chk.len()) {
            let mut nxt = pos;
            for c in &perm {
                nxt = match c {
                    'v' => (nxt.0, nxt.1 + 1),
                    '^' => (nxt.0, nxt.1 - 1),
                    '>' => (nxt.0 + 1, nxt.1),
                    '<' => (nxt.0 - 1, nxt.1),
                    _ => pos,
                };
                if nxt == *NUMPAD.get(&' ').unwrap() {
                    break;
                }
            }
            if nxt == *NUMPAD.get(&' ').unwrap() {
                continue;
            }
            perm.push(&'A');
            shortest = shortest.min(get_dir2_presses(perm.into_iter().join(""), 24));
        }
        pos = target;
        if ret.checked_add(shortest).is_some() {
            ret += shortest;
        } else {
            ret = shortest;
        }
    }

    ret
}

pub fn part_two(input: &str) -> Option<u64> {
    Some(
        input
            .lines()
            .map(|line| {
                get_25num_presses(line.to_string())
                    * (line
                        .chars()
                        .filter(|c| c.is_ascii_digit())
                        .collect::<String>()
                        .parse::<u64>()
                        .unwrap())
            })
            .sum::<u64>(),
    )
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(126384));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(154115708116294));
    }
}
