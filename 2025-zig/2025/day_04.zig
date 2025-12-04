const directions = [_][2]i32{
    .{ -1, 0 },  .{ 1, 0 },  .{ 0, -1 }, .{ 0, 1 },
    .{ -1, -1 }, .{ -1, 1 }, .{ 1, -1 }, .{ 1, 1 },
};

pub fn part1(input: []const u8) i32 {
    const allocator = std.heap.page_allocator;
    var lines = std.ArrayListUnmanaged([]const u8){};

    var line_iter = std.mem.tokenizeScalar(u8, input, '\n');
    while (line_iter.next()) |line| {
        lines.append(allocator, line) catch unreachable;
    }

    const height = lines.items.len;
    const width = lines.items[0].len;

    var accessible_count: i32 = 0;
    var row: usize = 0;
    while (row < height) : (row += 1) {
        var col: usize = 0;
        while (col < width) : (col += 1) {
            if (lines.items[row][col] != '@') continue;

            const adjacent = countAdjacentRolls(lines.items, row, col, height, width);

            if (adjacent < 4) {
                accessible_count += 1;
            }
        }
    }

    return accessible_count;
}

fn countAdjacentRolls(grid: [][]const u8, row: usize, col: usize, height: usize, width: usize) i32 {
    var count: i32 = 0;

    for (directions) |dir| {
        const new_row = @as(i32, @intCast(row)) + dir[0];
        const new_col = @as(i32, @intCast(col)) + dir[1];

        if (new_row < 0 or new_row >= @as(i32, @intCast(height))) continue;
        if (new_col < 0 or new_col >= @as(i32, @intCast(width))) continue;

        const r = @as(usize, @intCast(new_row));
        const c = @as(usize, @intCast(new_col));

        if (grid[r][c] == '@') {
            count += 1;
        }
    }

    return count;
}

fn countAdjacentRollsMutable(grid: [][]u8, row: usize, col: usize, height: usize, width: usize) i32 {
    var count: i32 = 0;

    for (directions) |dir| {
        const new_row = @as(i32, @intCast(row)) + dir[0];
        const new_col = @as(i32, @intCast(col)) + dir[1];

        if (new_row < 0 or new_row >= @as(i32, @intCast(height))) continue;
        if (new_col < 0 or new_col >= @as(i32, @intCast(width))) continue;

        const r = @as(usize, @intCast(new_row));
        const c = @as(usize, @intCast(new_col));

        if (grid[r][c] == '@') {
            count += 1;
        }
    }

    return count;
}

pub fn part2(input: []u8) !i32 {
    const allocator = std.heap.page_allocator;
    var lines = std.ArrayListUnmanaged([]u8){};
    var line_iter = std.mem.tokenizeScalar(u8, input, '\n');
    while (line_iter.next()) |line| {
        const line_copy = allocator.dupe(u8, line) catch unreachable;
        lines.append(allocator, line_copy) catch unreachable;
    }

    const height = lines.items.len;
    const width = lines.items[0].len;

    var total_removed: i32 = 0;
    while (true) {
        var to_remove = std.ArrayListUnmanaged([2]usize){};

        var row: usize = 0;
        while (row < height) : (row += 1) {
            var col: usize = 0;
            while (col < width) : (col += 1) {
                if (lines.items[row][col] != '@') continue;

                const adjacent = countAdjacentRollsMutable(lines.items, row, col, height, width);
                if (adjacent < 4) {
                    to_remove.append(allocator, .{ row, col }) catch unreachable;
                }
            }
        }

        if (to_remove.items.len == 0) break;

        for (to_remove.items) |pos| {
            lines.items[pos[0]][pos[1]] = '.';
            total_removed += 1;
        }
    }

    return total_removed;
}

const std = @import("std");
