/*
--- Day 10: Pipe Maze ---

You use the hang glider to ride the hot air from Desert Island all the way up to the floating metal island. This island is surprisingly cold and there definitely aren't any thermals to glide on, so you leave your hang glider behind.

You wander around for a while, but you don't find any people or animals. However, you do occasionally find signposts labeled "Hot Springs" pointing in a seemingly consistent direction; maybe you can find someone at the hot springs and ask them where the desert-machine parts are made.

The landscape here is alien; even the flowers and trees are made of metal. As you stop to admire some metal grass, you notice something metallic scurry away in your peripheral vision and jump into a big pipe! It didn't look like any animal you've ever seen; if you want a better look, you'll need to get ahead of it.

Scanning the area, you discover that the entire field you're standing on is densely packed with pipes; it was hard to tell at first because they're the same metallic silver color as the "ground". You make a quick sketch of all of the surface pipes you can see (your puzzle input).

The pipes are arranged in a two-dimensional grid of tiles:

    | is a vertical pipe connecting north and south.
    - is a horizontal pipe connecting east and west.
    L is a 90-degree bend connecting north and east.
    J is a 90-degree bend connecting north and west.
    7 is a 90-degree bend connecting south and west.
    F is a 90-degree bend connecting south and east.
    . is ground; there is no pipe in this tile.
    S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.

Based on the acoustics of the animal's scurrying, you're confident the pipe that contains the animal is one large, continuous loop.

For example, here is a square loop of pipe:

.....
.F-7.
.|.|.
.L-J.
.....

If the animal had entered this loop in the northwest corner, the sketch would instead look like this:

.....
.S-7.
.|.|.
.L-J.
.....

In the above diagram, the S tile is still a 90-degree F bend: you can tell because of how the adjacent pipes connect to it.

Unfortunately, there are also many pipes that aren't connected to the loop! This sketch shows the same loop as above:

-L|F7
7S-7|
L|7||
-L-J|
L|-JF

In the above diagram, you can still figure out which pipes form the main loop: they're the ones connected to S, pipes those pipes connect to, pipes those pipes connect to, and so on. Every pipe in the main loop connects to its two neighbors (including S, which will have exactly two pipes connecting to it, and which is assumed to connect back to those two pipes).

Here is a sketch that contains a slightly more complex main loop:

..F7.
.FJ|.
SJ.L7
|F--J
LJ...

Here's the same example sketch with the extra, non-main-loop pipe tiles also shown:

7-F7-
.FJ|7
SJLL7
|F--J
LJ.LJ

If you want to get out ahead of the animal, you should find the tile in the loop that is farthest from the starting position. Because the animal is in the pipe, it doesn't make sense to measure this by direct distance. Instead, you need to find the tile that would take the longest number of steps along the loop to reach from the starting point - regardless of which way around the loop the animal went.

In the first example with the square loop:

.....
.S-7.
.|.|.
.L-J.
.....

You can count the distance each tile in the loop is from the starting point like this:

.....
.012.
.1.3.
.234.
.....

In this example, the farthest point from the start is 4 steps away.

Here's the more complex loop again:

..F7.
.FJ|.
SJ.L7
|F--J
LJ...

Here are the distances for each tile on that loop:

..45.
.236.
01.78
14567
23...

Find the single giant loop starting at S. How many steps along the loop does it take to get from the starting position to the point farthest from the starting position?

Your puzzle answer was 6864.

--- Part Two ---
You quickly reach the farthest point of the loop, but the animal never emerges. Maybe its nest is within the area enclosed by the loop?

To determine whether it's even worth taking the time to search for such a nest, you should calculate how many tiles are contained within the loop. For example:

...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........

The above loop encloses merely four tiles - the two pairs of . in the southwest and southeast (marked I below). The middle . tiles (marked O below) are not in the loop. Here is the same loop again with those regions marked:

...........
.S-------7.
.|F-----7|.
.||OOOOO||.
.||OOOOO||.
.|L-7OF-J|.
.|II|O|II|.
.L--JOL--J.
.....O.....

In fact, there doesn't even need to be a full tile path to the outside for tiles to count as outside the loop - squeezing between pipes is also allowed! Here, I is still within the loop and O is still outside the loop:

..........
.S------7.
.|F----7|.
.||OOOO||.
.||OOOO||.
.|L-7F-J|.
.|II||II|.
.L--JL--J.
..........

In both of the above examples, 4 tiles are enclosed by the loop.

Here's a larger example:

.F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...

The above sketch has many random bits of ground, some of which are in the loop (I) and some of which are outside it (O):

OF----7F7F7F7F-7OOOO
O|F--7||||||||FJOOOO
O||OFJ||||||||L7OOOO
FJL7L7LJLJ||LJIL-7OO
L--JOL7IIILJS7F-7L7O
OOOOF-JIIF7FJ|L7L7L7
OOOOL7IF7||L7|IL7L7|
OOOOO|FJLJ|FJ|F7|OLJ
OOOOFJL-7O||O||||OOO
OOOOL---JOLJOLJLJOOO

In this larger example, 8 tiles are enclosed by the loop.

Any tile that isn't part of the main loop can count as being enclosed by the loop. Here's another example with many bits of junk pipe lying around that aren't connected to the main loop at all:

FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJ7F7FJ-
L---JF-JLJ.||-FJLJJ7
|F|F-JF---7F7-L7L|7|
|FFJF7L7F-JF7|JL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L

Here are just the tiles that are enclosed by the loop marked with I:

FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJIF7FJ-
L---JF-JLJIIIIFJLJJ7
|F|F-JF---7IIIL7L|7|
|FFJF7L7F-JF7IIL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L

In this last example, 10 tiles are enclosed by the loop.

Figure out whether you have time to search for the nest by calculating the area within the loop. How many tiles are enclosed by the loop?

Your puzzle answer was 349.
*/

