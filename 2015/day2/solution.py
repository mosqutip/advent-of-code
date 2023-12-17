"""
--- Day 2: I Was Told There Would Be No Math ---

The elves are running low on wrapping paper, and so they need to submit an
order for more. They have a list of the dimensions (length l, width w, and
height h) of each present, and only want to order exactly as much as they
need.

Fortunately, every present is a box (a perfect right rectangular prism),
which makes calculating the required wrapping paper for each gift a little
easier: find the surface area of the box, which is 2*l*w + 2*w*h + 2*h*l.
The elves also need a little extra paper for each present: the area of the
smallest side.

For example:

   - A present with dimensions 2x3x4 requires 2*6 + 2*12 + 2*8 = 52 square feet of
     wrapping paper plus 6 square feet of slack, for a total of 58 square feet.
   - A present with dimensions 1x1x10 requires 2*1 + 2*10 + 2*10 = 42 square feet
     of wrapping paper plus 1 square foot of slack, for a total of 43 square feet.

All numbers in the elves' list are in feet. How many total square feet of
wrapping paper should they order?
"""

import os


def read_input() -> str:
    present_dimensions: [[int]] = []

    input_file_path: str = os.path.join(os.getcwd(), "2020\\day2\\input.txt")
    with open(input_file_path, "r") as input_file:
        for line in input_file.readlines():
            line = line.strip()
            present_dimensions.append([int(dimension) for dimension in line.split("x")])

    return present_dimensions


def calculate_total_paper_needed(present_dimensions: [[int]]) -> int:
    total_paper: int = 0
    for dimensions in present_dimensions:
        largest_dimension: int = max(dimensions)
        slack: int = (
            dimensions[0] * dimensions[1] * dimensions[2]
        ) // largest_dimension
        surface_area: int = (
            (2 * dimensions[0] * dimensions[1])
            + (2 * dimensions[1] * dimensions[2])
            + (2 * dimensions[0] * dimensions[2])
        )

        total_paper += slack + surface_area

    return total_paper


def calculate_total_ribbon(present_dimensions: [[int]]) -> int:
    total_ribbon: int = 0
    for dimensions in present_dimensions:
        largest_dimension: int = max(dimensions)
        smallest_face: int = (
            (dimensions[0] * 2) + (dimensions[1] * 2) + (dimensions[2] * 2)
        ) - (largest_dimension * 2)
        volume: int = dimensions[0] * dimensions[1] * dimensions[2]

        total_ribbon += smallest_face + volume

    return total_ribbon


present_dimensions = read_input()
print(calculate_total_paper_needed(present_dimensions))
print(calculate_total_ribbon(present_dimensions))
