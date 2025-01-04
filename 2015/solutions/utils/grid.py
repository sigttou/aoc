# from ...utils import grid


def neighbors_all(x, y):
    ret = []
    for dx, dy in [
        (0, 1),
        (0, -1),
        (1, 0),
        (-1, 0),
        (1, 1),
        (-1, -1),
        (1, -1),
        (-1, 1),
    ]:
        ret.append((x + dx, y + dy))
    return ret
