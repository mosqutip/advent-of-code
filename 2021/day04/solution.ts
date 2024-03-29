/*
--- Day 4: Giant Squid ---

You're already almost 1.5km (almost a mile) below the surface of the ocean, already so deep that you can't see any sunlight. What you can see, however, is a giant squid that has attached itself to the outside of your submarine.

Maybe it wants to play bingo?

Bingo is played on a set of boards each consisting of a 5x5 grid of numbers. Numbers are chosen at random, and the chosen number is marked on all boards on which it appears. (Numbers may not appear on all boards.) If all numbers in any row or any column of a board are marked, that board wins. (Diagonals don't count.)

The submarine has a bingo subsystem to help passengers (currently, you and the giant squid) pass the time. It automatically generates a random order in which to draw numbers and a random set of boards (your puzzle input). For example:

7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7

After the first five numbers are drawn (7, 4, 9, 5, and 11), there are no winners, but the boards are marked as follows (shown here adjacent to each other to save space):

22 13 17 11  0         3 15  0  2 22        14 21 17 24  4
 8  2 23  4 24         9 18 13 17  5        10 16 15  9 19
21  9 14 16  7        19  8  7 25 23        18  8 23 26 20
 6 10  3 18  5        20 11 10 24  4        22 11 13  6  5
 1 12 20 15 19        14 21 16 12  6         2  0 12  3  7

After the next six numbers are drawn (17, 23, 2, 0, 14, and 21), there are still no winners:

22 13 17 11  0         3 15  0  2 22        14 21 17 24  4
 8  2 23  4 24         9 18 13 17  5        10 16 15  9 19
21  9 14 16  7        19  8  7 25 23        18  8 23 26 20
 6 10  3 18  5        20 11 10 24  4        22 11 13  6  5
 1 12 20 15 19        14 21 16 12  6         2  0 12  3  7

Finally, 24 is drawn:

22 13 17 11  0         3 15  0  2 22        14 21 17 24  4
 8  2 23  4 24         9 18 13 17  5        10 16 15  9 19
21  9 14 16  7        19  8  7 25 23        18  8 23 26 20
 6 10  3 18  5        20 11 10 24  4        22 11 13  6  5
 1 12 20 15 19        14 21 16 12  6         2  0 12  3  7

At this point, the third board wins because it has at least one complete row or column of marked numbers (in this case, the entire top row is marked: 14 21 17 24 4).

The score of the winning board can now be calculated. Start by finding the sum of all unmarked numbers on that board; in this case, the sum is 188. Then, multiply that sum by the number that was just called when the board won, 24, to get the final score, 188 * 24 = 4512.

To guarantee victory against the giant squid, figure out which board will win first. What will your final score be if you choose that board?

Your puzzle answer was 41503.

--- Part Two ---

On the other hand, it might be wise to try a different strategy: let the giant squid win.

You aren't sure how many bingo boards a giant squid could play at once, so rather than waste time counting its arms, the safe thing to do is to figure out which board will win last and choose that one. That way, no matter which boards it picks, it will win for sure.

In the above example, the second board is the last to win, which happens after 13 is eventually called and its middle column is completely marked. If you were to keep playing until this point, the second board would have a sum of unmarked numbers equal to 148 for a final score of 148 * 13 = 1924.

Figure out which board will win last. Once it wins, what would its final score be?

Your puzzle answer was 3178.
*/

import { printAnswer, readInputFile } from '../common/utilities';

const inputLines = readInputFile(__dirname);
const BOARD_SIZE = 5;

let drawNumbers: Array<number>;
let boards: Array<Array<Array<[number, boolean]>>>;
let boardLookups: Array<Map<number, Array<number>>>;
let boardsWon: Set<number>;

