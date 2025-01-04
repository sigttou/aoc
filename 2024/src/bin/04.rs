advent_of_code::solution!(4);

static XMAS: &[char] = &['X', 'M', 'A', 'S'];
static OFFSETS: &[(i32, i32)] = &[
    (0, -1),
    (0, 1),
    (-1, 0),
    (1, 0),
    (-1, -1),
    (1, -1),
    (-1, 1),
    (1, 1),
];

fn parse(input: &str) -> Vec<Vec<char>> {
    input
        .lines()
        .map(|x| x.chars().collect::<Vec<_>>())
        .collect()
}

fn chk_xmas(grid: &[Vec<char>], x: i32, y: i32, dx: i32, dy: i32) -> bool {
    for (i, e) in XMAS.iter().enumerate() {
        let x = x + (dx * i as i32);
        let y = y + (dy * i as i32);

        if x < 0
            || y < 0
            || y >= grid.len().try_into().unwrap()
            || x >= grid[0].len().try_into().unwrap()
            || grid[y as usize][x as usize] != *e
        {
            return false;
        }
    }
    true
}

fn chk_cross_mas(grid: &[Vec<char>], x: usize, y: usize) -> bool {
    if grid[y][x] != 'A' {
        return false;
    }

    if ((grid[y - 1][x - 1] == 'M' && grid[y + 1][x + 1] == 'S')
        || (grid[y - 1][x - 1] == 'S' && grid[y + 1][x + 1] == 'M'))
        && ((grid[y - 1][x + 1] == 'M' && grid[y + 1][x - 1] == 'S')
            || (grid[y - 1][x + 1] == 'S' && grid[y + 1][x - 1] == 'M'))
    {
        return true;
    }
    false
}

pub fn part_one(input: &str) -> Option<u32> {
    let grid = parse(input);
    let mut sum = 0;

    for y in 0..grid.len() {
        for x in 0..grid[0].len() {
            sum += OFFSETS
                .iter()
                .filter(|(dx, dy)| chk_xmas(&grid, x as i32, y as i32, *dx, *dy))
                .count();
        }
    }

    Some(sum.try_into().unwrap())
}

pub fn part_two(input: &str) -> Option<u32> {
    let grid = parse(input);
    let mut sum = 0;

    for y in 1..(grid.len() - 1) {
        for x in 1..(grid[0].len() - 1) {
            if chk_cross_mas(&grid, x, y) {
                sum += 1;
            }
        }
    }

    Some(sum.try_into().unwrap())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(18));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(9));
    }
}
