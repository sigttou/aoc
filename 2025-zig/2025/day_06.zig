const std = @import("std");

pub fn part1(input: []const u8) i64 {
    const allocator = std.heap.page_allocator;

    var lines = std.array_list.Managed([]const u8).init(allocator);
    defer lines.deinit();

    var it = std.mem.splitScalar(u8, input, '\n');
    while (it.next()) |line| {
        lines.append(line) catch continue;
    }

    while (lines.items.len > 0 and lines.items[lines.items.len - 1].len == 0) {
        _ = lines.pop();
    }

    var max_width: usize = 0;
    for (lines.items) |line| {
        if (line.len > 0 and line.len > max_width) max_width = line.len;
    }

    var empty_cols = std.array_list.Managed(bool).init(allocator);
    defer empty_cols.deinit();
    for (0..max_width) |col| {
        var is_empty = true;
        for (lines.items) |line| {
            if (col < line.len and line[col] != ' ') {
                is_empty = false;
                break;
            }
        }
        empty_cols.append(is_empty) catch continue;
    }

    var grand_total: i64 = 0;
    var col: usize = 0;
    while (col < max_width) {
        while (col < max_width and empty_cols.items[col]) {
            col += 1;
        }

        if (col >= max_width) break;

        const start_col = col;
        while (col < max_width and !empty_cols.items[col]) {
            col += 1;
        }

        const end_col = col;
        var numbers = std.array_list.Managed(i64).init(allocator);
        var operator: ?u8 = null;
        for (lines.items, 0..) |line, row_idx| {
            var num_str = std.array_list.Managed(u8).init(allocator);
            defer num_str.deinit();

            const actual_start = @min(start_col, line.len);
            const actual_end = @min(end_col, line.len);

            for (actual_start..actual_end) |c| {
                const ch = line[c];
                if (row_idx == lines.items.len - 1) {
                    if (ch == '+' or ch == '*') {
                        operator = ch;
                    }
                } else if (ch >= '0' and ch <= '9') {
                    num_str.append(ch) catch continue;
                }
            }

            if (num_str.items.len > 0 and row_idx < lines.items.len - 1) {
                const num = std.fmt.parseInt(i64, num_str.items, 10) catch continue;
                numbers.append(num) catch continue;
            }
        }

        if (numbers.items.len > 0 and operator != null) {
            var result = numbers.items[0];

            for (numbers.items[1..]) |num| {
                if (operator.? == '+') {
                    result += num;
                } else if (operator.? == '*') {
                    result *= num;
                }
            }

            grand_total += result;
        }
    }

    return grand_total;
}

pub fn part2(input: []u8) !i64 {
    const allocator = std.heap.page_allocator;
    var lines = std.array_list.Managed([]const u8).init(allocator);
    var it = std.mem.splitScalar(u8, input, '\n');
    while (it.next()) |line| {
        const trimmed = std.mem.trimRight(u8, line, "\r");
        lines.append(trimmed) catch continue;
    }

    while (lines.items.len > 0 and lines.items[lines.items.len - 1].len == 0) {
        _ = lines.pop();
    }

    var max_width: usize = 0;
    for (lines.items) |line| {
        if (line.len > max_width) max_width = line.len;
    }

    var p2: i64 = 0;
    var start_c: usize = 0;

    var cc: usize = 0;
    while (cc <= max_width) : (cc += 1) {
        var is_blank = true;
        if (cc < max_width) {
            for (lines.items) |line| {
                if (cc < line.len and line[cc] != ' ') {
                    is_blank = false;
                    break;
                }
            }
        }

        if (is_blank) {
            if (start_c < cc) {
                const op = lines.items[lines.items.len - 1][start_c];
                var score: i64 = if (op == '+') 0 else 1;
                var c = cc;
                while (c > start_c) {
                    c -= 1;
                    var n: i64 = 0;
                    for (lines.items[0 .. lines.items.len - 1]) |line| {
                        if (c < line.len and line[c] != ' ') {
                            const digit = line[c] - '0';
                            n = n * 10 + digit;
                        }
                    }

                    if (op == '+') {
                        score += n;
                    } else {
                        score *= n;
                    }
                }

                p2 += score;
            }

            start_c = cc + 1;
        }
    }

    return p2;
}
