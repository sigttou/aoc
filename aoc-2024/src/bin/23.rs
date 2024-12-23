use std::collections::{HashMap, HashSet};

use itertools::Itertools;

advent_of_code::solution!(23);

fn parse(input: &str) -> HashMap<String, HashSet<String>> {
    let mut ret = HashMap::<String, HashSet<String>>::new();

    for line in input.lines() {
        let (a, b) = line.split_once("-").unwrap();
        ret.entry(a.to_string()).or_default().insert(b.to_string());
        ret.entry(b.to_string()).or_default().insert(a.to_string());
    }

    ret
}

fn get_triangles(graph: HashMap<String, HashSet<String>>) -> HashSet<Vec<String>> {
    let mut ret = HashSet::new();

    for node in graph.keys() {
        let neighs = graph.get(node).unwrap();
        for neigh in neighs {
            for third in neighs.intersection(graph.get(neigh).unwrap()) {
                let mut to_insert = vec![node.clone(), neigh.clone(), third.clone()];
                to_insert.sort();
                ret.insert(to_insert);
            }
        }
    }

    ret
}

pub fn part_one(input: &str) -> Option<u32> {
    Some(
        get_triangles(parse(input))
            .into_iter()
            .filter(|nodes| nodes.iter().any(|x| x.starts_with('t')))
            .collect::<Vec<_>>()
            .len() as u32,
    )
}

fn find_largest_clique(graph: HashMap<String, HashSet<String>>) -> HashSet<String> {
    let mut ret = HashSet::new();

    for node in graph.keys() {
        let mut group = HashSet::<String>::new();
        group.insert(node.clone());
        for neigh in graph.get(node).unwrap() {
            if group.iter().all(|e| graph.get(neigh).unwrap().contains(e)) {
                group.insert(neigh.clone());
            }
        }
        if group.len() > ret.len() {
            ret = group;
        }
    }

    ret
}

pub fn part_two(input: &str) -> Option<String> {
    Some(
        find_largest_clique(parse(input))
            .into_iter()
            .sorted()
            .join(","),
    )
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(7));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(String::from("co,de,ka,ta")));
    }
}
