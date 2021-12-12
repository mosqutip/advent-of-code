/*
--- Day 9: Smoke Basin ---

These caves seem to be lava tubes. Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.

If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).

Smoke flows to the lowest point of the area it's in. For example, consider the following heightmap:

2199943210
3987894921
9856789892
8767896789
9899965678

Each number corresponds to the height of a particular location, where 9 is the highest and 0 is the lowest a location can be.

Your first goal is to find the low points - the locations that are lower than any of its adjacent locations. Most locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)

In the above example, there are four low points, all highlighted: two are in the first row (a 1 and a 0), one is in the third row (a 5), and one is in the bottom row (also a 5). All other locations on the heightmap have some lower adjacent location, and so are not low points.

The risk level of a low point is 1 plus its height. In the above example, the risk levels of the low points are 2, 1, 6, and 6. The sum of the risk levels of all low points in the heightmap is therefore 15.

Find all of the low points on your heightmap. What is the sum of the risk levels of all low points on your heightmap?

Your puzzle answer was 439.

--- Part Two ---

Next, you need to find the largest basins so you know what areas are most important to avoid.

A basin is all locations that eventually flow downward to a single low point. Therefore, every low point has a basin, although some basins are very small. Locations of height 9 do not count as being in any basin, and all other locations will always be part of exactly one basin.

The size of a basin is the number of locations within the basin, including the low point. The example above has four basins.

The top-left basin, size 3:

2199943210
3987894921
9856789892
8767896789
9899965678

The top-right basin, size 9:

2199943210
3987894921
9856789892
8767896789
9899965678

The middle basin, size 14:

2199943210
3987894921
9856789892
8767896789
9899965678

The bottom-right basin, size 9:

2199943210
3987894921
9856789892
8767896789
9899965678

Find the three largest basins and multiply their sizes together. In the above example, this is 9 * 14 * 9 = 1134.

What do you get if you multiply together the sizes of the three largest basins?


*/

import { printAnswer, readInputFile } from '../common/utilities';

const inputLines = readInputFile(__dirname);
const LEFT_BOUND = 0;
let RIGHT_BOUND = 0;
const TOP_BOUND = 0;
let BOTTOM_BOUND = 0;

function createHeightMap() {
    let heightMap: Array<Array<Array<any>>> = [];
    for (let line of inputLines) {
        let rowVals = line.split('').map((digit) => [parseInt(digit), false]);
        heightMap.push(rowVals);
    }

    RIGHT_BOUND = heightMap[0].length - 1;
    BOTTOM_BOUND = heightMap.length - 1;

    return heightMap;
}

function findLowPoints(heightMap: Array<Array<Array<any>>>) {
    let lowPoints: Array<any> = [];
    for (let i = 0; i < heightMap.length; i++) {
        for (let j = 0; j < heightMap[i].length; j++) {
            let result = identifyLowPoint(heightMap, i, j);
            if (result[0] != -1) {
                lowPoints.push(result);
            }
        }
    }

    return lowPoints;
}

function identifyLowPoint(heightMap: Array<Array<Array<any>>>, row: number, col: number) {
    let val = heightMap[row][col][0];
    let checks = 0;
    let equal = 0;
    let nonLowPoint = [-1, []];
    if (row > TOP_BOUND) {
        checks += 1;
        if (heightMap[row - 1][col][0] == val) {
            equal += 1;
        } else if (heightMap[row - 1][col][0] < val) {
            return nonLowPoint;
        }
    }
    if (row < BOTTOM_BOUND) {
        checks += 1;
        if (heightMap[row + 1][col][0] == val) {
            equal += 1;
        } else if (heightMap[row + 1][col][0] < val) {
            return nonLowPoint;
        }
    }
    if (col > LEFT_BOUND) {
        checks += 1;
        if (heightMap[row][col - 1][0] == val) {
            equal += 1;
        } else if (heightMap[row][col - 1][0] < val) {
            return nonLowPoint;
        }
    }
    if (col < RIGHT_BOUND) {
        checks += 1;
        if (heightMap[row][col + 1][0] == val) {
            equal += 1;
        } else if (heightMap[row][col + 1][0] < val) {
            return nonLowPoint;
        }
    }

    if (checks == equal) {
        return nonLowPoint;
    }

    return [val + 1, row, col];
}

function calculateRiskLevel(lowPoints: Array<any>) {
    return lowPoints.map((lowPoint) => lowPoint[0]).reduce((sum, lowPoint) => (sum += lowPoint));
}

function findLargestBasins(heightMap: Array<Array<Array<any>>>, lowPoints: Array<Array<number>>) {
    let basinSizes: Array<number> = [];
    for (let lowPoint of lowPoints) {
        basinSizes.push(markBasin(heightMap, lowPoint[1], lowPoint[2]));
    }

    // sorted array?
    if (basinSizes.length >= 3) {
        basinSizes = basinSizes.sort((a, b) => b - a);
        return basinSizes[0] * basinSizes[1] * basinSizes[2];
    }

    return 0;
}

function markBasin(heightMap: Array<Array<Array<any>>>, row: number, col: number): number {
    heightMap[row][col][1] = true;
    let size = 1;

    if (row > TOP_BOUND) {
        if (!heightMap[row - 1][col][1] && heightMap[row - 1][col][0] != 9) {
            size += markBasin(heightMap, row - 1, col);
        }
    }
    if (row < BOTTOM_BOUND) {
        if (!heightMap[row + 1][col][1] && heightMap[row + 1][col][0] != 9) {
            size += markBasin(heightMap, row + 1, col);
        }
    }
    if (col > LEFT_BOUND) {
        if (!heightMap[row][col - 1][1] && heightMap[row][col - 1][0] != 9) {
            size += markBasin(heightMap, row, col - 1);
        }
    }
    if (col < RIGHT_BOUND) {
        if (!heightMap[row][col + 1][1] && heightMap[row][col + 1][0] != 9) {
            size += markBasin(heightMap, row, col + 1);
        }
    }

    return size;
}

let heightMap = createHeightMap();
let lowPoints = findLowPoints(heightMap);
const partOneAnswer = calculateRiskLevel(lowPoints);
printAnswer(1, partOneAnswer.toString());

const partTwoAnswer = findLargestBasins(heightMap, lowPoints);
printAnswer(2, partTwoAnswer.toString());
