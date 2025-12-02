pub fn part1(input: []const u8) [:0]const u8 {
    const allocator = std.heap.page_allocator;
    var sum: i64 = 0;

    var ranges = std.mem.splitScalar(u8, input, ',');
    while (ranges.next()) |range| {
        const trimmed = std.mem.trim(u8, range, " \n\r\t");

        var parts = std.mem.splitScalar(u8, trimmed, '-');
        const start_str = parts.next() orelse continue;
        const end_str = parts.next() orelse continue;

        const start = std.fmt.parseInt(i64, start_str, 10) catch continue;
        const end = std.fmt.parseInt(i64, end_str, 10) catch continue;

        var id = start;
        while (id <= end) : (id += 1) {
            if (isInvalidIdPart1(id)) {
                sum += id;
            }
        }
    }

    var buf: [64]u8 = undefined;
    const result = std.fmt.bufPrintZ(&buf, "{d}", .{sum}) catch unreachable;
    const result_copy = allocator.dupeZ(u8, result) catch unreachable;
    return result_copy;
}

fn isInvalidIdPart1(id: i64) bool {
    var buf: [32]u8 = undefined;
    const id_str = std.fmt.bufPrint(&buf, "{d}", .{id}) catch return false;

    if (id_str.len % 2 != 0) return false;

    const half_len = id_str.len / 2;
    const first_half = id_str[0..half_len];
    const second_half = id_str[half_len..];

    return std.mem.eql(u8, first_half, second_half);
}

fn isInvalidIdPart2(id: i64) bool {
    var buf: [32]u8 = undefined;
    const id_str = std.fmt.bufPrint(&buf, "{d}", .{id}) catch return false;

    const len = id_str.len;

    var pattern_len: usize = 1;
    while (pattern_len <= len / 2) : (pattern_len += 1) {
        if (len % pattern_len != 0) continue;

        const pattern = id_str[0..pattern_len];
        var is_valid = true;
        var pos: usize = pattern_len;

        while (pos < len) : (pos += pattern_len) {
            const segment = id_str[pos .. pos + pattern_len];
            if (!std.mem.eql(u8, pattern, segment)) {
                is_valid = false;
                break;
            }
        }

        if (is_valid) {
            return true;
        }
    }

    return false;
}

pub fn part2(input: []u8) !i64 {
    var sum: i64 = 0;

    var ranges = std.mem.splitScalar(u8, input, ',');
    while (ranges.next()) |range| {
        const trimmed = std.mem.trim(u8, range, " \n\r\t");

        var parts = std.mem.splitScalar(u8, trimmed, '-');
        const start_str = parts.next() orelse continue;
        const end_str = parts.next() orelse continue;

        const start = std.fmt.parseInt(i64, start_str, 10) catch continue;
        const end = std.fmt.parseInt(i64, end_str, 10) catch continue;

        var id = start;
        while (id <= end) : (id += 1) {
            if (isInvalidIdPart2(id)) {
                sum += id;
            }
        }
    }

    return sum;
}

const std = @import("std");
