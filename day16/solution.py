"""
--- Day 16: Ticket Translation ---
As you're walking to yet another connecting flight, you realize that one of the legs of your re-routed trip coming up is on a high-speed train. However, the train ticket you were given is in a language you don't understand. You should probably figure out what it says before you get to the train station after the next flight.

Unfortunately, you can't actually read the words on the ticket. You can, however, read the numbers, and so you figure out the fields these tickets must have and the valid ranges for values in those fields.

You collect the rules for ticket fields, the numbers on your ticket, and the numbers on other nearby tickets for the same train service (via the airport security cameras) together into a single document you can reference (your puzzle input).

The rules for ticket fields specify a list of fields that exist somewhere on the ticket and the valid ranges of values for each field. For example, a rule like class: 1-3 or 5-7 means that one of the fields in every ticket is named class and can be any value in the ranges 1-3 or 5-7 (inclusive, such that 3 and 5 are both valid in this field, but 4 is not).

Each ticket is represented by a single line of comma-separated values. The values are the numbers on the ticket in the order they appear; every ticket has the same format. For example, consider this ticket:

.--------------------------------------------------------.
| ????: 101    ?????: 102   ??????????: 103     ???: 104 |
|                                                        |
| ??: 301  ??: 302             ???????: 303      ??????? |
| ??: 401  ??: 402           ???? ????: 403    ????????? |
'--------------------------------------------------------'

Here, ? represents text in a language you don't understand. This ticket might be represented as 101,102,103,104,301,302,303,401,402,403; of course, the actual train tickets you're looking at are much more complicated. In any case, you've extracted just the numbers in such a way that the first number is always the same specific field, the second number is always a different specific field, and so on - you just don't know what each position actually means!

Start by determining which tickets are completely invalid; these are tickets that contain values which aren't valid for any field. Ignore your ticket for now.

For example, suppose you have the following notes:

class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12

It doesn't matter which position corresponds to which field; you can identify invalid nearby tickets by considering only whether tickets contain values that are not valid for any field. In this example, the values on the first nearby ticket are all valid for at least one field. This is not true of the other three nearby tickets: the values 4, 55, and 12 are are not valid for any field. Adding together all of the invalid values produces your ticket scanning error rate: 4 + 55 + 12 = 71.

Consider the validity of the nearby tickets you scanned. What is your ticket scanning error rate?

Your puzzle answer was 21071.

--- Part Two ---
Now that you've identified which tickets contain invalid values, discard those tickets entirely. Use the remaining valid tickets to determine which field is which.

Using the valid ranges for each field, determine what order the fields appear on the tickets. The order is consistent between all tickets: if seat is the third field, it is the third field on every ticket, including your ticket.

For example, suppose you have the following notes:

class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9

Based on the nearby tickets in the above example, the first position must be row, the second position must be class, and the third position must be seat; you can conclude that in your ticket, class is 12, row is 11, and seat is 13.

Once you work out which field is which, look for the six fields on your ticket that start with the word departure. What do you get if you multiply those six values together?

Your puzzle answer was .
"""

import os
import types


def read_input() -> [str]:
    input_file_path: str = os.path.join(os.getcwd(), "day16\\input.txt")
    with open(input_file_path, "r") as input_file:
        lines: [str] = input_file.readlines()

    return lines


def parse_input() -> ({str: [(int)]}, [[int]]):
    input: [str] = read_input()
    rules: {str: [(int)]} = {}
    tickets: [[int]] = []

    state = 0
    for line in input:
        line = line.strip()
        if not line:
            continue

        if line == "your ticket:":
            state = 1
            continue
        elif line == "nearby tickets:":
            continue

        if state == 0:
            parts: [str] = line.split(":")
            rule: str = parts[0]
            rules[rule] = []

            valid_ranges = parts[1].split()
            for valid_range in valid_ranges:
                if valid_range == "or":
                    continue

                nums = valid_range.split("-")
                rules[rule].append((int(nums[0]), int(nums[1])))
        elif state == 1:
            values: [int] = []
            for value in line.split(","):
                values.append(int(value))
            tickets.append(values)

    return rules, tickets


def calculate_error_rate() -> int:
    rules, tickets = parse_input()
    error_rate: int = 0

    for ticket in tickets[1:]:
        for num in ticket:
            if not validate_num(num, rules):
                error_rate += num

    return error_rate


def validate_num(num: int, rules: {str: [(int)]}) -> bool:
    for rule in rules.values():
        for range in rule:
            if num >= range[0] and num <= range[1]:
                return True

    return False


def get_valid_tickets(tickets: [{int}], rules: {str: [(int)]}) -> [{int}]:
    valid_tickets: [[int]] = [tickets[0]]

    for ticket in tickets[1:]:
        invalid = False
        for num in ticket:
            if not validate_num(num, rules):
                invalid = True

        if not invalid:
            valid_tickets.append(ticket)

    return valid_tickets


def assign_fields() -> ({int}, {int: str}):
    rules, tickets = parse_input()
    valid_tickets: [[int]] = get_valid_tickets(tickets, rules)
    rule_matches: {int: {str}} = {}
    assigned_rules: {str} = set()
    rule_assigments: {int: str} = {}

    for j in range(len(valid_tickets[0])):
        rule_matches[j] = set()
        test_values = [ticket_values[j] for ticket_values in valid_tickets]
        for rule_name, rule_ranges in rules.items():
            if validate_rule(test_values, rule_ranges):
                rule_matches[j].add(rule_name)

    all_matched: bool = False
    while not all_matched:
        all_matched = True

        for rule_index in rule_matches.keys():
            matches: {int} = rule_matches[rule_index]
            rule_matches[rule_index] = matches - assigned_rules
            if len(rule_matches[rule_index]) == 1:
                match: int = matches.pop()
                assigned_rules.add(match)
                all_matched = False
                rule_assigments[rule_index] = match

    return valid_tickets[0], rule_assigments


def validate_rule(nums: [int], rule: [(int)]) -> bool:
    low1, high1, low2, high2 = rule[0][0], rule[0][1], rule[1][0], rule[1][1]
    for num in nums:
        if not ((num >= low1 and num <= high1) or (num >= low2 and num <= high2)):
            return False

    return True


def calculate_departure_ticket() -> int:
    my_ticket, rule_assigments = assign_fields()

    product: int = 1
    for index, rule_name in rule_assigments.items():
        if rule_name.startswith("departure"):
            product *= my_ticket[index]

    return product


print(calculate_error_rate())
print(calculate_departure_ticket())
