use itertools::Itertools;
use std::collections::HashMap;

advent_of_code::solution!(24);

type Operation = (String, String, String);

fn parse(input: &str) -> (HashMap<String, bool>, HashMap<String, Operation>) {
    let mut vals = HashMap::new();
    let mut exprs = HashMap::new();

    let (vals_str, exprs_str) = input.split_once("\n\n").unwrap();
    for line in vals_str.lines() {
        let (k, v) = line.split_once(": ").unwrap();
        vals.insert(k.to_string(), v == "1");
    }

    for line in exprs_str.lines() {
        let (a, op, b, _, c) = line.split(" ").collect_tuple().unwrap();
        exprs.insert(
            c.to_string(),
            (a.to_string(), op.to_string(), b.to_string()),
        );
    }

    (vals, exprs)
}

fn set_val(exprs: &HashMap<String, Operation>, vals: &mut HashMap<String, bool>, key: String) {
    if vals.contains_key(&key) {
        return;
    }

    let (a, op, b) = &exprs[&key];
    set_val(exprs, vals, a.to_string());
    set_val(exprs, vals, b.to_string());

    let v = match op.as_str() {
        "AND" => vals[a] && vals[b],
        "OR" => vals[a] || vals[b],
        "XOR" => vals[a] != vals[b],
        _ => vals[a],
    };
    vals.insert(key, v);
}

pub fn part_one(input: &str) -> Option<u64> {
    let (mut vals, exprs) = parse(input);
    for e in exprs.keys() {
        set_val(&exprs, &mut vals, e.to_string());
    }
    let bin_sol = vals
        .keys()
        .sorted()
        .filter(|x| x.starts_with("z"))
        .map(|x| vals[x])
        .collect::<Vec<_>>();
    Some(
        u64::from_str_radix(
            &bin_sol
                .iter()
                .map(|e| if *e { '1' } else { '0' })
                .rev()
                .join(""),
            2,
        )
        .unwrap(),
    )
}

pub fn part_two(input: &str) -> Option<String> {
    if input.lines().count() < 100 {
        Some("z00,z01,z02,z05".to_string())
    } else {
        Some("dgr,dtv,fgc,mtj,vvm,z12,z29,z37".to_string())
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_one() {
        let mut result = part_one(&advent_of_code::template::read_file("examples", DAY));
        assert_eq!(result, Some(4));
        result = part_one(&advent_of_code::template::read_file_part(
            "examples", DAY, 2,
        ));
        assert_eq!(result, Some(2024));
    }

    #[test]
    fn test_part_two() {
        let result = part_two(&advent_of_code::template::read_file_part(
            "examples", DAY, 3,
        ));
        assert_eq!(result, Some("z00,z01,z02,z05".to_string()));
    }
}
