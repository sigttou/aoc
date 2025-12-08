const std = @import("std");

const Point = struct {
    x: i32,
    y: i32,
    z: i32,
};

const Edge = struct {
    from: usize,
    to: usize,
    dist_sq: i64,
};

fn edgeLessThan(_: void, a: Edge, b: Edge) bool {
    return a.dist_sq < b.dist_sq;
}

const UnionFind = struct {
    parent: []usize,
    size: []usize,
    allocator: std.mem.Allocator,

    fn init(allocator: std.mem.Allocator, n: usize) !UnionFind {
        const parent = try allocator.alloc(usize, n);
        const size = try allocator.alloc(usize, n);

        for (0..n) |i| {
            parent[i] = i;
            size[i] = 1;
        }

        return UnionFind{
            .parent = parent,
            .size = size,
            .allocator = allocator,
        };
    }

    fn deinit(self: *UnionFind) void {
        self.allocator.free(self.parent);
        self.allocator.free(self.size);
    }

    fn find(self: *UnionFind, x: usize) usize {
        if (self.parent[x] != x) {
            self.parent[x] = self.find(self.parent[x]);
        }
        return self.parent[x];
    }

    fn unite(self: *UnionFind, x: usize, y: usize) bool {
        var root_x = self.find(x);
        var root_y = self.find(y);

        if (root_x == root_y) return false;

        if (self.size[root_x] < self.size[root_y]) {
            const temp = root_x;
            root_x = root_y;
            root_y = temp;
        }

        self.parent[root_y] = root_x;
        self.size[root_x] += self.size[root_y];

        return true;
    }

    fn getComponentSizes(self: *UnionFind, allocator: std.mem.Allocator) ![]usize {
        var sizes = std.array_list.Managed(usize).init(allocator);

        var seen = std.AutoHashMap(usize, void).init(allocator);
        defer seen.deinit();

        for (0..self.parent.len) |i| {
            const root = self.find(i);
            const result = try seen.getOrPut(root);
            if (!result.found_existing) {
                try sizes.append(self.size[root]);
            }
        }

        return sizes.toOwnedSlice();
    }
};

pub fn part1(input: []const u8) i64 {
    const allocator = std.heap.page_allocator;
    var points = std.array_list.Managed(Point).init(allocator);
    defer points.deinit();

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        var coords = std.mem.splitScalar(u8, line, ',');
        const x = std.fmt.parseInt(i32, coords.next() orelse continue, 10) catch continue;
        const y = std.fmt.parseInt(i32, coords.next() orelse continue, 10) catch continue;
        const z = std.fmt.parseInt(i32, coords.next() orelse continue, 10) catch continue;

        points.append(Point{ .x = x, .y = y, .z = z }) catch continue;
    }
    const n = points.items.len;

    var edges = std.array_list.Managed(Edge).init(allocator);
    defer edges.deinit();
    for (0..n) |i| {
        for (i + 1..n) |j| {
            const p1 = points.items[i];
            const p2 = points.items[j];

            const dx: i64 = @as(i64, p1.x) - @as(i64, p2.x);
            const dy: i64 = @as(i64, p1.y) - @as(i64, p2.y);
            const dz: i64 = @as(i64, p1.z) - @as(i64, p2.z);

            const dist_sq = dx * dx + dy * dy + dz * dz;

            edges.append(Edge{ .from = i, .to = j, .dist_sq = dist_sq }) catch continue;
        }
    }

    std.mem.sort(Edge, edges.items, {}, edgeLessThan);

    var uf = UnionFind.init(allocator, n) catch return 0;
    defer uf.deinit();

    var connections_made: usize = 0;
    var edges_processed: usize = 0;
    var result_p1: i64 = 0;
    var result_p2: i64 = 0;

    for (edges.items) |edge| {
        if (edges_processed == 1000) {
            const sizes = uf.getComponentSizes(allocator) catch return 0;
            defer allocator.free(sizes);

            std.mem.sort(usize, sizes, {}, struct {
                fn desc(_: void, a: usize, b: usize) bool {
                    return a > b;
                }
            }.desc);

            if (sizes.len >= 3) {
                result_p1 = @intCast(sizes[0] * sizes[1] * sizes[2]);
            }
        }

        if (uf.unite(edge.from, edge.to)) {
            connections_made += 1;

            if (connections_made == n - 1) {
                const p_from = points.items[edge.from];
                const p_to = points.items[edge.to];
                result_p2 = @as(i64, p_from.x) * @as(i64, p_to.x);
            }
        }

        edges_processed += 1;
    }

    return result_p1;
}

pub fn part2(input: []u8) !i64 {
    const allocator = std.heap.page_allocator;

    var points = std.array_list.Managed(Point).init(allocator);
    defer points.deinit();

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        var coords = std.mem.splitScalar(u8, line, ',');
        const x = std.fmt.parseInt(i32, coords.next() orelse continue, 10) catch continue;
        const y = std.fmt.parseInt(i32, coords.next() orelse continue, 10) catch continue;
        const z = std.fmt.parseInt(i32, coords.next() orelse continue, 10) catch continue;

        points.append(Point{ .x = x, .y = y, .z = z }) catch continue;
    }

    const n = points.items.len;

    var edges = std.array_list.Managed(Edge).init(allocator);
    defer edges.deinit();
    for (0..n) |i| {
        for (i + 1..n) |j| {
            const p1 = points.items[i];
            const p2 = points.items[j];

            const dx: i64 = @as(i64, p1.x) - @as(i64, p2.x);
            const dy: i64 = @as(i64, p1.y) - @as(i64, p2.y);
            const dz: i64 = @as(i64, p1.z) - @as(i64, p2.z);

            const dist_sq = dx * dx + dy * dy + dz * dz;

            edges.append(Edge{ .from = i, .to = j, .dist_sq = dist_sq }) catch continue;
        }
    }

    std.mem.sort(Edge, edges.items, {}, edgeLessThan);

    var uf = UnionFind.init(allocator, n) catch return 0;
    defer uf.deinit();
    var connections_made: usize = 0;
    for (edges.items) |edge| {
        if (uf.unite(edge.from, edge.to)) {
            connections_made += 1;

            if (connections_made == n - 1) {
                const p_from = points.items[edge.from];
                const p_to = points.items[edge.to];
                return @as(i64, p_from.x) * @as(i64, p_to.x);
            }
        }
    }

    return 0;
}
