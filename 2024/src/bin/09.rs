use std::iter;

advent_of_code::solution!(9);

#[derive(Debug, Copy, Clone, Eq, PartialEq)]
struct Block {
    id: i64,
    idx: usize,
    size: usize,
}

impl Ord for Block {
    fn cmp(&self, other: &Self) -> std::cmp::Ordering {
        self.idx.cmp(&other.idx)
    }
}

impl PartialOrd for Block {
    fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering> {
        Some(self.cmp(other))
    }
}

fn parse(input: &str) -> Vec<i64> {
    let mut disk = Vec::new();
    for (idx, c) in input.chars().enumerate() {
        if c.is_ascii_digit() {
            let cnt = c.to_digit(10).unwrap();
            if idx % 2 == 1 {
                disk.extend(iter::repeat(-1_i64).take(cnt as usize));
            } else {
                disk.extend(iter::repeat((idx / 2) as i64).take(cnt as usize));
            }
        }
    }
    disk
}

fn parse_p2(input: &str) -> (Vec<Block>, Vec<Block>) {
    let mut full = Vec::new();
    let mut empty = Vec::new();

    let mut idx = 0;
    for (i, c) in input.chars().enumerate() {
        if c.is_ascii_digit() {
            let size = c.to_digit(10).unwrap() as usize;
            if i % 2 == 1 {
                empty.push(Block {
                    id: -1_i64,
                    idx,
                    size,
                });
            } else {
                full.push(Block {
                    id: (i / 2) as i64,
                    idx,
                    size,
                });
            }
            idx += size;
        }
    }
    (full, empty)
}

fn checksum(disk: Vec<i64>) -> Option<i64> {
    Some(
        disk.iter()
            .enumerate()
            .filter(|(_, &x)| x >= 0)
            .map(|(i, x)| x * (i as i64))
            .sum::<i64>(),
    )
}

fn checksum_block(blk: &Block) -> i64 {
    let mut ret = 0;
    for i in blk.idx..(blk.idx + blk.size) {
        ret += (i as i64) * blk.id;
    }
    ret
}

pub fn part_one(input: &str) -> Option<i64> {
    let mut disk = parse(input);

    let mut left_idx = 0;
    let mut right_idx = disk.len() - 1;
    loop {
        while left_idx < disk.len() && disk[left_idx] >= 0 {
            left_idx += 1;
        }
        while right_idx > 0 && disk[right_idx] < 0 {
            right_idx -= 1;
        }

        if left_idx >= right_idx {
            break;
        }

        disk.swap(left_idx, right_idx);
        left_idx += 1;
        right_idx -= 1;
    }

    checksum(disk)
}

pub fn part_two(input: &str) -> Option<i64> {
    let (full, mut empty) = parse_p2(input);

    let mut disk = Vec::<Block>::new();
    for file in full.iter().rev() {
        let chk_idx = empty
            .iter()
            .position(|x| x.idx < file.idx && x.size >= file.size);
        if chk_idx.is_none() {
            disk.push(*file);
            continue;
        }
        let space_idx = chk_idx.unwrap();
        let space = empty[space_idx];
        empty.remove(space_idx);
        disk.push(Block {
            id: file.id,
            idx: space.idx,
            size: file.size,
        });
        if space.size > file.size {
            empty.push(Block {
                id: space.id,
                idx: space.idx + file.size,
                size: space.size - file.size,
            });
        }
        empty.sort();
    }

    Some(disk.iter().map(checksum_block).sum())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(1928));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(2858));
    }
}
