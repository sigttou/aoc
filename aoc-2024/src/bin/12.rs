use std::collections::{HashMap, HashSet};

advent_of_code::solution!(12);

static DIRECTIONS: &[(i32, i32)] = &[(0, -1), (1, 0), (0, 1), (-1, 0)];

fn parse(input: &str) -> ((usize, usize), HashMap<(i32, i32), char>) {
    let mut grid = HashMap::new();
    let mut xlen = 0;
    let mut ylen = 0;
    for (y, line) in input.lines().enumerate() {
        for (x, c) in line.chars().enumerate() {
            grid.insert((x as i32, y as i32), c);
            xlen = x;
            ylen = y;
        }
    }
    ((xlen + 1, ylen + 1), grid)
}

fn get_areas(
    xlen: usize,
    ylen: usize,
    mut grid: HashMap<(i32, i32), char>,
) -> Vec<HashSet<(i32, i32)>> {
    let mut areas = Vec::<HashSet<(i32, i32)>>::new();
    for x in 0..xlen {
        for y in 0..ylen {
            let start = (x as i32, y as i32);
            if let Some(e) = grid.get(&start).copied() {
                grid.remove(&start);
                let mut area = HashSet::<(i32, i32)>::new();
                area.insert((x as i32, y as i32));
                let mut todo = Vec::<(i32, i32)>::new();
                todo.push((x as i32, y as i32));
                while let Some(pos) = todo.pop() {
                    for dir in DIRECTIONS {
                        let chk = (pos.0 + dir.0, pos.1 + dir.1);
                        if Some(e) == grid.get(&chk).copied() {
                            grid.remove(&chk);
                            todo.push(chk);
                            area.insert(chk);
                        }
                    }
                }
                areas.push(area);
            }
        }
    }
    areas
}

pub fn part_one(input: &str) -> Option<u32> {
    let ((xlen, ylen), grid) = parse(input);

    let areas = get_areas(xlen, ylen, grid);

    let mut ret = 0;
    for area in areas {
        let mut boarder_cnt = 0;
        for pos in &area {
            for dir in DIRECTIONS {
                let chk = (pos.0 + dir.0, pos.1 + dir.1);
                if !area.contains(&chk) {
                    boarder_cnt += 1;
                }
            }
        }
        ret += area.len() * boarder_cnt;
    }

    Some(ret as u32)
}

pub fn part_two(input: &str) -> Option<u32> {
    let ((xlen, ylen), grid) = parse(input);

    let areas = get_areas(xlen, ylen, grid);

    let mut ret = 0;
    for area in areas {
        let mut side_cnt = 0;

        for dir in DIRECTIONS {
            let mut side = HashSet::<(i32, i32)>::new();
            for pos in &area {
                let chk = (pos.0 + dir.0, pos.1 + dir.1);
                if !area.contains(&chk) {
                    side.insert(chk);
                }
            }

            let mut to_remove = HashSet::<(i32, i32)>::new();
            for pos in &side {
                let mut chk = (pos.0 + dir.1, pos.1 + dir.0);
                while side.contains(&chk) {
                    to_remove.insert(chk);
                    chk = (chk.0 + dir.1, chk.1 + dir.0);
                }
            }
            side_cnt += side.len() - to_remove.len();
        }
        ret += area.len() * side_cnt;
    }

    Some(ret as u32)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let mut result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(1930));
        result = part_one(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(140));
        result = part_one(&advent_of_code::template::read_file_part(
            "examples", DAY, 3,
        ));
        assert_eq!(result, Some(772));
    }

    #[test]
    fn test_part_two() {
        let mut result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(1206));
        result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(80));
        result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 3,
        ));
        assert_eq!(result, Some(436));
        result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 4,
        ));
        assert_eq!(result, Some(236));
        result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 5,
        ));
        assert_eq!(result, Some(368));
    }
}
