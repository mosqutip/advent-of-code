"""
--- Day 3: Perfectly Spherical Houses in a Vacuum ---

Santa is delivering presents to an infinite two-dimensional grid of houses.

He begins by delivering a present to the house at his starting location, and
then an elf at the North Pole calls him via radio and tells him where to move
next. Moves are always exactly one house to the north (^), south (v), east
(>), or west (<). After each move, he delivers another present to the house
at his new location.

However, the elf back at the north pole has had a little too much eggnog, and
so his directions are a little off, and Santa ends up visiting some houses
more than once. How many houses receive at least one present?

For example:

    > delivers presents to 2 houses: one at the starting location, and one to the east.
    ^>v< delivers presents to 4 houses in a square, including twice to the house at his starting/ending location.
    ^v^v^v^v^v delivers a bunch of presents to some very lucky children at only 2 houses.

Your puzzle answer was 2081.

--- Part Two ---

The next year, to speed up the process, Santa creates a robot version of himself, Robo-Santa, to deliver presents with him.

Santa and Robo-Santa start at the same location (delivering two presents to the same starting house), then take turns moving based on instructions from the elf, who is eggnoggedly reading from the same script as the previous year.

This year, how many houses receive at least one present?

For example:

    ^v delivers presents to 3 houses, because Santa goes north, and then Robo-Santa goes south.
    ^>v< now delivers presents to 3 houses, and Santa and Robo-Santa end up back where they started.
    ^v^v^v^v^v now delivers presents to 11 houses, with Santa going one direction and Robo-Santa going the other.

Your puzzle answer was 2341.
"""

import os


def read_input() -> str:
    directions: [str] = []

    input_file_path: str = os.path.join(os.getcwd(), "2020\\day3\\input.txt")
    with open(input_file_path, "r") as input_file:
        directions = list(input_file.readline().strip())

    return directions


def deliver_presents(directions: [str]) -> {(int, int)}:
    x_coord: int = 0
    y_coord: int = 0
    houses: {(int, int)} = set((0, 0))

    for direction in directions:
        if direction == "^":
            y_coord += 1
        elif direction == ">":
            x_coord += 1
        elif direction == "v":
            y_coord -= 1
        elif direction == "<":
            x_coord -= 1

        if (x_coord, y_coord) not in houses:
            houses.add((x_coord, y_coord))

    return houses


def deliver_presents_with_robot(directions: [str]) -> {(int, int)}:
    santa_x_coord: int = 0
    santa_y_coord: int = 0
    robot_x_coord: int = 0
    robot_y_coord: int = 0
    is_real_santa: bool = True
    houses: {(int, int)} = set({(0, 0))

    for direction in directions:
        if is_real_santa:
            x_coord = santa_x_coord
            y_coord = santa_y_coord
        else:
            x_coord = robot_x_coord
            y_coord = robot_y_coord

        if direction == "^":
            y_coord += 1
        elif direction == ">":
            x_coord += 1
        elif direction == "v":
            y_coord -= 1
        elif direction == "<":
            x_coord -= 1

        if (x_coord, y_coord) not in houses:
            houses.add((x_coord, y_coord))

        if is_real_santa:
            santa_x_coord = x_coord
            santa_y_coord = y_coord
        else:
            robot_x_coord = x_coord
            robot_y_coord = y_coord

        is_real_santa = not is_real_santa

    return houses


def count_houses_with_presents(houses: {(int, int)}) -> int:
    return len(houses)


directions: [str] = read_input()
houses: {(int, int)} = deliver_presents(directions)
print(count_houses_with_presents(houses))

houses_with_robot: {(int, int)} = deliver_presents_with_robot(directions)
print(count_houses_with_presents(houses_with_robot))
