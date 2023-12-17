"""
--- Day 4: The Ideal Stocking Stuffer ---

Santa needs help mining some AdventCoins (very similar to bitcoins) to use as gifts for all the economically forward-thinking little girls and boys.

To do this, he needs to find MD5 hashes which, in hexadecimal, start with at least five zeroes. The input to the MD5 hash is some secret key (your puzzle input, given below) followed by a number in decimal. To mine AdventCoins, you must find Santa the lowest positive number (no leading zeroes: 1, 2, 3, ...) that produces such a hash.

For example:

    If your secret key is abcdef, the answer is 609043, because the MD5 hash of abcdef609043 starts with five zeroes (000001dbbfa...), and it is the lowest such number to do so.
    If your secret key is pqrstuv, the lowest number it combines with to make an MD5 hash starting with five zeroes is 1048970; that is, the MD5 hash of pqrstuv1048970 looks like 000006136ef....

Your puzzle input is yzbqklnj.

Your puzzle answer was 282749.

--- Part Two ---

Now find one that starts with six zeroes.
"""

import os
import hashlib


def read_input() -> str:
    key: str = None

    input_file_path: str = os.path.join(os.getcwd(), "2020\\day4\\input.txt")
    with open(input_file_path, "r") as input_file:
        key = input_file.readline().strip()

    return key


def compute_lowest_hash(key: str, digits: int) -> int:
    zeroes: str = "0" * digits
    num: int = 0
    starting_digits: str = None

    while starting_digits != zeroes:
        num += 1
        test_input: str = (key + str(num)).encode()
        hex_hash: str = hashlib.md5(test_input).hexdigest()
        starting_digits = hex_hash[:digits]

    return num


key: str = read_input()
print(compute_lowest_hash(key, 5))
print(compute_lowest_hash(key, 6))
