const std = @import("std");

const Beam = struct {
    row: i32,
    col: i32,
};

pub fn part1(input: []const u8) i32 {
    const allocator = std.heap.page_allocator;
    var grid = std.array_list.Managed([]const u8).init(allocator);
    defer grid.deinit();

    var it = std.mem.tokenizeScalar(u8, input, '\n');
    while (it.next()) |line| {
        grid.append(line) catch continue;
    }

    const rows: i32 = @intCast(grid.items.len);
    const cols: i32 = @intCast(grid.items[0].len);

    var start_col: i32 = 0;
    for (grid.items[0], 0..) |c, col| {
        if (c == 'S') {
            start_col = @intCast(col);
            break;
        }
    }

    var hit_splitters = std.AutoHashMap([2]i32, void).init(allocator);
    var beams = std.array_list.Managed(Beam).init(allocator);
    beams.append(Beam{ .row = 1, .col = start_col }) catch return 0;

    var split_count: i32 = 0;
    var iterations: i32 = 0;
    while (beams.items.len > 0) {
        iterations += 1;

        var new_beams = std.array_list.Managed(Beam).init(allocator);
        defer new_beams.deinit();

        for (beams.items) |beam| {
            if (beam.row < 0 or beam.row >= rows or beam.col < 0 or beam.col >= cols) {
                continue;
            }

            const cell = grid.items[@intCast(beam.row)][@intCast(beam.col)];

            if (cell == '^') {
                const key = [2]i32{ beam.row, beam.col };
                const result = hit_splitters.getOrPut(key) catch continue;

                if (!result.found_existing) {
                    split_count += 1;

                    new_beams.append(Beam{ .row = beam.row + 1, .col = beam.col - 1 }) catch continue;
                    new_beams.append(Beam{ .row = beam.row + 1, .col = beam.col + 1 }) catch continue;
                }
            } else {
                new_beams.append(Beam{ .row = beam.row + 1, .col = beam.col }) catch continue;
            }
        }

        beams.clearRetainingCapacity();
        for (new_beams.items) |beam| {
            beams.append(beam) catch continue;
        }
    }

    return split_count;
}

pub fn part2(input: []u8) !i64 {
    const allocator = std.heap.page_allocator;
    var grid = std.array_list.Managed([]const u8).init(allocator);

    var it = std.mem.tokenizeScalar(u8, input, '\n');
    while (it.next()) |line| {
        grid.append(line) catch continue;
    }

    const rows: i32 = @intCast(grid.items.len);
    const cols: i32 = @intCast(grid.items[0].len);
    var start_col: i32 = 0;
    for (grid.items[0], 0..) |c, col| {
        if (c == 'S') {
            start_col = @intCast(col);
            break;
        }
    }

    var memo = std.AutoHashMap([2]i32, i64).init(allocator);
    return countPaths(1, start_col, rows, cols, grid.items, &memo);
}

fn countPaths(
    row: i32,
    col: i32,
    rows: i32,
    cols: i32,
    grid: []const []const u8,
    memo: *std.AutoHashMap([2]i32, i64),
) !i64 {
    if (row >= rows or col < 0 or col >= cols) {
        return 1;
    }

    if (row < 0) {
        return 0;
    }

    const key = [2]i32{ row, col };
    if (memo.get(key)) |cached| {
        return cached;
    }

    const cell = grid[@intCast(row)][@intCast(col)];
    var count: i64 = 0;
    if (cell == '^') {
        count += try countPaths(row + 1, col - 1, rows, cols, grid, memo);
        count += try countPaths(row + 1, col + 1, rows, cols, grid, memo);
    } else {
        count += try countPaths(row + 1, col, rows, cols, grid, memo);
    }

    try memo.put(key, count);
    return count;
}
