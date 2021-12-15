/*
--- Day 14: Extended Polymerization ---

The incredible pressures at this depth are starting to put a strain on your submarine. The submarine has polymerization equipment that would produce suitable materials to reinforce the submarine, and the nearby volcanically-active caves should even have the necessary input elements in sufficient quantities.

The submarine manual contains instructions for finding the optimal polymer formula; specifically, it offers a polymer template and a list of pair insertion rules (your puzzle input). You just need to work out what polymer would result after repeating the pair insertion process a few times.

For example:

NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C

The first line is the polymer template - this is the starting point of the process.

The following section defines the pair insertion rules. A rule like AB -> C means that when elements A and B are immediately adjacent, element C should be inserted between them. These insertions all happen simultaneously.

So, starting with the polymer template NNCB, the first step simultaneously considers all three pairs:

    The first pair (NN) matches the rule NN -> C, so element C is inserted between the first N and the second N.
    The second pair (NC) matches the rule NC -> B, so element B is inserted between the N and the C.
    The third pair (CB) matches the rule CB -> H, so element H is inserted between the C and the B.

Note that these pairs overlap: the second element of one pair is the first element of the next pair. Also, because all pairs are considered simultaneously, inserted elements are not considered to be part of a pair until the next step.

After the first step of this process, the polymer becomes NCNBCHB.

Here are the results of a few steps using the above rules:

Template:     NNCB
After step 1: NCNBCHB
After step 2: NBCCNBBBCBHCB
After step 3: NBBBCNCCNBBNBNBBCHBHHBCHB
After step 4: NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB

This polymer grows quickly. After step 5, it has length 97; After step 10, it has length 3073. After step 10, B occurs 1749 times, C occurs 298 times, H occurs 161 times, and N occurs 865 times; taking the quantity of the most common element (B, 1749) and subtracting the quantity of the least common element (H, 161) produces 1749 - 161 = 1588.

Apply 10 steps of pair insertion to the polymer template and find the most and least common elements in the result. What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?

Your puzzle answer was 2768.

--- Part Two ---

The resulting polymer isn't nearly strong enough to reinforce the submarine. You'll need to run more steps of the pair insertion process; a total of 40 steps should do it.

In the above example, the most common element is B (occurring 2192039569602 times) and the least common element is H (occurring 3849876073 times); subtracting these produces 2188189693529.

Apply 40 steps of pair insertion to the polymer template and find the most and least common elements in the result. What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?

Your puzzle answer was 
*/

import { printAnswer, readInputFile } from '../common/utilities';

const inputLines = readInputFile(__dirname);

function parseInput(): Array<any> {
    let template = inputLines[0];
    let insertionRules = new Map<string, Array<string>>();
    for (let i = 1; i < inputLines.length; i++) {
        let rule = inputLines[i].split('->').map((part) => part.trim());
        insertionRules.set(rule[0], [`${rule[0][0]}${rule[1]}`, `${rule[1]}${rule[0][1]}`]);
    }

    return [template, insertionRules];
}

function buildPolymer(template: string, insertionRules: Map<string, Array<string>>, iterations: number) {
    let pairCounts = new Map<string, number>();
    for (let i = 0; i < template.length - 1; i++) {
        let pair = `${template[i]}${template[i + 1]}`;
        if (pairCounts.has(pair)) {
            pairCounts.set(pair, pairCounts.get(pair)! + 1);
        } else {
            pairCounts.set(pair, 1);
        }
    }

    while (iterations--) {
        let seedPairs = Array.from(pairCounts.keys());
        let stepCounts = new Map<string, number>();
        for (let seedPair of seedPairs) {
            for (let insertionPair of insertionRules.get(seedPair)!) {
                if (stepCounts.has(insertionPair)) {
                    stepCounts.set(insertionPair, stepCounts.get(insertionPair)! + pairCounts.get(seedPair)!);
                } else {
                    stepCounts.set(insertionPair, pairCounts.get(seedPair)!);
                }
            }
        }

        for (let pairCount of pairCounts.keys()) {
            pairCounts.set(pairCount, 0);
        }

        for (let stepCount of stepCounts.keys()) {
            pairCounts.set(stepCount, stepCounts.get(stepCount)!);
        }
    }

    return pairCounts;
}

function countElements(pairCounts: Map<string, number>) {
    let elementCounts = new Map<string, number>();
    for (let pair of pairCounts) {
        let firstElement = pair[0][0],
            secondElement = pair[0][1];
        if (elementCounts.has(firstElement)) {
            elementCounts.set(firstElement, elementCounts.get(firstElement)! + pair[1]);
        } else {
            elementCounts.set(firstElement, pair[1]);
        }
        if (elementCounts.has(secondElement)) {
            elementCounts.set(secondElement, elementCounts.get(secondElement)! + pair[1]);
        } else {
            elementCounts.set(secondElement, pair[1]);
        }
    }

    let counts = Array.from(elementCounts.values()).map((value) => Math.ceil(value / 2.0));
    return Math.max(...counts) - Math.min(...counts);
}

const polymerData = parseInput();
const template = polymerData[0];
const insertionRules = polymerData[1];

let elementCounts = buildPolymer(template, insertionRules, 10);
const partOneAnswer = countElements(elementCounts);
printAnswer(1, partOneAnswer.toString());

elementCounts = buildPolymer(template, insertionRules, 40);
const partTwoAnswer = countElements(elementCounts);
printAnswer(2, partTwoAnswer.toString());
