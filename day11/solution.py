"""
--- Day 11: Seating System ---
Your plane lands with plenty of time to spare. The final leg of your journey is a ferry that goes directly to the tropical island where you can finally start your vacation. As you reach the waiting area to board the ferry, you realize you're so early, nobody else has even arrived yet!

By modeling the process people use to choose (or abandon) their seat in the waiting area, you're pretty sure you can predict the best place to sit. You make a quick map of the seat layout (your puzzle input).

The seat layout fits neatly on a grid. Each position is either floor (.), an empty seat (L), or an occupied seat (#). For example, the initial seat layout might look like this:

L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL

Now, you just need to model the people who will be arriving shortly. Fortunately, people are entirely predictable and always follow a simple set of rules. All decisions are based on the number of occupied seats adjacent to a given seat (one of the eight positions immediately up, down, left, right, or diagonal from the seat). The following rules are applied to every seat simultaneously:

    If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
    If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
    Otherwise, the seat's state does not change.

Floor (.) never changes; seats don't move, and nobody sits on the floor.

After one round of these rules, every seat in the example layout becomes occupied:

#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##

After a second round, the seats with four or more occupied adjacent seats become empty again:

#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##

This process continues for three more rounds:

#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##

#.#L.L#.##
#LLL#LL.L#
L.L.L..#..
#LLL.##.L#
#.LL.LL.LL
#.LL#L#.##
..L.L.....
#L#LLLL#L#
#.LLLLLL.L
#.#L#L#.##

#.#L.L#.##
#LLL#LL.L#
L.#.L..#..
#L##.##.L#
#.#L.LL.LL
#.#L#L#.##
..L.L.....
#L#L##L#L#
#.LLLLLL.L
#.#L#L#.##

At this point, something interesting happens: the chaos stabilizes and further applications of these rules cause no seats to change state! Once people stop moving around, you count 37 occupied seats.

Simulate your seating area by applying the seating rules repeatedly until no seats change state. How many seats end up occupied?

Your puzzle answer was 2281.

--- Part Two ---
As soon as people start to arrive, you realize your mistake. People don't just care about adjacent seats - they care about the first seat they can see in each of those eight directions!

Now, instead of considering just the eight immediately adjacent seats, consider the first seat in each of those eight directions. For example, the empty seat below would see eight occupied seats:

.......#.
...#.....
.#.......
.........
..#L....#
....#....
.........
#........
...#.....

The leftmost empty seat below would only see one empty seat, but cannot see any of the occupied ones:

.............
.L.L.#.#.#.#.
.............

The empty seat below would see no occupied seats:

.##.##.
#.#.#.#
##...##
...L...
##...##
#.#.#.#
.##.##.

Also, people seem to be more tolerant than you expected: it now takes five or more visible occupied seats for an occupied seat to become empty (rather than four or more from the previous rules). The other rules still apply: empty seats that see no occupied seats become occupied, seats matching no rule don't change, and floor never changes.

Given the same starting layout as above, these new rules cause the seating area to shift around as follows:

L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL

#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##

#.LL.LL.L#
#LLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLLL.L
#.LLLLL.L#

#.L#.##.L#
#L#####.LL
L.#.#..#..
##L#.##.##
#.##.#L.##
#.#####.#L
..#.#.....
LLL####LL#
#.L#####.L
#.L####.L#

#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##LL.LL.L#
L.LL.LL.L#
#.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLL#.L
#.L#LL#.L#

#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.#L.L#
#.L####.LL
..#.#.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#

#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.LL.L#
#.LLLL#.LL
..#.L.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#

Again, at this point, people stop shifting around and the seating area reaches equilibrium. Once this occurs, you count 26 occupied seats.

Given the new visibility method and the rule change for occupied seats becoming empty, once equilibrium is reached, how many seats end up occupied?

Your puzzle answer was 2085.
"""

# 4 possible states:
# X: seat empty, becomes occupied
# L: seat empty, remains empty
# #: seat occupied, remains occupied
# O: seat occupied, becomes empty

import os
import types


def read_input() -> None:
    grid: [str] = []

    input_file_path: str = os.path.join(os.getcwd(), "day11\\input.txt")
    with open(input_file_path, "r") as input_file:
        for line in input_file.readlines():
            grid.append(list(line.strip()))

    return grid


def update_seat(grid: [str], row: int, col: int, current_state: str) -> None:
    low_row: int = max(row - 1, 0)
    low_col: int = max(col - 1, 0)
    high_row: int = min(len(grid) - 1, row + 1)
    high_col: int = min(len(grid[0]) - 1, col + 1)

    occupied_count: int = -1 if current_state == "#" else 0
    for i in range(low_row, high_row + 1):
        for j in range(low_col, high_col + 1):
            if grid[i][j] == "#" or grid[i][j] == "O":
                occupied_count += 1

    if current_state == "#":
        if occupied_count >= 4:
            grid[row][col] = "O"
    if current_state == "L":
        if occupied_count == 0:
            grid[row][col] = "X"


def update_all_seats(grid: [str]) -> bool:
    steady_state: bool = True

    for i in range(len(grid)):
        for j in range(len(grid[0])):
            current_state: str = grid[i][j]
            if current_state == ".":
                continue

            update_seat(grid, i, j, current_state)
            if grid[i][j] != current_state:
                steady_state = False

    for i in range(len(grid)):
        for j in range(len(grid[0])):
            if grid[i][j] == "X":
                grid[i][j] = "#"
            elif grid[i][j] == "O":
                grid[i][j] = "L"
            else:
                continue

    return steady_state


