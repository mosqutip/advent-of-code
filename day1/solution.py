"""
--- Day 1: Report Repair ---
Before you leave, the Elves in accounting just need you to fix your expense report (your puzzle input); apparently, something isn't quite adding up.

Specifically, they need you to find the two entries that sum to 2020 and then multiply those two numbers together.

For example, suppose your expense report contained the following:

1721
979
366
299
675
1456

In this list, the two entries that sum  to 2020 are 1721 and 299. Multiplying them together produces 1721 * 299 = 514579, so the correct answer is 514579.

Of course, your expense report is much larger. Find the two entries that sum to 2020; what do you get if you multiply them together?

Your puzzle answer was 793524.

--- Part Two ---
The Elves in accounting are thankful for your help; one of them even offers you a starfish coin they had left over from a past vacation. They offer you a second one if you can find three numbers in your expense report that meet the same criteria.

Using the above example again, the three entries that sum to 2020 are 979, 366, and 675. Multiplying them together produces the answer, 241861950.

In your expense report, what is the product of the three entries that sum to 2020?

Your puzzle answer was 61515678.
"""

import os
import types

target: int = 2020


def read_input() -> [int]:
    nums: [int] = []

    input_file_path: str = os.path.join(os.getcwd(), "day1\\input.txt")
    with open(input_file_path, "r") as input_file:
        for line in input_file.readlines():
            nums.append(int(line.strip()))

    return nums


def two_sum() -> int:
    nums: [int] = read_input()
    entries: {int} = set()

    for num in nums:
        if (target - num) in entries:
            return (target - num) * num

        entries.add(num)

    return -1


def three_sum() -> int:
    nums: [int] = read_input()

    for i in range(len(nums) - 1):
        entries: {int: (int, int)} = {}
        current_target: int = target - nums[i]

        for j in range((i + 1), len(nums)):
            two_sum: int = current_target - nums[j]
            if two_sum <= 0:
                continue

            if two_sum in entries:
                return nums[j] * entries[two_sum][0] * entries[two_sum][1]

            entries[nums[j]] = (nums[i], nums[j])

    return -1


print(two_sum())
print(three_sum())
