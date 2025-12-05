const std = @import("std");

const Range = struct {
    start: i64,
    end: i64,
};

fn parseInput(
    input: []const u8,
    ranges: *std.array_list.Managed(Range),
    avail: *std.array_list.Managed(i64),
) !void {
    var it = std.mem.splitScalar(u8, input, '\n');
    var at_avail = false;

    while (it.next()) |line| {
        if (line.len == 0) {
            at_avail = true;
            continue;
        }

        if (!at_avail) {
            if (std.mem.indexOfScalar(u8, line, '-')) |dash| {
                const a = try std.fmt.parseInt(i64, line[0..dash], 10);
                const b = try std.fmt.parseInt(i64, line[dash + 1 ..], 10);
                try ranges.append(.{ .start = a, .end = b });
            }
        } else {
            const x = try std.fmt.parseInt(i64, line, 10);
            try avail.append(x);
        }
    }
}

pub fn part1(input: []const u8) i64 {
    const allocator = std.heap.page_allocator;
    var ranges = std.array_list.Managed(Range).init(allocator);
    var avail = std.array_list.Managed(i64).init(allocator);

    parseInput(input, &ranges, &avail) catch return 0;

    var count: i64 = 0;
    for (avail.items) |ing| {
        for (ranges.items) |rng| {
            if (ing >= rng.start and ing <= rng.end) {
                count += 1;
                break;
            }
        }
    }

    return count;
}

fn rangesOverlap(r1: Range, r2: Range) bool {
    return !(r1.end < r2.start or r2.end < r1.start);
}

pub fn part2(input: []u8) !i64 {
    const allocator = std.heap.page_allocator;
    var ranges = std.array_list.Managed(Range).init(allocator);
    var avail = std.array_list.Managed(i64).init(allocator);
    var total_ranges = std.array_list.Managed(Range).init(allocator);

    try parseInput(input, &ranges, &avail);

    for (ranges.items) |r| {
        try total_ranges.append(r);
    }

    while (true) {
        var changed = false;
        var new_ranges = std.array_list.Managed(Range).init(allocator);

        var used = std.AutoHashMap(usize, void).init(allocator);

        for (total_ranges.items, 0..) |r1, i| {
            if (used.contains(i)) continue;

            var merges = false;
            var merged_range = r1;

            for (total_ranges.items[i + 1 ..], i + 1..) |r2, j| {
                if (used.contains(j)) continue;

                if (rangesOverlap(merged_range, r2)) {
                    merged_range = Range{
                        .start = @min(merged_range.start, r2.start),
                        .end = @max(merged_range.end, r2.end),
                    };
                    try used.put(j, {});
                    merges = true;
                    changed = true;
                }
            }

            try new_ranges.append(merged_range);
        }

        if (!changed) break;

        total_ranges.clearRetainingCapacity();
        for (new_ranges.items) |r| {
            try total_ranges.append(r);
        }
    }

    var total: i64 = 0;
    for (total_ranges.items) |r| {
        total += (r.end - r.start + 1);
    }

    return total;
}
