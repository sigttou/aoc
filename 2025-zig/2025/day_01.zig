pub fn part1(input: []const u8) [:0]const u8 {
    var position: i32 = 50;
    var count: u32 = 0;

    var lines = std.mem.tokenizeSequence(u8, input, "\n");
    while (lines.next()) |line| {
        const direction = line[0];
        const distance = std.fmt.parseInt(i32, line[1..], 10) catch continue;

        if (direction == 'L') {
            position -= distance;
        } else if (direction == 'R') {
            position += distance;
        }

        position = @mod(position, 100);

        if (position == 0) {
            count += 1;
        }
    }

    var buf: [32]u8 = undefined;
    const result = std.fmt.bufPrintZ(&buf, "{d}", .{count}) catch unreachable;

    const allocator = std.heap.page_allocator;
    const result_copy = allocator.dupeZ(u8, result) catch unreachable;
    return result_copy;
}

pub fn part2(input: []u8) !i32 {
    var position: i32 = 50;
    var count: i32 = 0;

    var lines = std.mem.tokenizeSequence(u8, input, "\n");
    while (lines.next()) |line| {
        const direction = line[0];
        const distance = std.fmt.parseInt(i32, line[1..], 10) catch continue;

        if (direction == 'R') {
            const first_zero = @mod(100 - position, 100);
            if (first_zero == 0) {
                count += @divFloor(distance, 100);
            } else if (distance >= first_zero) {
                count += 1 + @divFloor(distance - first_zero, 100);
            }
            position = @mod(position + distance, 100);
        } else {
            if (position == 0) {
                count += @divFloor(distance, 100);
            } else if (distance >= position) {
                count += 1 + @divFloor(distance - position, 100);
            }
            position = @mod(position - distance, 100);
        }
    }

    return count;
}

const std = @import("std");
