const std = @import("std");

const INF = std.math.inf(f64);
const EPS = 1e-9;

pub fn part1(input: []const u8) i64 {
    const allocator = std.heap.page_allocator;

    var total_presses: i64 = 0;

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        const presses = solveMachine(line, allocator) catch continue;
        total_presses += presses;
    }

    return total_presses;
}

fn solveMachine(line: []const u8, allocator: std.mem.Allocator) !i64 {
    const bracket_start = std.mem.indexOf(u8, line, "[") orelse return error.InvalidFormat;
    const bracket_end = std.mem.indexOf(u8, line[bracket_start..], "]") orelse return error.InvalidFormat;
    const target_str = line[bracket_start + 1 .. bracket_start + bracket_end];

    var target = std.array_list.Managed(u8).init(allocator);

    for (target_str) |c| {
        if (c == '#') {
            try target.append(1);
        } else if (c == '.') {
            try target.append(0);
        }
    }

    const num_lights = target.items.len;
    var buttons = std.array_list.Managed(std.array_list.Managed(usize)).init(allocator);
    defer {
        for (buttons.items) |*btn| {
            btn.deinit();
        }
        buttons.deinit();
    }

    var i = bracket_start + bracket_end + 1;
    while (i < line.len) {
        if (line[i] == '(') {
            const paren_end = std.mem.indexOfScalarPos(u8, line, i, ')') orelse break;
            const button_str = line[i + 1 .. paren_end];

            var button = std.array_list.Managed(usize).init(allocator);
            var nums = std.mem.tokenizeScalar(u8, button_str, ',');
            while (nums.next()) |num_str| {
                const num = std.fmt.parseInt(usize, num_str, 10) catch continue;
                try button.append(num);
            }

            try buttons.append(button);
            i = paren_end + 1;
        } else {
            i += 1;
        }
    }

    const max_state: usize = @as(usize, 1) << @intCast(num_lights);
    var distances = try allocator.alloc(i32, max_state);
    defer allocator.free(distances);
    @memset(distances, -1);
    distances[0] = 0;

    var queue = std.array_list.Managed(usize).init(allocator);
    defer queue.deinit();
    try queue.append(0);

    var button_masks = std.array_list.Managed(usize).init(allocator);
    defer button_masks.deinit();

    for (buttons.items) |button| {
        var mask: usize = 0;
        for (button.items) |bit| {
            if (bit < num_lights) {
                mask |= @as(usize, 1) << @intCast(bit);
            }
        }
        try button_masks.append(mask);
    }

    var target_mask: usize = 0;
    for (target.items, 0..) |bit, idx| {
        if (bit == 1) {
            target_mask |= @as(usize, 1) << @intCast(idx);
        }
    }

    var queue_idx: usize = 0;
    while (queue_idx < queue.items.len) : (queue_idx += 1) {
        const state = queue.items[queue_idx];

        for (button_masks.items) |mask| {
            const next_state = state ^ mask;
            if (distances[next_state] == -1) {
                distances[next_state] = distances[state] + 1;
                try queue.append(next_state);
            }
        }
    }

    if (distances[target_mask] == -1) {
        return error.NoSolution;
    }

    return distances[target_mask];
}

pub fn part2(input: []u8) !i64 {
    const allocator = std.heap.page_allocator;

    var total: i64 = 0;
    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        const result = try solveMachine2Gauss(line, allocator);
        total += result;
    }

    return total;
}

fn solveMachine2Gauss(line: []const u8, allocator: std.mem.Allocator) !i64 {
    var buttons = std.array_list.Managed(std.array_list.Managed(usize)).init(allocator);

    var i: usize = 0;
    while (i < line.len) {
        if (line[i] == '(') {
            const paren_end = std.mem.indexOfScalarPos(u8, line, i, ')') orelse break;
            const button_str = line[i + 1 .. paren_end];

            var button = std.array_list.Managed(usize).init(allocator);

            var nums = std.mem.tokenizeScalar(u8, button_str, ',');
            while (nums.next()) |num_str| {
                const num = std.fmt.parseInt(usize, num_str, 10) catch continue;
                try button.append(num);
            }

            try buttons.append(button);
            i = paren_end + 1;
        } else {
            i += 1;
        }
    }

    const brace_start = std.mem.indexOf(u8, line, "{") orelse return error.InvalidFormat;
    const brace_end = std.mem.indexOfScalarPos(u8, line, brace_start, '}') orelse return error.InvalidFormat;
    const target_str = line[brace_start + 1 .. brace_end];

    var joltages = std.array_list.Managed(i64).init(allocator);
    defer joltages.deinit();

    var target_nums = std.mem.tokenizeScalar(u8, target_str, ',');
    while (target_nums.next()) |num_str| {
        const num = try std.fmt.parseInt(i64, num_str, 10);
        try joltages.append(num);
    }

    const n = buttons.items.len;
    const m = joltages.items.len;

    var matrix = try allocator.alloc([]i64, m);

    for (0..m) |j| {
        matrix[j] = try allocator.alloc(i64, n + 1);
        for (0..n) |btn_idx| {
            var has_j = false;
            for (buttons.items[btn_idx].items) |counter| {
                if (counter == j) {
                    has_j = true;
                    break;
                }
            }
            matrix[j][btn_idx] = if (has_j) 1 else 0;
        }
        matrix[j][n] = joltages.items[j];
    }

    var pivot_cols = std.array_list.Managed(usize).init(allocator);
    defer pivot_cols.deinit();

    var current_row: usize = 0;

    for (0..n) |col| {
        var pivot_row: ?usize = null;
        for (current_row..m) |row| {
            if (matrix[row][col] != 0) {
                pivot_row = row;
                break;
            }
        }

        if (pivot_row == null) continue;

        if (pivot_row.? != current_row) {
            const temp = matrix[current_row];
            matrix[current_row] = matrix[pivot_row.?];
            matrix[pivot_row.?] = temp;
        }

        try pivot_cols.append(col);

        for (current_row + 1..m) |row| {
            if (matrix[row][col] != 0) {
                const factor = matrix[row][col];
                const pivot = matrix[current_row][col];

                for (0..n + 1) |j| {
                    matrix[row][j] = matrix[row][j] * pivot - matrix[current_row][j] * factor;
                }
            }
        }

        current_row += 1;
    }

    var pivot_set = std.AutoHashMap(usize, void).init(allocator);
    defer pivot_set.deinit();

    for (pivot_cols.items) |col| {
        try pivot_set.put(col, {});
    }

    var free_vars = std.array_list.Managed(usize).init(allocator);

    for (0..n) |idx| {
        if (!pivot_set.contains(idx)) {
            try free_vars.append(idx);
        }
    }

    var best_sum: i64 = std.math.maxInt(i64);
    const max_val: i64 = if (joltages.items.len > 0) blk: {
        var max_j: i64 = 0;
        for (joltages.items) |j| {
            if (j > max_j) max_j = j;
        }
        break :blk max_j * 3;
    } else 100;

    const search_limit: i64 = @min(max_val, 200);

    try searchSolutions(
        matrix,
        pivot_cols.items,
        free_vars.items,
        buttons.items,
        joltages.items,
        n,
        m,
        search_limit,
        &best_sum,
        allocator,
    );

    if (best_sum == std.math.maxInt(i64)) {
        return error.NoSolution;
    }

    return best_sum;
}

