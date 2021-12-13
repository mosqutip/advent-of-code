/*
--- Day 13: Transparent Origami ---

You reach another volcanically active part of the cave. It would be nice if you could do some kind of thermal imaging so you could tell ahead of time which caves are too hot to safely enter.

Fortunately, the submarine seems to be equipped with a thermal camera! When you activate it, you are greeted with:

Congratulations on your purchase! To activate this infrared thermal imaging
camera system, please enter the code found on page 1 of the manual.

Apparently, the Elves have never used this feature. To your surprise, you manage to find the manual; as you go to open it, page 1 falls out. It's a large sheet of transparent paper! The transparent paper is marked with random dots and includes instructions on how to fold it up (your puzzle input). For example:

6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5

The first section is a list of dots on the transparent paper. 0,0 represents the top-left coordinate. The first value, x, increases to the right. The second value, y, increases downward. So, the coordinate 3,0 is to the right of 0,0, and the coordinate 0,7 is below 0,0. The coordinates in this example form the following pattern, where # is a dot on the paper and . is an empty, unmarked position:

...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........

Then, there is a list of fold instructions. Each instruction indicates a line on the transparent paper and wants you to fold the paper up (for horizontal y=... lines) or left (for vertical x=... lines). In this example, the first fold instruction is fold along y=7, which designates the line formed by all of the positions where y is 7 (marked here with -):

...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
-----------
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........

Because this is a horizontal line, fold the bottom half up. Some of the dots might end up overlapping after the fold is complete, but dots will never appear exactly on a fold line. The result of doing this fold looks like this:

#.##..#..#.
#...#......
......#...#
#...#......
.#.#..#.###
...........
...........

Now, only 17 dots are visible.

Notice, for example, the two dots in the bottom left corner before the transparent paper is folded; after the fold is complete, those dots appear in the top left corner (at 0,0 and 0,1). Because the paper is transparent, the dot just below them in the result (at 0,3) remains visible, as it can be seen through the transparent paper.

Also notice that some dots can end up overlapping; in this case, the dots merge together and become a single dot.

The second fold instruction is fold along x=5, which indicates this line:

#.##.|#..#.
#...#|.....
.....|#...#
#...#|.....
.#.#.|#.###
.....|.....
.....|.....

Because this is a vertical line, fold left:

#####
#...#
#...#
#...#
#####
.....
.....

The instructions made a square!

The transparent paper is pretty big, so for now, focus on just completing the first fold. After the first fold in the example above, 17 dots are visible - dots that end up overlapping after the fold is completed count as a single dot.

How many dots are visible after completing just the first fold instruction on your transparent paper?

Your puzzle answer was 621.

--- Part Two ---

Finish folding the transparent paper according to the instructions. The manual says the code is always eight capital letters.

What code do you use to activate the infrared thermal imaging camera system?

Your puzzle answer was HKUJGAJZ
*/

import { printAnswer, readInputFile } from '../common/utilities';

const inputLines = readInputFile(__dirname);

function constructPaper() {
    let coords = new Map<string, number>();
    let folds: Array<Array<any>> = [];
    for (let line of inputLines) {
        if (line.startsWith('fold')) {
            let fold: Array<string> = line.split(' ').pop()?.split('=')!;
            folds.push([fold[0], parseInt(fold[1])]);
        } else {
            coords.set(line, 1);
        }
    }

    let result: Array<any> = [coords, folds];
    return result;
}

function fold(coords: Map<string, number>, fold: Array<any>) {
    let horizontalFold = true;
    if (fold[0] == 'y') {
        horizontalFold = false;
    }

    let keys = Array.from(coords.keys());
    for (let coord of keys) {
        let vals = coord.split(',').map((num) => parseInt(num));
        if (horizontalFold && vals[0] > fold[1]) {
            let difference = vals[0] - fold[1];
            vals[0] -= difference * 2;
        } else if (!horizontalFold && vals[1] > fold[1]) {
            let difference = vals[1] - fold[1];
            vals[1] -= difference * 2;
        }

        let newCoord = `${vals[0]},${vals[1]}`;
        if (newCoord != coord) {
            if (coords.has(newCoord)) {
                coords.set(newCoord, coords.get(newCoord)! + 1);
            } else {
                coords.set(newCoord, 1);
            }

            coords.delete(coord);
        }
    }

    return coords;
}

function printCoords(coords: Map<string, number>) {
    let values = Array.from(coords.keys())
        .map((str) => str.split(','))
        .map((pair) => [parseInt(pair[0]), parseInt(pair[1])]);

    let maxRow = 0;
    let maxCol = 0;
    for (let pair of values) {
        if (pair[0] > maxRow) {
            maxRow = pair[0];
        }
        if (pair[1] > maxCol) {
            maxCol = pair[1];
        }
    }

    for (let i = 0; i <= maxRow; i++) {
        let line = [];
        for (let j = 0; j <= maxCol; j++) {
            if (coords.has(`${i},${j}`)) {
                line.push('#');
            } else {
                line.push('.');
            }
        }
        console.log(line);
    }
    console.log();
}

let data = constructPaper();
let coords = data[0];
let folds = data[1];
coords = fold(coords, folds[0]);
printAnswer(1, coords.size);

for (let i = 1; i < data[1].length; i++) {
    coords = fold(coords, folds[i]);
}
printCoords(coords);