using AdventOfCode2023;

class Day10()
{
    private static List<string> inputFileLines = Utilites.ReadInputFile("10");

    private static List<char[]> pipeMap = new List<char[]>();

    private static List<int[]> pipeMapSteps = new List<int[]>();

    private static Tuple<int, int> sCoords;

    private static void parseMap()
    {
        for (int i = 0; i < inputFileLines.Count; i++)
        {
            var line = inputFileLines[i];
            pipeMap.Add(line.ToCharArray());
            pipeMapSteps.Add(Enumerable.Repeat(-1, line.Length).ToArray());
            for (int j = 0; j < line.Length; j++)
            {
                if (sCoords == null && char.ToUpper(line[j]) == 'S')
                {
                    sCoords = new Tuple<int, int>(i, j);
                }
            }
        }
    }

    private static List<Tuple<int, int, char>> findStartConnections()
    {
        List<Tuple<int, int, char>> pipes = new List<Tuple<int, int, char>>();
        char[] startPipeNeighbors = new char[2];
        int index = 0;

        if (sCoords.Item1 + 1 < pipeMap.Count)
        {
            char pipe = char.ToUpper(pipeMap[sCoords.Item1 + 1][sCoords.Item2]);
            if (pipe == '|' || pipe == 'L' || pipe == 'J')
            {
                startPipeNeighbors[index] = 'U';
                pipes.Add(new Tuple<int, int, char>(sCoords.Item1 + 1, sCoords.Item2, startPipeNeighbors[index]));
                index++;
                pipeMapSteps[sCoords.Item1 + 1][sCoords.Item2] = 1;
            }
        }
        if (sCoords.Item1 - 1 > 0)
        {
            char pipe = char.ToUpper(pipeMap[sCoords.Item1 - 1][sCoords.Item2]);
            if (pipe == '|' || pipe == '7' || pipe == 'F')
            {
                startPipeNeighbors[index] = 'D';
                pipes.Add(new Tuple<int, int, char>(sCoords.Item1 - 1, sCoords.Item2, startPipeNeighbors[index]));
                index++;
                pipeMapSteps[sCoords.Item1 - 1][sCoords.Item2] = 1;
            }
        }
        if (sCoords.Item2 - 1 > 0)
        {
            char pipe = char.ToUpper(pipeMap[sCoords.Item1][sCoords.Item2 - 1]);
            if (pipe == '-' || pipe == 'L' || pipe == 'F')
            {
                startPipeNeighbors[index] = 'R';
                pipes.Add(new Tuple<int, int, char>(sCoords.Item1, sCoords.Item2 - 1, startPipeNeighbors[index]));
                index++;
                pipeMapSteps[sCoords.Item1][sCoords.Item2 - 1] = 1;
            }
        }
        if (sCoords.Item2 + 1 < pipeMap[0].Length)
        {
            char pipe = char.ToUpper(pipeMap[sCoords.Item1][sCoords.Item2 + 1]);
            if (pipe == '-' || pipe == 'J' || pipe == '7')
            {
                startPipeNeighbors[index] = 'L';
                pipes.Add(new Tuple<int, int, char>(sCoords.Item1, sCoords.Item2 + 1, startPipeNeighbors[index]));
                pipeMapSteps[sCoords.Item1][sCoords.Item2 + 1] = 1;
            }
        }

        char startPipe = '.';
        var neighbors = string.Join("", startPipeNeighbors);
        switch (neighbors)
        {
            case "UD":
                startPipe = '|';
                break;
            case "UL":
                startPipe = 'F';
                break;
            case "UR":
                startPipe = '7';
                break;
            case "DL":
                startPipe = 'L';
                break;
            case "DR":
                startPipe = '7';
                break;
            case "LR":
                startPipe = '-';
                break;
            default:
                break;
        }

        pipeMap[sCoords.Item1][sCoords.Item2] = startPipe;
        pipeMapSteps[sCoords.Item1][sCoords.Item2] = 0;

        return pipes;
    }