fn searchSolutions(
    matrix: [][]i64,
    pivot_cols: []usize,
    free_vars: []usize,
    buttons: []std.array_list.Managed(usize),
    joltages: []i64,
    n: usize,
    m: usize,
    search_limit: i64,
    best_sum: *i64,
    allocator: std.mem.Allocator,
) !void {
    if (free_vars.len == 0) {
        try trySolution(matrix, pivot_cols, &[_]i64{}, free_vars, buttons, joltages, n, m, best_sum, allocator);
    } else if (free_vars.len == 1) {
        var val: i64 = 0;
        while (val <= search_limit) : (val += 1) {
            if (val > best_sum.*) break;
            try trySolution(matrix, pivot_cols, &[_]i64{val}, free_vars, buttons, joltages, n, m, best_sum, allocator);
        }
    } else if (free_vars.len == 2) {
        var v0: i64 = 0;
        while (v0 <= search_limit) : (v0 += 1) {
            var v1: i64 = 0;
            while (v1 <= search_limit) : (v1 += 1) {
                if (v0 + v1 > best_sum.*) break;
                try trySolution(matrix, pivot_cols, &[_]i64{ v0, v1 }, free_vars, buttons, joltages, n, m, best_sum, allocator);
            }
        }
    } else {
        const limit = @min(search_limit, 30);
        const combo = try allocator.alloc(i64, free_vars.len);
        defer allocator.free(combo);
        try searchRecursive(0, combo, limit, matrix, pivot_cols, free_vars, buttons, joltages, n, m, best_sum, allocator);
    }
}

fn searchRecursive(
    idx: usize,
    combo: []i64,
    limit: i64,
    matrix: [][]i64,
    pivot_cols: []usize,
    free_vars: []usize,
    buttons: []std.array_list.Managed(usize),
    joltages: []i64,
    n: usize,
    m: usize,
    best_sum: *i64,
    allocator: std.mem.Allocator,
) !void {
    if (idx == combo.len) {
        var sum: i64 = 0;
        for (combo) |c| sum += c;
        if (sum >= best_sum.*) return;

        try trySolution(matrix, pivot_cols, combo, free_vars, buttons, joltages, n, m, best_sum, allocator);
        return;
    }

    var val: i64 = 0;
    while (val <= limit) : (val += 1) {
        combo[idx] = val;
        try searchRecursive(idx + 1, combo, limit, matrix, pivot_cols, free_vars, buttons, joltages, n, m, best_sum, allocator);
    }
}

fn trySolution(
    matrix: [][]i64,
    pivot_cols: []usize,
    free_values: []const i64,
    free_vars: []usize,
    buttons: []std.array_list.Managed(usize),
    joltages: []i64,
    n: usize,
    m: usize,
    best_sum: *i64,
    allocator: std.mem.Allocator,
) !void {
    var solution = try allocator.alloc(i64, n);
    defer allocator.free(solution);
    @memset(solution, 0);

    for (free_vars, 0..) |var_idx, i| {
        solution[var_idx] = free_values[i];
    }

    var row_idx: usize = pivot_cols.len;
    while (row_idx > 0) {
        row_idx -= 1;

        const col = pivot_cols[row_idx];
        var val = matrix[row_idx][n];

        for (col + 1..n) |j| {
            val -= matrix[row_idx][j] * solution[j];
        }

        if (matrix[row_idx][col] == 0) return;

        if (@rem(val, matrix[row_idx][col]) != 0) return;

        solution[col] = @divTrunc(val, matrix[row_idx][col]);

        if (solution[col] < 0) return;
    }

    for (0..m) |j| {
        var total: i64 = 0;
        for (0..n) |btn_idx| {
            for (buttons[btn_idx].items) |counter| {
                if (counter == j) {
                    total += solution[btn_idx];
                    break;
                }
            }
        }
        if (total != joltages[j]) return;
    }

    var total_sum: i64 = 0;
    for (solution) |s| {
        total_sum += s;
    }

    if (total_sum < best_sum.*) {
        best_sum.* = total_sum;
    }
}
