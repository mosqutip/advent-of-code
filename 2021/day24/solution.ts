/*
--- Day 24: Arithmetic Logic Unit ---

Magic smoke starts leaking from the submarine's arithmetic logic unit (ALU). Without the ability to perform basic arithmetic and logic functions, the submarine can't produce cool patterns with its Christmas lights!

It also can't navigate. Or run the oxygen system.

Don't worry, though - you probably have enough oxygen left to give you enough time to build a new ALU.

The ALU is a four-dimensional processing unit: it has integer variables w, x, y, and z. These variables all start with the value 0. The ALU also supports six instructions:

    inp a - Read an input value and write it to variable a.
    add a b - Add the value of a to the value of b, then store the result in variable a.
    mul a b - Multiply the value of a by the value of b, then store the result in variable a.
    div a b - Divide the value of a by the value of b, truncate the result to an integer, then store the result in variable a. (Here, "truncate" means to round the value toward zero.)
    mod a b - Divide the value of a by the value of b, then store the remainder in variable a. (This is also called the modulo operation.)
    eql a b - If the value of a and b are equal, then store the value 1 in variable a. Otherwise, store the value 0 in variable a.

In all of these instructions, a and b are placeholders; a will always be the variable where the result of the operation is stored (one of w, x, y, or z), while b can be either a variable or a number. Numbers can be positive or negative, but will always be integers.

The ALU has no jump instructions; in an ALU program, every instruction is run exactly once in order from top to bottom. The program halts after the last instruction has finished executing.

(Program authors should be especially cautious; attempting to execute div with b=0 or attempting to execute mod with a<0 or b<=0 will cause the program to crash and might even damage the ALU. These operations are never intended in any serious ALU program.)

For example, here is an ALU program which takes an input number, negates it, and stores it in x:

inp x
mul x -1

Here is an ALU program which takes two input numbers, then sets z to 1 if the second input number is three times larger than the first input number, or sets z to 0 otherwise:

inp z
inp x
mul z 3
eql z x

Here is an ALU program which takes a non-negative integer as input, converts it into binary, and stores the lowest (1's) bit in z, the second-lowest (2's) bit in y, the third-lowest (4's) bit in x, and the fourth-lowest (8's) bit in w:

inp w
add z w
mod z 2
div w 2
add y w
mod y 2
div w 2
add x w
mod x 2
div w 2
mod w 2

Once you have built a replacement ALU, you can install it in the submarine, which will immediately resume what it was doing when the ALU failed: validating the submarine's model number. To do this, the ALU will run the MOdel Number Automatic Detector program (MONAD, your puzzle input).

Submarine model numbers are always fourteen-digit numbers consisting only of digits 1 through 9. The digit 0 cannot appear in a model number.

When MONAD checks a hypothetical fourteen-digit model number, it uses fourteen separate inp instructions, each expecting a single digit of the model number in order of most to least significant. (So, to check the model number 13579246899999, you would give 1 to the first inp instruction, 3 to the second inp instruction, 5 to the third inp instruction, and so on.) This means that when operating MONAD, each input instruction should only ever be given an integer value of at least 1 and at most 9.

Then, after MONAD has finished running all of its instructions, it will indicate that the model number was valid by leaving a 0 in variable z. However, if the model number was invalid, it will leave some other non-zero value in z.

MONAD imposes additional, mysterious restrictions on model numbers, and legend says the last copy of the MONAD documentation was eaten by a tanuki. You'll need to figure out what MONAD does some other way.

To enable as many submarine features as possible, find the largest valid fourteen-digit model number that contains no 0 digits. What is the largest model number accepted by MONAD?

Your puzzle answer was 39494195799979.

--- Part Two ---

As the submarine starts booting up things like the Retro Encabulator, you realize that maybe you don't need all these submarine features after all.

What is the smallest model number accepted by MONAD?

Your puzzle answer was 13161151139617.
*/

import { printAnswer, readInputFile } from '../common/utilities';

const inputLines = readInputFile(__dirname);

function runBlock(lines: string[], registers: number[]): number[] {
    const offset = 'w'.charCodeAt(0);

    for (let line of lines) {
        const command = line.split(' ');
        const instruction = command[0];
        const register = command[1].charCodeAt(0) - offset;

        let argument = Number(command[2]);
        if (isNaN(Number(command[2]))) {
            argument = registers[command[2].charCodeAt(0) - offset];
        }

        switch (instruction) {
            case 'add':
                registers[register] += argument;
                break;
            case 'mul':
                registers[register] *= argument;
                break;
            case 'div':
                registers[register] = Math.floor(registers[register] / argument);
                break;
            case 'mod':
                registers[register] %= argument;
                break;
            case 'eql':
                registers[register] = registers[register] === argument ? 1 : 0;
                break;
            default:
                break;
        }
    }

    return registers;
}

