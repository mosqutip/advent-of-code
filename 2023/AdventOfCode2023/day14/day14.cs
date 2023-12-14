/*
--- Day 14: Parabolic Reflector Dish ---

You reach the place where all of the mirrors were pointing: a massive parabolic reflector dish attached to the side of another large mountain.

The dish is made up of many small mirrors, but while the mirrors themselves are roughly in the shape of a parabolic reflector dish, each individual mirror seems to be pointing in slightly the wrong direction. If the dish is meant to focus light, all it's doing right now is sending it in a vague direction.

This system must be what provides the energy for the lava! If you focus the reflector dish, maybe you can go where it's pointing and use the light to fix the lava production.

Upon closer inspection, the individual mirrors each appear to be connected via an elaborate system of ropes and pulleys to a large metal platform below the dish. The platform is covered in large rocks of various shapes. Depending on their position, the weight of the rocks deforms the platform, and the shape of the platform controls which ropes move and ultimately the focus of the dish.

In short: if you move the rocks, you can focus the dish. The platform even has a control panel on the side that lets you tilt it in one of four directions! The rounded rocks (O) will roll when the platform is tilted, while the cube-shaped rocks (#) will stay in place. You note the positions of all of the empty spaces (.) and rocks (your puzzle input). For example:

O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....

Start by tilting the lever so all of the rocks will slide north as far as they will go:

OOOO.#.O..
OO..#....#
OO..O##..O
O..#.OO...
........#.
..#....#.#
..O..#.O.O
..O.......
#....###..
#....#....

You notice that the support beams along the north side of the platform are damaged; to ensure the platform doesn't collapse, you should calculate the total load on the north support beams.

The amount of load caused by a single rounded rock (O) is equal to the number of rows from the rock to the south edge of the platform, including the row the rock is on. (Cube-shaped rocks (#) don't contribute to load.) So, the amount of load caused by each rock in each row is as follows:

OOOO.#.O.. 10
OO..#....#  9
OO..O##..O  8
O..#.OO...  7
........#.  6
..#....#.#  5
..O..#.O.O  4
..O.......  3
#....###..  2
#....#....  1

The total load is the sum of the load caused by all of the rounded rocks. In this example, the total load is 136.

Tilt the platform so that the rounded rocks all roll north. Afterward, what is the total load on the north support beams?

Your puzzle answer was 112773.

The first half of this puzzle is complete! It provides one gold star: *
--- Part Two ---

The parabolic reflector dish deforms, but not in a way that focuses the beam. To do that, you'll need to move the rocks to the edges of the platform. Fortunately, a button on the side of the control panel labeled "spin cycle" attempts to do just that!

Each cycle tilts the platform four times so that the rounded rocks roll north, then west, then south, then east. After each tilt, the rounded rocks roll as far as they can before the platform tilts in the next direction. After one cycle, the platform will have finished rolling the rounded rocks in those four directions in that order.

Here's what happens in the example above after each of the first few cycles:

After 1 cycle:
.....#....
....#...O#
...OO##...
.OO#......
.....OOO#.
.O#...O#.#
....O#....
......OOOO
#...O###..
#..OO#....

After 2 cycles:
.....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#..OO###..
#.OOO#...O

After 3 cycles:
.....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#...O###.O
#.OOO#...O

This process should work if you leave it running long enough, but you're still worried about the north support beams. To make sure they'll survive for a while, you need to calculate the total load on the north support beams after 1000000000 cycles.

In the above example, after 1000000000 cycles, the total load on the north support beams is 64.

Run the spin cycle for 1000000000 cycles. Afterward, what is the total load on the north support beams?

Your puzzle answer was 98894.
*/

using System.Text;
using AdventOfCode2023;

enum Direction
{
    North = 0,
    West = 1,
    South = 2,
    East = 3
}

class Day14()
{
    private static List<string> inputFileLines = Utilites.ReadInputFile("14");

    private static char[,] platform;

    private static void parsePlatform()
    {
        platform = new char[inputFileLines.Count, inputFileLines[0].Length];
        for (int i = 0; i < inputFileLines.Count; i++)
        {
            for (int j = 0; j < inputFileLines[i].Length; j++)
            {
                platform[i, j] = inputFileLines[i][j];
            }
        }
    }