function setupGame() {
    boards = [];
    boardLookups = [];
    boardsWon = new Set();
    drawNumbers = inputLines[0].split(',').map((number) => parseInt(number));

    for (let i = 1; i < inputLines.length; i += BOARD_SIZE) {
        let currentBoard = [];
        let currentBoardLookup: Map<number, Array<number>> = new Map();
        for (let j = 0; j < BOARD_SIZE; j++) {
            let currentBoardLine: Array<[number, boolean]> = [];
            let line = inputLines[i + j];
            let numbers = line
                .split(' ')
                .filter((number) => number != '')
                .map((number) => parseInt(number));
            for (let k = 0; k < BOARD_SIZE; k++) {
                currentBoardLine.push([numbers[k], false]);
                currentBoardLookup.set(numbers[k], [j, k]);
            }

            currentBoard.push(currentBoardLine);
        }

        boards.push(currentBoard);
        boardLookups.push(currentBoardLookup);
    }
}

function markDrawnNumber(drawnNumber: number) {
    for (let i = 0; i < boards.length; i++) {
        if (boardLookups[i].has(drawnNumber)) {
            let coords = boardLookups[i].get(drawnNumber);
            if (coords) {
                boards[i][coords[0]][coords[1]][1] = true;
            }
        }
    }
}

function getWinningBoard() {
    for (let i = 0; i < boards.length; i++) {
        if (isWinner(boards[i])) {
            return i;
        }
    }

    return -1;
}

function checkWinningBoards() {
    for (let i = 0; i < boards.length; i++) {
        if (!boardsWon.has(i) && isWinner(boards[i])) {
            boardsWon.add(i);
        }
    }
}

function isWinner(board: Array<Array<[number, boolean]>>) {
    let rowWinner = true;
    let colWinner = true;
    for (let i = 0; i < board.length; i++) {
        rowWinner = true;
        for (let j = 0; j < board.length; j++) {
            if (!board[i][j][1]) {
                rowWinner = false;
                break;
            }
        }

        if (rowWinner) {
            return true;
        }

        colWinner = true;
        for (let j = 0; j < board[i].length; j++) {
            if (!board[j][i][1]) {
                colWinner = false;
                break;
            }
        }

        if (colWinner) {
            return true;
        }
    }

    return false;
}

function calculateScore(board: Array<Array<[number, boolean]>>, lastDrawnNumber: number) {
    let undrawnSum = 0;
    for (let i = 0; i < board.length; i++) {
        for (let j = 0; j < board.length; j++) {
            if (!board[i][j][1]) {
                undrawnSum += board[i][j][0];
            }
        }
    }

    return undrawnSum * lastDrawnNumber;
}

function winGame() {
    setupGame();

    for (let i = 0; i < BOARD_SIZE - 1; i++) {
        markDrawnNumber(drawNumbers[i]);
    }

    // We can safely draw four numbers (indices 0 to [BOARD_SIZE - 2]) before a win is possible.
    let lastDrawnIndex = BOARD_SIZE - 2;
    let isGameOver = false;
    let score = 0;
    while (!isGameOver) {
        lastDrawnIndex++;
        markDrawnNumber(drawNumbers[lastDrawnIndex]);

        let winningBoard = getWinningBoard();
        if (winningBoard != -1) {
            score = calculateScore(boards[winningBoard], drawNumbers[lastDrawnIndex]);
            isGameOver = true;
        }
    }

    return score;
}

function loseGame() {
    setupGame();

    for (let i = 0; i < BOARD_SIZE - 1; i++) {
        markDrawnNumber(drawNumbers[i]);
    }

    let lastDrawnIndex = BOARD_SIZE - 2;
    let isGameOver = false;
    let score = 0;
    let lastBoard = -1;
    while (!isGameOver) {
        lastDrawnIndex++;
        markDrawnNumber(drawNumbers[lastDrawnIndex]);

        checkWinningBoards();
        if (lastBoard == -1 && boardsWon.size == boards.length - 1) {
            let allBoards = new Set(Array.from(boards).keys());
            let difference = new Set([...allBoards].filter((board) => !boardsWon.has(board)));
            lastBoard = [...difference][0];
        } else if (boardsWon.size == boards.length) {
            score = calculateScore(boards[lastBoard], drawNumbers[lastDrawnIndex]);
            isGameOver = true;
        }
    }

    return score;
}

const partOneAnswer = winGame();
printAnswer(1, partOneAnswer.toString());

const partTwoAnswer = loseGame();
printAnswer(2, partTwoAnswer.toString());