function isValidSerialNumber(serialNumber: string) {
    let inputIndices = inputLines
        .map((line, index) => (line.startsWith('inp') ? index : undefined))
        .filter((index) => index !== undefined);
    inputIndices.push(inputLines.length);
    let testNumber = serialNumber.split('').map((digit) => Number(digit));

    let registers = [0, 0, 0, 0];
    for (let i = 0; i < inputIndices.length - 1; i++) {
        let block = inputLines.slice(inputIndices[i]! + 1, inputIndices[i + 1]);

        registers[0] = testNumber[i];
        registers = runBlock(block, registers);
    }

    return registers[3] === 0;
}

const partOneAnswer = '39494195799979';
if (isValidSerialNumber(partOneAnswer)) {
    printAnswer(1, partOneAnswer.toString());
}

const partTwoAnswer = '13161151139617';
if (isValidSerialNumber(partTwoAnswer)) {
    printAnswer(2, partTwoAnswer.toString());
}

/*
Each block:
    x = 0
    x = z
    x %= 26
    z /= [1 | 26]
    x += u1
    x == w
    x == 0
    y = 0
    y += 25
    y *= x
    y += 1
    z *= y
    y = 0
    y += w
    y += u2
    y *= x
    z += y

Rule for x:
    u1 is positive 7 times, negative 7 times, and when positive always >= 10
    if u1 is positive
        x = z % 26, x = 0..25
        x + u1 = 10..35 (u1 always >= 10 when positive)
        w = 1..9, u1 > w
        x + u1 > w
        z = [(z * 26) + w + u2]
    else if u1 is negative
        x = z % 26, x = 0..25
        x + u1 = -∞..25
        if [(z % 26) + u1] == w
            x == w
        if u1 -24..0
            x ?= w

Rule for y:
    if x == w
        y = 0
        z = [(z * 1) + 0], z = z
    else:
        y = 1
        z = [(z * 26) + w + u2]

Rule for z:
    z(14) = 0

Therefore:
    - The 7 blocks with positive u1 values always INCREASE the z value
        - Positive u1 is always >= 10
        - x + u1 is strictly > w
        - z increases by [(z * 26) + w + u2]
    - The 7 blocks with negative u1 values must then always DECREASE the z value
        - This undoes the multiplication by 26 from the positive steps
    - x - u1 must EQUAL w when u1 is negative
        - Otherwise, z will increase in a block with negative u1 values

Looking at each block instruction for u1 and u2:
add x u1    [z = (z * 26) + w + u2]     (for POSITIVE u1)
add x u1    [z = (z / 26); w = x - u1]  (for NEGATIVE u1)

add x 13    [z = (z * 26) + w + 6]
add x 15    [z = (z * 26) + w + 7]
add x 15    [z = (z * 26) + w + 10]
add x 11    [z = (z * 26) + w + 2]
add x -7    [z = (z / 26); w = x - 7]
add x 10    [z = (z * 26) + w + 5]
add x 10    [z = (z * 26) + w + 1]
add x -5    [z = (z / 26); w = x - 5]
add x 15    [z = (z * 26) + w + 5]
add x -3    [z = (z / 26); w = x - 3]
add x  0    [z = (z / 26); w = x]
add x -5    [z = (z / 26); w = x - 5]
add x -9    [z = (z / 26); w = x - 9]
add x  0    [z = (z / 26); w = x]

Since each add multiplies z by 26, and each subtraction sets z to z modulo 26,
we can ignore everything but the added values (w + u2) in the comparison:
POSITIVE    add x 13    w + 6 ===========================================\  3   1
POSITIVE    add x 15    w + 7 ===================================\  9   3 \
POSITIVE    add x 15    w + 10 ==========================\  4   1 \        \
POSITIVE    add x 11    w + 2 ========\ 9   6             \        \        \
NEGATIVE    add x -7    w = z - 7 ====/ 4   1              \        \        \
POSITIVE    add x 10    w + 8 ===============\      1   1   \        \        \
POSITIVE    add x 10    w + 1 ========\ 9   5 \              \        \        \
NEGATIVE    add x -5    w = z - 5 ====/ 5   1  \             /        /        /
POSITIVE    add x 15    w + 5 ========\ 7   1  /            /        /        /
NEGATIVE    add x -3    w = z - 3 ====/ 9   3 /            /        /        /
NEGATIVE    add x  0    w = z ===============/      9   9 /        /        /
NEGATIVE    add x -5    w = z - 5 =======================/  9   6 /        /
NEGATIVE    add x -9    w = z - 9 ===============================/  7   1 /
NEGATIVE    add x  0    w = z ===========================================/  9   7

=> 39494195799979
=> 13161151139617
*/