    private static void printArr(char[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write(array[i, j]);
            }

            Console.WriteLine();
        }
    }

    private static void tiltPlatform(Direction direction)
    {
        if (direction == Direction.North)
        {
            for (int i = 0; i < platform.GetLength(0); i++)
            {
                for (int j = 0; j < platform.GetLength(1); j++)
                {
                    if (platform[i, j] == 'O')
                    {
                        slide(platform, i, j, direction);
                    }
                }
            }
        }
        else if (direction == Direction.West)
        {
            for (int j = 0; j < platform.GetLength(1); j++)
            {
                for (int i = platform.GetLength(0) - 1; i >= 0; i--)
                {
                    if (platform[i, j] == 'O')
                    {
                        slide(platform, i, j, direction);
                    }
                }
            }
        }
        else if (direction == Direction.South)
        {
            for (int i = platform.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < platform.GetLength(1); j++)
                {
                    if (platform[i, j] == 'O')
                    {
                        slide(platform, i, j, direction);
                    }
                }
            }
        }
        else if (direction == Direction.East)
        {
            for (int j = platform.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = 0; i < platform.GetLength(0); i++)
                {
                    if (platform[i, j] == 'O')
                    {
                        slide(platform, i, j, direction);
                    }
                }
            }
        }
    }

    private static void slide(char[,] platform, int i, int j, Direction direction)
    {
        int k, a;
        switch (direction)
        {
            case Direction.North:
                k = i;
                a = i - 1;
                while (a >= 0)
                {
                    if (platform[a, j] == '.')
                    {
                        platform[k, j] = '.';
                        platform[a, j] = 'O';
                        a--;
                        k--;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case Direction.West:
                k = j;
                a = j - 1;
                while (a >= 0)
                {
                    if (platform[i, a] == '.')
                    {
                        platform[i, k] = '.';
                        platform[i, a] = 'O';
                        a--;
                        k--;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case Direction.South:
                k = i;
                a = i + 1;
                while (a < platform.GetLength(0))
                {
                    if (platform[a, j] == '.')
                    {
                        platform[k, j] = '.';
                        platform[a, j] = 'O';
                        a++;
                        k++;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case Direction.East:
                k = j;
                a = j + 1;
                while (a < platform.GetLength(1))
                {
                    if (platform[i, a] == '.')
                    {
                        platform[i, k] = '.';
                        platform[i, a] = 'O';
                        a++;
                        k++;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            default:
                break;
        }
    }

    private static int calculateLoad()
    {
        int totalLoad = 0;
        int rowLoad = 1;
        for (int i = platform.GetLength(0) - 1; i >= 0; i--, rowLoad++)
        {
            for (int j = 0; j < platform.GetLength(1); j++)
            {
                if (platform[i, j] == 'O')
                {
                    totalLoad += rowLoad;
                }
            }
        }

        return totalLoad;
    }

    private static int solvePartOne()
    {
        tiltPlatform(Direction.North);
        return calculateLoad();
    }

    private static int solvePartTwo()
    {
        Dictionary<int, int> platformHashes = new Dictionary<int, int>();
        int cycleLength = 0;
        while (true)
        {
            tiltPlatform(Direction.North);
            tiltPlatform(Direction.West);
            tiltPlatform(Direction.South);
            tiltPlatform(Direction.East);

            StringBuilder platformString = new StringBuilder();
            for (int i = 0; i < platform.GetLength(0); i++)
            {
                for (int j = 0; j < platform.GetLength(1); j++)
                {
                    platformString.Append(platform[i, j]);
                }
            }

            var hash = platformString.ToString().GetHashCode();
            if (platformHashes.ContainsKey(hash))
            {
                platformHashes[hash]++;
                // The first time we see 3, we must be at the beginning of the cycle
                if (platformHashes[hash] == 3)
                {
                    break;
                }

                cycleLength++;
            }
            else
            {
                platformHashes[hash] = 1;
            }
        }

        // We've completed one cycle for each hash, one additional cycle for
        // each hash in the cycle, and one more cycle for the start hash
        int cyclesCompleted = platformHashes.Count + cycleLength + 1;
        int remainingCycles = (1_000_000_000 - cyclesCompleted) % cycleLength;
        while (remainingCycles > 0)
        {
            tiltPlatform(Direction.North);
            tiltPlatform(Direction.West);
            tiltPlatform(Direction.South);
            tiltPlatform(Direction.East);
            remainingCycles--;
        }

        return calculateLoad();
    }

    public static void solve()
    {
        parsePlatform();
        Console.WriteLine($"Part 1: {solvePartOne()}");
        Console.WriteLine($"Part 2: {solvePartTwo()}");
    }
}