const std = @import("std");

pub fn part1(input: []const u8) i64 {
    const allocator = std.heap.page_allocator;

    var box_w: ?i32 = null;
    var box_h: ?i32 = null;

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    var current_shape_lines = std.array_list.Managed([]const u8).init(allocator);
    defer current_shape_lines.deinit();

    var parsing_shapes = true;

    while (lines.next()) |line| {
        if (line.len == 0) {
            if (current_shape_lines.items.len > 0) {
                const width: i32 = @intCast(getMaxWidth(current_shape_lines.items));
                const height: i32 = @intCast(current_shape_lines.items.len);

                if (box_w == null) {
                    box_w = width;
                    box_h = height;
                }

                current_shape_lines.clearRetainingCapacity();
            }
            continue;
        }

        if (line.len >= 2 and line[line.len - 1] == ':') {
            var is_digit = true;
            for (line[0 .. line.len - 1]) |c| {
                if (c < '0' or c > '9') {
                    is_digit = false;
                    break;
                }
            }
            if (is_digit) {
                if (current_shape_lines.items.len > 0) {
                    const width: i32 = @intCast(getMaxWidth(current_shape_lines.items));
                    const height: i32 = @intCast(current_shape_lines.items.len);

                    if (box_w == null) {
                        box_w = width;
                        box_h = height;
                    }

                    current_shape_lines.clearRetainingCapacity();
                }
                parsing_shapes = true;
                continue;
            }
        }

        if (std.mem.indexOf(u8, line, "x") != null and std.mem.indexOf(u8, line, ": ") != null) {
            parsing_shapes = false;

            if (current_shape_lines.items.len > 0) {
                const width: i32 = @intCast(getMaxWidth(current_shape_lines.items));
                const height: i32 = @intCast(current_shape_lines.items.len);

                if (box_w == null) {
                    box_w = width;
                    box_h = height;
                }

                current_shape_lines.clearRetainingCapacity();
            }
        }

        if (parsing_shapes and isShapeLine(line)) {
            current_shape_lines.append(line) catch continue;
        }
    }

    const bw = box_w.?;
    const bh = box_h.?;

    var count_ok: i64 = 0;
    var lines2 = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines2.next()) |line| {
        const colon_space = std.mem.indexOf(u8, line, ": ") orelse continue;
        const x_pos = std.mem.indexOf(u8, line, "x") orelse continue;
        if (x_pos > colon_space) continue;

        const W = std.fmt.parseInt(i32, line[0..x_pos], 10) catch continue;
        const H = std.fmt.parseInt(i32, line[x_pos + 1 .. colon_space], 10) catch continue;

        const counts_str = std.mem.trim(u8, line[colon_space + 2 ..], " ");

        var total_presents: i32 = 0;
        var nums = std.mem.tokenizeScalar(u8, counts_str, ' ');

        while (nums.next()) |num_str| {
            const count_val = std.fmt.parseInt(i32, num_str, 10) catch 0;
            total_presents += count_val;
        }

        const capacity = @divFloor(W, bw) * @divFloor(H, bh);

        if (total_presents <= capacity) {
            count_ok += 1;
        }
    }

    return count_ok;
}

fn getMaxWidth(lines: [][]const u8) usize {
    var max: usize = 0;
    for (lines) |line| {
        if (line.len > max) max = line.len;
    }
    return max;
}

fn isShapeLine(line: []const u8) bool {
    for (line) |c| {
        if (c != '.' and c != '#') return false;
    }
    return true;
}

pub fn part2(input: []u8) !i32 {
    std.debug.print("Press the button.\n", .{});
    _ = input;
    return 0;
}