def update_seat_directional(grid: [str], row: int, col: int) -> bool:
    for i in range(row + 1, len(grid)):
        if grid[i][col] == "#" or grid[i][col] == "O":
            return False
        if grid[i][col] == "L" or grid[i][col] == "X":
            break
    for i in range(row - 1, -1, -1):
        if grid[i][col] == "#" or grid[i][col] == "O":
            return False
        if grid[i][col] == "L" or grid[i][col] == "X":
            break
    for j in range(col + 1, len(grid[0])):
        if grid[row][j] == "#" or grid[row][j] == "O":
            return False
        if grid[row][j] == "L" or grid[row][j] == "X":
            break
    for j in range(col - 1, -1, -1):
        if grid[row][j] == "#" or grid[row][j] == "O":
            return False
        if grid[row][j] == "L" or grid[row][j] == "X":
            break

    # Diagonals
    left: int = col - 1
    right: int = col + 1
    for i in range(row + 1, len(grid)):
        if left < 0 and right >= len(grid[i]):
            break

        if left >= 0:
            if grid[i][left] == "#" or grid[i][left] == "O":
                return False
            if grid[i][left] == "L" or grid[i][left] == "X":
                left = -1
            left -= 1
        if right < len(grid[i]):
            if grid[i][right] == "#" or grid[i][right] == "O":
                return False
            if grid[i][right] == "L" or grid[i][right] == "X":
                right = len(grid[i])
            right += 1

    left = col - 1
    right = col + 1
    for i in range(row - 1, -1, -1):
        if left < 0 and right >= len(grid[i]):
            break

        if left >= 0:
            if grid[i][left] == "#" or grid[i][left] == "O":
                return False
            if grid[i][left] == "L" or grid[i][left] == "X":
                left = -1
            left -= 1
        if right < len(grid[i]):
            if grid[i][right] == "#" or grid[i][right] == "O":
                return False
            if grid[i][right] == "L" or grid[i][right] == "X":
                right = len(grid[i])
            right += 1

    return True


def vacate_seat_directional(grid: [str], row: int, col: int) -> None:
    occupied_count: int = 0

    for i in range(row + 1, len(grid)):
        if grid[i][col] == "#" or grid[i][col] == "O":
            occupied_count += 1
            break
        if grid[i][col] == "L" or grid[i][col] == "X":
            break
    for i in range(row - 1, -1, -1):
        if grid[i][col] == "#" or grid[i][col] == "O":
            occupied_count += 1
            break
        if grid[i][col] == "L" or grid[i][col] == "X":
            break
    for j in range(col + 1, len(grid[0])):
        if grid[row][j] == "#" or grid[row][j] == "O":
            occupied_count += 1
            break
        if grid[row][j] == "L" or grid[row][j] == "X":
            break
    for j in range(col - 1, -1, -1):
        if grid[row][j] == "#" or grid[row][j] == "O":
            occupied_count += 1
            break
        if grid[row][j] == "L" or grid[row][j] == "X":
            break

    # Diagonals
    left: int = col - 1
    right: int = col + 1
    for i in range(row + 1, len(grid)):
        if left < 0 and right >= len(grid[i]):
            break

        if left >= 0:
            if grid[i][left] == "#" or grid[i][left] == "O":
                occupied_count += 1
                left = -1
        if left >= 0:
            if grid[i][left] == "L" or grid[i][left] == "X":
                left = -1
            left -= 1
        if right < len(grid[i]):
            if grid[i][right] == "#" or grid[i][right] == "O":
                occupied_count += 1
                right = len(grid[i])
        if right < len(grid[i]):
            if grid[i][right] == "L" or grid[i][right] == "X":
                right = len(grid[i])
            right += 1

    left = col - 1
    right = col + 1
    for i in range(row - 1, -1, -1):
        if left < 0 and right >= len(grid[i]):
            break

        if left >= 0:
            if grid[i][left] == "#" or grid[i][left] == "O":
                occupied_count += 1
                left = -1
        if left >= 0:
            if grid[i][left] == "L" or grid[i][left] == "X":
                left = -1
            left -= 1
        if right < len(grid[i]):
            if grid[i][right] == "#" or grid[i][right] == "O":
                occupied_count += 1
                right = len(grid[i])
        if right < len(grid[i]):
            if grid[i][right] == "L" or grid[i][right] == "X":
                right = len(grid[i])
            right += 1

    if occupied_count >= 5:
        grid[row][col] = "O"


def update_all_seats_directional(grid: [str]) -> bool:
    steady_state: bool = True

    for i in range(len(grid)):
        for j in range(len(grid[0])):
            current_state: str = grid[i][j]
            if current_state == ".":
                continue

            if current_state == "L":
                if update_seat_directional(grid, i, j):
                    grid[i][j] = "X"
            elif current_state == "#":
                vacate_seat_directional(grid, i, j)

            if grid[i][j] != current_state:
                steady_state = False

    for i in range(len(grid)):
        for j in range(len(grid[0])):
            if grid[i][j] == "X":
                grid[i][j] = "#"
            elif grid[i][j] == "O":
                grid[i][j] = "L"
            else:
                continue

    return steady_state


def stabilize(directional: bool = False) -> int:
    grid: [str] = read_input()
    steady_state: bool = False

    while not steady_state:
        if directional:
            steady_state = update_all_seats_directional(grid)
        else:
            steady_state = update_all_seats(grid)

    occupied_seats: int = 0
    for row in grid:
        for seat in row:
            if seat == "#":
                occupied_seats += 1

    return occupied_seats


# print(stabilize())
print(stabilize(True))