const std = @import("std");

const Point = struct {
    x: i64,
    y: i64,
};

pub fn part1(input: []const u8) i64 {
    const allocator = std.heap.page_allocator;

    var tiles = std.array_list.Managed(Point).init(allocator);
    defer tiles.deinit();

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        var coords = std.mem.tokenizeScalar(u8, line, ',');
        const x = std.fmt.parseInt(i64, coords.next() orelse continue, 10) catch continue;
        const y = std.fmt.parseInt(i64, coords.next() orelse continue, 10) catch continue;

        tiles.append(Point{ .x = x, .y = y }) catch continue;
    }

    var max_area: i64 = 0;

    for (tiles.items, 0..) |tile1, i| {
        for (tiles.items[i + 1 ..]) |tile2| {
            const min_x = @min(tile1.x, tile2.x);
            const max_x = @max(tile1.x, tile2.x);
            const min_y = @min(tile1.y, tile2.y);
            const max_y = @max(tile1.y, tile2.y);

            const width = max_x - min_x + 1;
            const height = max_y - min_y + 1;
            const area = width * height;

            if (area > max_area) {
                max_area = area;
            }
        }
    }

    return max_area;
}

pub fn part2(input: []u8) !i64 {
    const allocator = std.heap.page_allocator;

    var tiles = std.array_list.Managed(Point).init(allocator);
    defer tiles.deinit();

    var lines = std.mem.tokenizeScalar(u8, input, '\n');
    while (lines.next()) |line| {
        var coords = std.mem.tokenizeScalar(u8, line, ',');
        const x = std.fmt.parseInt(i64, coords.next() orelse continue, 10) catch continue;
        const y = std.fmt.parseInt(i64, coords.next() orelse continue, 10) catch continue;

        tiles.append(Point{ .x = x, .y = y }) catch continue;
    }

    var max_area: i64 = 0;
    for (tiles.items, 0..) |tile1, i| {
        for (tiles.items[i + 1 ..]) |tile2| {
            const x1 = @min(tile1.x, tile2.x);
            const x2 = @max(tile1.x, tile2.x);
            const y1 = @min(tile1.y, tile2.y);
            const y2 = @max(tile1.y, tile2.y);

            if (rectInPoly(x1, x2, y1, y2, tiles.items)) {
                const width = x2 - x1 + 1;
                const height = y2 - y1 + 1;
                const area = width * height;

                if (area > max_area) {
                    max_area = area;
                }
            }
        }
    }

    return max_area;
}

fn rectInPoly(x1: i64, x2: i64, y1: i64, y2: i64, poly: []const Point) bool {
    const corners = [_]Point{
        Point{ .x = x1, .y = y1 },
        Point{ .x = x1, .y = y2 },
        Point{ .x = x2, .y = y1 },
        Point{ .x = x2, .y = y2 },
    };

    for (corners) |corner| {
        if (!isPointInPoly(corner.x, corner.y, poly)) {
            return false;
        }
    }

    const rect_edges = [_][2]Point{
        [_]Point{ Point{ .x = x1, .y = y1 }, Point{ .x = x2, .y = y1 } },
        [_]Point{ Point{ .x = x2, .y = y1 }, Point{ .x = x2, .y = y2 } },
        [_]Point{ Point{ .x = x2, .y = y2 }, Point{ .x = x1, .y = y2 } },
        [_]Point{ Point{ .x = x1, .y = y2 }, Point{ .x = x1, .y = y1 } },
    };

    const n = poly.len;
    for (rect_edges) |rect_edge| {
        for (0..n) |i| {
            const poly_edge = [_]Point{
                poly[i],
                poly[(i + 1) % n],
            };

            if (segmentsIntersect(rect_edge[0], rect_edge[1], poly_edge[0], poly_edge[1])) {
                return false;
            }
        }
    }

    return true;
}

fn isPointInPoly(px: i64, py: i64, poly: []const Point) bool {
    const n = poly.len;
    for (0..n) |i| {
        const p1 = poly[i];
        const p2 = poly[(i + 1) % n];

        if (isPointOnSegment(px, py, p1.x, p1.y, p2.x, p2.y)) {
            return true;
        }
    }

    var inside = false;
    var j: usize = n - 1;
    for (poly, 0..) |pi, i| {
        const pj = poly[j];

        if ((pi.y > py) != (pj.y > py)) {
            const slope = @as(f64, @floatFromInt(pj.x - pi.x)) / @as(f64, @floatFromInt(pj.y - pi.y));
            const intersect_x = @as(f64, @floatFromInt(pi.x)) + slope * @as(f64, @floatFromInt(py - pi.y));

            if (@as(f64, @floatFromInt(px)) < intersect_x) {
                inside = !inside;
            }
        }

        j = i;
    }

    return inside;
}

fn isPointOnSegment(px: i64, py: i64, x1: i64, y1: i64, x2: i64, y2: i64) bool {
    const cross = (x2 - x1) * (py - y1) - (y2 - y1) * (px - x1);
    if (cross != 0) return false;

    if (px < @min(x1, x2) or px > @max(x1, x2)) return false;
    if (py < @min(y1, y2) or py > @max(y1, y2)) return false;

    return true;
}

fn getOrientation(ax: i64, ay: i64, bx: i64, by: i64, cx: i64, cy: i64) i64 {
    const v = (bx - ax) * (cy - ay) - (by - ay) * (cx - ax);
    if (v > 0) return 1;
    if (v < 0) return -1;
    return 0;
}

fn segmentsIntersect(a1: Point, a2: Point, b1: Point, b2: Point) bool {
    const o1 = getOrientation(a1.x, a1.y, a2.x, a2.y, b1.x, b1.y);
    const o2 = getOrientation(a1.x, a1.y, a2.x, a2.y, b2.x, b2.y);
    const o3 = getOrientation(b1.x, b1.y, b2.x, b2.y, a1.x, a1.y);
    const o4 = getOrientation(b1.x, b1.y, b2.x, b2.y, a2.x, a2.y);

    return o1 * o2 < 0 and o3 * o4 < 0;
}
