const std = @import("std");

pub fn part1(input: []const u8) i64 {
    const allocator = std.heap.page_allocator;

    const graph = parseGraph(input, allocator) catch return 0;
    defer freeGraph(graph, allocator);

    var check_nodes = std.StringHashMap(void).init(allocator);
    defer check_nodes.deinit();

    const result = countPaths(graph, "you", "out", &check_nodes, allocator) catch 0;
    return result;
}

pub fn part2(input: []u8) !i64 {
    const allocator = std.heap.page_allocator;

    const graph = try parseGraph(input, allocator);
    defer freeGraph(graph, allocator);

    var check_nodes = std.StringHashMap(void).init(allocator);
    defer check_nodes.deinit();

    try check_nodes.put("fft", {});
    try check_nodes.put("dac", {});

    const result = try countPaths(graph, "svr", "out", &check_nodes, allocator);
    return result;
}

fn parseGraph(input: []const u8, allocator: std.mem.Allocator) !std.StringHashMap(std.array_list.Managed([]const u8)) {
    var graph = std.StringHashMap(std.array_list.Managed([]const u8)).init(allocator);

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        const colon_pos = std.mem.indexOf(u8, line, ":") orelse continue;
        const source = line[0..colon_pos];
        const dests_str = std.mem.trim(u8, line[colon_pos + 1 ..], " ");

        var dests = std.array_list.Managed([]const u8).init(allocator);

        if (dests_str.len > 0) {
            var dest_it = std.mem.tokenizeScalar(u8, dests_str, ' ');
            while (dest_it.next()) |dest| {
                try dests.append(dest);
            }
        }

        try graph.put(source, dests);
    }

    return graph;
}

fn freeGraph(graph: std.StringHashMap(std.array_list.Managed([]const u8)), allocator: std.mem.Allocator) void {
    _ = allocator;
    var it = graph.iterator();
    while (it.next()) |entry| {
        entry.value_ptr.deinit();
    }
    var g = graph;
    g.deinit();
}

fn countPaths(
    graph: std.StringHashMap(std.array_list.Managed([]const u8)),
    start: []const u8,
    end: []const u8,
    check_nodes: *std.StringHashMap(void),
    allocator: std.mem.Allocator,
) !i64 {
    var memo = std.AutoHashMap(u64, i64).init(allocator);
    defer memo.deinit();

    var visited_check_start = std.StringHashMap(void).init(allocator);
    defer visited_check_start.deinit();

    if (check_nodes.contains(start)) {
        try visited_check_start.put(start, {});
    }

    var visited_set = std.StringHashMap(void).init(allocator);
    defer visited_set.deinit();
    try visited_set.put(start, {});

    return try dfs(
        graph,
        start,
        end,
        check_nodes,
        &visited_check_start,
        &visited_set,
        &memo,
        allocator,
    );
}

fn dfs(
    graph: std.StringHashMap(std.array_list.Managed([]const u8)),
    node: []const u8,
    end: []const u8,
    check_nodes: *std.StringHashMap(void),
    visited_check: *std.StringHashMap(void),
    visited_set: *std.StringHashMap(void),
    memo: *std.AutoHashMap(u64, i64),
    allocator: std.mem.Allocator,
) !i64 {
    if (std.mem.eql(u8, node, end)) {
        if (check_nodes.count() == 0) return 1;

        var it = check_nodes.keyIterator();
        while (it.next()) |required| {
            if (!visited_check.contains(required.*)) {
                return 0;
            }
        }
        return 1;
    }

    const key = computeMemoKey(node, visited_check);

    if (memo.get(key)) |cached| {
        return cached;
    }

    var total_paths: i64 = 0;

    if (graph.get(node)) |neighbors| {
        for (neighbors.items) |neighbor| {
            if (visited_set.contains(neighbor)) continue;

            var new_visited_check = try cloneStringHashMap(visited_check, allocator);
            defer new_visited_check.deinit();

            if (check_nodes.contains(neighbor)) {
                try new_visited_check.put(neighbor, {});
            }

            try visited_set.put(neighbor, {});
            defer _ = visited_set.remove(neighbor);

            const paths = try dfs(
                graph,
                neighbor,
                end,
                check_nodes,
                &new_visited_check,
                visited_set,
                memo,
                allocator,
            );

            total_paths += paths;
        }
    }

    try memo.put(key, total_paths);
    return total_paths;
}

fn computeMemoKey(node: []const u8, visited_check: *std.StringHashMap(void)) u64 {
    var h = std.hash.Wyhash.init(0);
    h.update(node);

    var keys = std.array_list.Managed([]const u8).init(std.heap.page_allocator);
    defer keys.deinit();

    var it = visited_check.keyIterator();
    while (it.next()) |key| {
        keys.append(key.*) catch {};
    }

    std.mem.sort([]const u8, keys.items, {}, struct {
        fn lessThan(_: void, a: []const u8, b: []const u8) bool {
            return std.mem.order(u8, a, b) == .lt;
        }
    }.lessThan);

    for (keys.items) |key| {
        h.update(key);
    }

    return h.final();
}

fn cloneStringHashMap(original: *std.StringHashMap(void), allocator: std.mem.Allocator) !std.StringHashMap(void) {
    var clone = std.StringHashMap(void).init(allocator);

    var it = original.keyIterator();
    while (it.next()) |key| {
        try clone.put(key.*, {});
    }

    return clone;
}
