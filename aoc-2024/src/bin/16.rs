use std::{
    cmp::Ordering,
    collections::{HashMap, HashSet, VecDeque},
};

advent_of_code::solution!(16);

type Point = (i32, i32);
type Move = (i32, i32);
type Grid = (i32, i32, HashMap<Point, char>);

fn parse(input: &str) -> (Point, Point, Grid) {
    let mut grid: Grid = (0, 0, HashMap::<Point, char>::new());
    let mut start: Point = (0, 0);
    let mut end: Point = (0, 0);

    for (y, line) in input.lines().enumerate() {
        for (x, c) in line.chars().enumerate() {
            if c == 'S' {
                start.0 = x as i32;
                start.1 = y as i32;
            }
            if c == 'E' {
                end.0 = x as i32;
                end.1 = y as i32;
            }
            grid.2.insert((x as i32, y as i32), c);
            grid.0 = x as i32;
            grid.1 = y as i32;
        }
    }

    grid.0 += 1;
    grid.1 += 1;
    (start, end, grid)
}

fn solve_maze(
    start: Point,
    end: Point,
    grid: Grid,
) -> (HashMap<(Point, Move), u32>, HashSet<Point>) {
    let mut best = u32::MAX;
    let mut queue = VecDeque::<(Point, Move, HashSet<Point>, u32)>::new();
    queue.push_back((start, (1, 0), HashSet::<Point>::new(), 0));
    let mut visited = HashMap::<(Point, Move), u32>::new();
    let mut tiles = HashSet::<Point>::new();

    while let Some((pos, dir, path, score)) = queue.pop_front() {
        if visited.contains_key(&(pos, dir)) && *visited.get(&(pos, dir)).unwrap() < score {
            continue;
        }
        visited.insert((pos, dir), score);

        if pos == end {
            match score.cmp(&best) {
                Ordering::Less => {
                    best = score;
                    tiles.clear();
                    tiles.extend(&path);
                }
                Ordering::Equal => {
                    tiles.extend(&path);
                }
                Ordering::Greater => {}
            }
            continue;
        }

        let nxt = (pos.0 + dir.0, pos.1 + dir.1);
        if grid.2.contains_key(&nxt) && *grid.2.get(&nxt).unwrap() != '#' {
            let mut nxt_path = path.clone();
            nxt_path.insert(pos);
            queue.push_back((nxt, dir, nxt_path, score + 1));
        }
        queue.push_back((pos, (dir.1, -dir.0), path.clone(), score + 1000));
        queue.push_back((pos, (-dir.1, dir.0), path, score + 1000));
    }

    (visited, tiles)
}

pub fn part_one(input: &str) -> Option<u32> {
    let (start, end, grid) = parse(input);
    let (visited, _) = solve_maze(start, end, grid);

    Some(
        visited
            .iter()
            .filter(|(k, _)| k.0 == end)
            .map(|(_, v)| *v)
            .min()
            .unwrap(),
    )
}

pub fn part_two(input: &str) -> Option<usize> {
    let (start, end, grid) = parse(input);
    let (_, tiles) = solve_maze(start, end, grid);

    Some(tiles.len() + 1)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let mut result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(7036));
        result = part_one(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(11048));
    }

    #[test]
    fn test_part_two() {
        let mut result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(45));
        result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(64));
    }
}
