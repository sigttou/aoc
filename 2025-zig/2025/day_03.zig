pub fn part1(input: []const u8) !i64 {
    var sum: i64 = 0;

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        const max_joltage = findMaxJoltage(line);
        sum += max_joltage;
    }

    return sum;
}

fn findMaxJoltage(bank: []const u8) i64 {
    var max: i64 = 0;

    for (bank, 0..) |digit1, i| {
        if (digit1 < '0' or digit1 > '9') continue;

        for (bank[i + 1 ..]) |digit2| {
            if (digit2 < '0' or digit2 > '9') continue;

            const tens = digit1 - '0';
            const units = digit2 - '0';
            const joltage = tens * 10 + units;

            if (joltage > max) {
                max = joltage;
            }
        }
    }

    return max;
}

pub fn part2(input: []u8) !i128 {
    const allocator = std.heap.page_allocator;
    var sum: i128 = 0;

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        const max_joltage = findMaxJoltage12(line, allocator);
        sum += max_joltage;
    }

    return sum;
}

fn findMaxJoltage12(bank: []const u8, allocator: std.mem.Allocator) i128 {
    const target: usize = 12;
    var result = std.array_list.Managed(u8).init(allocator);

    var pos: usize = 0;
    var needed: usize = target;

    while (needed > 0 and pos < bank.len) {
        const available = bank.len - pos;
        const can_skip = available - needed;

        var best_digit: u8 = 0;
        var best_pos: usize = pos;

        var i: usize = 0;
        while (i <= can_skip and pos + i < bank.len) : (i += 1) {
            const digit = bank[pos + i];
            if (digit >= '0' and digit <= '9') {
                if (digit > best_digit) {
                    best_digit = digit;
                    best_pos = pos + i;
                }
            }
        }

        if (best_digit >= '0' and best_digit <= '9') {
            result.append(best_digit) catch unreachable;
            needed -= 1;
            pos = best_pos + 1;
        } else {
            pos += 1;
        }
    }

    var num: i128 = 0;
    for (result.items) |digit| {
        num = num * 10 + (digit - '0');
    }

    return num;
}

const std = @import("std");