    private static Tuple<int, int, char> navigatePipes(Tuple<int, int, char> pipe)
    {
        char pipeType = pipeMap[pipe.Item1][pipe.Item2];
        switch (pipe.Item3)
        {
            case 'U':
                if (pipeType == '|')
                {
                    return new Tuple<int, int, char>(pipe.Item1 + 1, pipe.Item2, 'U');
                }
                else if (pipeType == 'L')
                {
                    return new Tuple<int, int, char>(pipe.Item1, pipe.Item2 + 1, 'L');
                }
                else if (pipeType == 'J')
                {
                    return new Tuple<int, int, char>(pipe.Item1, pipe.Item2 - 1, 'R');
                }
                break;
            case 'D':
                if (pipeType == '|')
                {
                    return new Tuple<int, int, char>(pipe.Item1 - 1, pipe.Item2, 'D');
                }
                else if (pipeType == '7')
                {
                    return new Tuple<int, int, char>(pipe.Item1, pipe.Item2 - 1, 'R');
                }
                else if (pipeType == 'F')
                {
                    return new Tuple<int, int, char>(pipe.Item1, pipe.Item2 + 1, 'L');
                }
                break;
            case 'L':
                if (pipeType == '-')
                {
                    return new Tuple<int, int, char>(pipe.Item1, pipe.Item2 + 1, 'L');
                }
                else if (pipeType == 'J')
                {
                    return new Tuple<int, int, char>(pipe.Item1 - 1, pipe.Item2, 'D');
                }
                else if (pipeType == '7')
                {
                    return new Tuple<int, int, char>(pipe.Item1 + 1, pipe.Item2, 'U');
                }
                break;
            case 'R':
                if (pipeType == '-')
                {
                    return new Tuple<int, int, char>(pipe.Item1, pipe.Item2 - 1, 'R');
                }
                else if (pipeType == 'L')
                {
                    return new Tuple<int, int, char>(pipe.Item1 - 1, pipe.Item2, 'D');
                }
                else if (pipeType == 'F')
                {
                    return new Tuple<int, int, char>(pipe.Item1 + 1, pipe.Item2, 'U');
                }
                break;
            default:
                break;
        }

        return pipe;
    }

    private static int solvePartOne()
    {
        List<Tuple<int, int, char>> pipes = findStartConnections();
        int steps = 1;

        while (true)
        {
            pipes[0] = navigatePipes(pipes[0]);
            steps++;
            pipeMapSteps[pipes[0].Item1][pipes[0].Item2] = steps;
            if (pipes[0].Item1 == pipes[1].Item1 && pipes[0].Item2 == pipes[1].Item2)
            {
                return steps - 1;
            }
            pipes[1] = navigatePipes(pipes[1]);
            pipeMapSteps[pipes[1].Item1][pipes[1].Item2] = steps;
            if (pipes[0].Item1 == pipes[1].Item1 && pipes[0].Item2 == pipes[1].Item2)
            {
                return steps;
            }
        }
    }

    private static int solvePartTwo()
    {
        int sum = 0;
        for (int i = 0; i < pipeMap.Count; i++)
        {
            bool isOdd = false;
            for (int j = 0; j < pipeMap[i].Length; j++)
            {
                char pipe = pipeMap[i][j];

                // Within main loop
                if (pipeMapSteps[i][j] != -1)
                {
                    // Always ignore non-vertical pipes in the main loop
                    if (pipe == '-')
                    {
                        continue;
                    }
                    else if (pipe == '|' || pipe == 'J' || pipe == '7')
                    {
                        isOdd = !isOdd;
                    }
                    // FJ and L7 are odd, F7 and LJ are even
                    else if (pipe == 'F' || pipe == 'L')
                    {
                        for (int k = j + 1; k < pipeMap[i].Length; k++)
                        {
                            if (pipeMap[i][k] == '-')
                            {
                                continue;
                            }
                            if (pipe == 'F')
                            {
                                if (pipeMap[i][k] == 'J')
                                {
                                    isOdd = !isOdd;
                                    j = k;
                                    break;
                                }
                                else if (pipeMap[i][k] == '7')
                                {
                                    j = k;
                                    break;
                                }
                            }
                            else if (pipe == 'L')
                            {
                                if (pipeMap[i][k] == '7')
                                {
                                    isOdd = !isOdd;
                                    j = k;
                                    break;
                                }
                                else if (pipeMap[i][k] == 'J')
                                {
                                    j = k;
                                    break;
                                }
                            }
                        }
                    }
                }
                // Not within main loop
                else
                {
                    sum += isOdd ? 1 : 0;
                }
            }
        }

        return sum;
    }

    public static void solve()
    {
        parseMap();
        Console.WriteLine($"Part 1: {solvePartOne()}");
        Console.WriteLine($"Part 2: {solvePartTwo()}");
    }
}