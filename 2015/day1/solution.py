"""
--- Day 1: Not Quite Lisp ---

Santa is trying to deliver presents in a large apartment building, but he
can't find the right floor - the directions he got are a little confusing.
He starts on the ground floor (floor 0) and then follows the instructions
one character at a time.

An opening parenthesis, (, means he should go up one floor, and a closing
parenthesis, ), means he should go down one floor.

The apartment building is very tall, and the basement is very deep; he will
never find the top or bottom floors.

For example:

    (()) and ()() both result in floor 0.
    ((( and (()(()( both result in floor 3.
    ))((((( also results in floor 3.
    ()) and ))( both result in floor -1 (the first basement level).
    ))) and )())()) both result in floor -3.

To what floor do the instructions take Santa?


Your puzzle answer was 232.


--- Part Two ---

Now, given the same instructions, find the position of the first character
that causes him to enter the basement (floor -1). The first character in
the instructions has position 1, the second character has position 2, and
so on.

For example:

    ) causes him to enter the basement at character position 1.
    ()()) causes him to enter the basement at character position 5.

What is the position of the character that causes Santa to first enter the
basement?


Your puzzle answer was 1783.
"""

import os


def read_input() -> str:
    floors: str = None

    input_file_path: str = os.path.join(os.getcwd(), "2020\\day1\\input.txt")
    with open(input_file_path, "r") as input_file:
        floors = input_file.readline().strip()

    return floors


def calculate_ending_floor(floors: [str]) -> int:
    floor: int = 0
    for char in floors:
        if char == "(":
            floor += 1
        elif char == ")":
            floor -= 1

    return floor


def calculate_first_basement_visit(floors: [str]) -> int:
    floor: int = 0
    for i, char in enumerate(floors):
        if char == "(":
            floor += 1
        elif char == ")":
            floor -= 1

        if floor == -1:
            return i + 1

    return -1


floors = read_input()
print(calculate_ending_floor(floors))
print(calculate_first_basement_visit(floors))
