/*
--- Day 12: Passage Pathing ---

With your submarine's subterranean subsystems subsisting suboptimally, the only way you're getting out of this cave anytime soon is by finding a path yourself. Not just a path - the only way to know if you've found the best path is to find all of them.

Fortunately, the sensors are still mostly working, and so you build a rough map of the remaining caves (your puzzle input). For example:

start-A
start-b
A-c
A-b
b-d
A-end
b-end

This is a list of how all of the caves are connected. You start in the cave named start, and your destination is the cave named end. An entry like b-d means that cave b is connected to cave d - that is, you can move between them.

So, the above cave system looks roughly like this:

    start
    /   \
c--A-----b--d
    \   /
     end

Your goal is to find the number of distinct paths that start at start, end at end, and don't visit small caves more than once. There are two types of caves: big caves (written in uppercase, like A) and small caves (written in lowercase, like b). It would be a waste of time to visit any small cave more than once, but big caves are large enough that it might be worth visiting them multiple times. So, all paths you find should visit small caves at most once, and can visit big caves any number of times.

Given these rules, there are 10 paths through this example cave system:

start,A,b,A,c,A,end
start,A,b,A,end
start,A,b,end
start,A,c,A,b,A,end
start,A,c,A,b,end
start,A,c,A,end
start,A,end
start,b,A,c,A,end
start,b,A,end
start,b,end

(Each line in the above list corresponds to a single path; the caves visited by that path are listed in the order they are visited and separated by commas.)

Note that in this cave system, cave d is never visited by any path: to do so, cave b would need to be visited twice (once on the way to cave d and a second time when returning from cave d), and since cave b is small, this is not allowed.

Here is a slightly larger example:

dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc

The 19 paths through it are as follows:

start,HN,dc,HN,end
start,HN,dc,HN,kj,HN,end
start,HN,dc,end
start,HN,dc,kj,HN,end
start,HN,end
start,HN,kj,HN,dc,HN,end
start,HN,kj,HN,dc,end
start,HN,kj,HN,end
start,HN,kj,dc,HN,end
start,HN,kj,dc,end
start,dc,HN,end
start,dc,HN,kj,HN,end
start,dc,end
start,dc,kj,HN,end
start,kj,HN,dc,HN,end
start,kj,HN,dc,end
start,kj,HN,end
start,kj,dc,HN,end
start,kj,dc,end

Finally, this even larger example has 226 paths through it:

fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW

How many paths through this cave system are there that visit small caves at most once?

Your puzzle answer was 3485.

--- Part Two ---

After reviewing the available paths, you realize you might have time to visit a single small cave twice. Specifically, big caves can be visited any number of times, a single small cave can be visited at most twice, and the remaining small caves can be visited at most once. However, the caves named start and end can only be visited exactly once each: once you leave the start cave, you may not return to it, and once you reach the end cave, the path must end immediately.

Now, the 36 possible paths through the first example above are:

start,A,b,A,b,A,c,A,end
start,A,b,A,b,A,end
start,A,b,A,b,end
start,A,b,A,c,A,b,A,end
start,A,b,A,c,A,b,end
start,A,b,A,c,A,c,A,end
start,A,b,A,c,A,end
start,A,b,A,end
start,A,b,d,b,A,c,A,end
start,A,b,d,b,A,end
start,A,b,d,b,end
start,A,b,end
start,A,c,A,b,A,b,A,end
start,A,c,A,b,A,b,end
start,A,c,A,b,A,c,A,end
start,A,c,A,b,A,end
start,A,c,A,b,d,b,A,end
start,A,c,A,b,d,b,end
start,A,c,A,b,end
start,A,c,A,c,A,b,A,end
start,A,c,A,c,A,b,end
start,A,c,A,c,A,end
start,A,c,A,end
start,A,end
start,b,A,b,A,c,A,end
start,b,A,b,A,end
start,b,A,b,end
start,b,A,c,A,b,A,end
start,b,A,c,A,b,end
start,b,A,c,A,c,A,end
start,b,A,c,A,end
start,b,A,end
start,b,d,b,A,c,A,end
start,b,d,b,A,end
start,b,d,b,end
start,b,end

The slightly larger example above now has 103 paths through it, and the even larger example now has 3509 paths through it.

Given these new rules, how many paths through this cave system are there?

Your puzzle answer was 85062.
*/

import { printAnswer, readInputFile } from '../common/utilities';

const inputLines = readInputFile(__dirname);

function createGraph() {
    let graph = new Map<string, Array<string>>();
    for (let line of inputLines) {
        let nodes = line.split('-');
        if (graph.has(nodes[0])) {
            graph.get(nodes[0])?.push(nodes[1]);
        } else {
            graph.set(nodes[0], [nodes[1]]);
        }

        if (nodes[0] != 'start' && nodes[1] != 'end') {
            if (graph.has(nodes[1])) {
                graph.get(nodes[1])?.push(nodes[0]);
            } else {
                graph.set(nodes[1], [nodes[0]]);
            }
        }
    }

    return graph;
}

function createVisitedMap(graph: Map<string, Array<string>>) {
    let visited = new Map<string, number>();
    for (let key of graph.keys()) {
        if (key.toLowerCase() == key) {
            visited.set(key, 1);
        } else {
            visited.set(key, -1);
        }
    }

    return visited;
}

function getAllPaths(graph: Map<string, Array<string>>, visited: Map<string, number>) {
    let node: string = 'start';
    let path: Array<string> = ['start'];
    visited.set('start', 0);
    let allPaths = findAllPaths(graph, node, visited, path, []);
    return allPaths.map((path) => path.join(','));
}

function findAllPaths(
    graph: Map<string, Array<string>>,
    node: string,
    visited: Map<string, number>,
    path: Array<string>,
    paths: Array<Array<string>>
) {
    if (node == 'end') {
        let curPath = [];
        for (let node of path) {
            curPath.push(node);
        }

        paths.push(curPath);
        return paths;
    }

    for (let connection of graph.get(node)!) {
        let isLargeCave = connection.toLowerCase() != connection;
        if (isLargeCave || visited.get(connection) != 0) {
            if (!isLargeCave) {
                visited.set(connection, visited.get(connection)! - 1);
            }
            path.push(connection);
            findAllPaths(graph, connection, visited, path, paths);
            path.pop();
            visited.set(connection, visited.get(connection)! + 1);
        }
    }

    return paths;
}

function countAllPathsWithExtraTime(graph: Map<string, Array<string>>, visited: Map<string, number>) {
    let smallCaves = [];
    for (let key of graph.keys()) {
        if (key != 'start' && key.toLowerCase() == key) {
            smallCaves.push(key);
        }
    }

    let allPaths: Set<string> = new Set<string>();
    for (let smallCave of smallCaves) {
        visited.set(smallCave, 2);
        for (let path of getAllPaths(graph, visited)) {
            allPaths.add(path);
        }
        visited.set(smallCave, 1);
    }

    return allPaths.size;
}

const graph = createGraph();
let visited = createVisitedMap(graph);
const partOneAnswer = getAllPaths(graph, visited).length;
printAnswer(1, partOneAnswer.toString());

visited = createVisitedMap(graph);
const partTwoAnswer = countAllPathsWithExtraTime(graph, visited);
printAnswer(2, partTwoAnswer.toString());
