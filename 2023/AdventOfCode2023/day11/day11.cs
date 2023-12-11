/*
--- Day 11: Cosmic Expansion ---

You continue following signs for "Hot Springs" and eventually come across an observatory. The Elf within turns out to be a researcher studying cosmic expansion using the giant telescope here.

He doesn't know anything about the missing machine parts; he's only visiting for this research project. However, he confirms that the hot springs are the next-closest area likely to have people; he'll even take you straight there once he's done with today's observation analysis.

Maybe you can help him with the analysis to speed things up?

The researcher has collected a bunch of data and compiled the data into a single giant image (your puzzle input). The image includes empty space (.) and galaxies (#). For example:

...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....

The researcher is trying to figure out the sum of the lengths of the shortest path between every pair of galaxies. However, there's a catch: the universe expanded in the time it took the light from those galaxies to reach the observatory.

Due to something involving gravitational effects, only some space expands. In fact, the result is that any rows or columns that contain no galaxies should all actually be twice as big.

In the above example, three columns and two rows contain no galaxies:

   v  v  v
 ...#......
 .......#..
 #.........
>..........<
 ......#...
 .#........
 .........#
>..........<
 .......#..
 #...#.....
   ^  ^  ^

These rows and columns need to be twice as big; the result of cosmic expansion therefore looks like this:

....#........
.........#...
#............
.............
.............
........#....
.#...........
............#
.............
.............
.........#...
#....#.......

Equipped with this expanded universe, the shortest path between every pair of galaxies can be found. It can help to assign every galaxy a unique number:

....1........
.........2...
3............
.............
.............
........4....
.5...........
............6
.............
.............
.........7...
8....9.......

In these 9 galaxies, there are 36 pairs. Only count each pair once; order within the pair doesn't matter. For each pair, find any shortest path between the two galaxies using only steps that move up, down, left, or right exactly one . or # at a time. (The shortest path between two galaxies is allowed to pass through another galaxy.)

For example, here is one of the shortest paths between galaxies 5 and 9:

....1........
.........2...
3............
.............
.............
........4....
.5...........
.##.........6
..##.........
...##........
....##...7...
8....9.......

This path has length 9 because it takes a minimum of nine steps to get from galaxy 5 to galaxy 9 (the eight locations marked # plus the step onto galaxy 9 itself). Here are some other example shortest path lengths:

    Between galaxy 1 and galaxy 7: 15
    Between galaxy 3 and galaxy 6: 17
    Between galaxy 8 and galaxy 9: 5

In this example, after expanding the universe, the sum of the shortest path between all 36 pairs of galaxies is 374.

Expand the universe, then find the length of the shortest path between every pair of galaxies. What is the sum of these lengths?

Your puzzle answer was 10289334.

--- Part Two ---
The galaxies are much older (and thus much farther apart) than the researcher initially estimated.

Now, instead of the expansion you did before, make each empty row or column one million times larger. That is, each empty row should be replaced with 1000000 empty rows, and each empty column should be replaced with 1000000 empty columns.

(In the example above, if each empty row or column were merely 10 times larger, the sum of the shortest paths between every pair of galaxies would be 1030. If each empty row or column were merely 100 times larger, the sum of the shortest paths between every pair of galaxies would be 8410. However, your universe will need to expand far beyond these values.)

Starting with the same initial image, expand the universe according to these new rules, then find the length of the shortest path between every pair of galaxies. What is the sum of these lengths?

*/

using AdventOfCode2023;

class Day11()
{
    private static List<string> inputFileLines = Utilites.ReadInputFile("11");

    private static List<Tuple<int, int>> galaxies = new List<Tuple<int, int>>();

    private static void parseMap(int expansionFactor)
    {
        var galaxyMap = new char[inputFileLines.Count, inputFileLines[0].Length];
        int[] nonEmptyRows = new int[inputFileLines.Count];
        int[] nonEmptyCols = new int[inputFileLines[0].Length];

        for (int i = 0; i < inputFileLines.Count; i++)
        {
            for (int j = 0; j < inputFileLines[i].Length; j++)
            {
                if (inputFileLines[i][j] == '#')
                {
                    nonEmptyRows[i] = 1;
                    nonEmptyCols[j] = 1;
                    galaxyMap[i, j] = '#';
                    galaxies.Add(new Tuple<int, int>(i, j));
                }
                else
                {
                    galaxyMap[i, j] = inputFileLines[i][j];
                }
            }
        }

        for (int g = 0; g < galaxies.Count; g++)
        {
            int emptyRowCount = 0;
            for (int i = 0; i < nonEmptyRows.Length && i < galaxies[g].Item1; i++)
            {
                if (nonEmptyRows[i] == 0)
                {
                    emptyRowCount++;
                }
            }

            int emptyColCount = 0;
            for (int i = 0; i < nonEmptyCols.Length && i < galaxies[g].Item2; i++)
            {
                if (nonEmptyCols[i] == 0)
                {
                    emptyColCount++;
                }
            }

            galaxies[g] = new Tuple<int, int>(
                galaxies[g].Item1 + ((expansionFactor - 1) * emptyRowCount),
                galaxies[g].Item2 + ((expansionFactor - 1) * emptyColCount)
            );
        }
    }

    private static int findShortestPath(Tuple<int, int> startGalaxy, Tuple<int, int> endGalaxy)
    {
        int startX = startGalaxy.Item1;
        int startY = startGalaxy.Item2;
        int endX = endGalaxy.Item1;
        int endY = endGalaxy.Item2;

        return Math.Abs(endX - startX) + Math.Abs(endY - startY);
    }

    private static int solvePartOne()
    {
        int sum = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i; j < galaxies.Count; j++)
            {
                sum += findShortestPath(galaxies[i], galaxies[j]);
            }
        }

        return sum;
    }

    private static long solvePartTwo()
    {
        long sum = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i; j < galaxies.Count; j++)
            {
                sum += findShortestPath(galaxies[i], galaxies[j]);
            }
        }

        return sum;
    }

    public static void solve()
    {
        parseMap(2);
        Console.WriteLine($"Part 1: {solvePartOne()}");

        galaxies.Clear();
        parseMap(1000000);
        Console.WriteLine($"Part 2: {solvePartTwo()}");
    }
}