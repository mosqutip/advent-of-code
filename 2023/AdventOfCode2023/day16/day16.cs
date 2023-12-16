/*
--- Day 16: The Floor Will Be Lava ---

With the beam of light completely focused somewhere, the reindeer leads you deeper still into the Lava Production Facility. At some point, you realize that the steel facility walls have been replaced with cave, and the doorways are just cave, and the floor is cave, and you're pretty sure this is actually just a giant cave.

Finally, as you approach what must be the heart of the mountain, you see a bright light in a cavern up ahead. There, you discover that the beam of light you so carefully focused is emerging from the cavern wall closest to the facility and pouring all of its energy into a contraption on the opposite side.

Upon closer inspection, the contraption appears to be a flat, two-dimensional square grid containing empty space (.), mirrors (/ and \), and splitters (| and -).

The contraption is aligned so that most of the beam bounces around the grid, but each tile on the grid converts some of the beam's light into heat to melt the rock in the cavern.

You note the layout of the contraption (your puzzle input). For example:

.|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....

The beam enters in the top-left corner from the left and heading to the right. Then, its behavior depends on what it encounters as it moves:

    If the beam encounters empty space (.), it continues in the same direction.
    If the beam encounters a mirror (/ or \), the beam is reflected 90 degrees depending on the angle of the mirror. For instance, a rightward-moving beam that encounters a / mirror would continue upward in the mirror's column, while a rightward-moving beam that encounters a \ mirror would continue downward from the mirror's column.
    If the beam encounters the pointy end of a splitter (| or -), the beam passes through the splitter as if the splitter were empty space. For instance, a rightward-moving beam that encounters a - splitter would continue in the same direction.
    If the beam encounters the flat side of a splitter (| or -), the beam is split into two beams going in each of the two directions the splitter's pointy ends are pointing. For instance, a rightward-moving beam that encounters a | splitter would split into two beams: one that continues upward from the splitter's column and one that continues downward from the splitter's column.

Beams do not interact with other beams; a tile can have many beams passing through it at the same time. A tile is energized if that tile has at least one beam pass through it, reflect in it, or split in it.

In the above example, here is how the beam of light bounces around the contraption:

>|<<<\....
|v-.\^....
.v...|->>>
.v...v^.|.
.v...v^...
.v...v^..\
.v../2\\..
<->-/vv|..
.|<<<2-|.\
.v//.|.v..

Beams are only shown on empty tiles; arrows indicate the direction of the beams. If a tile contains beams moving in multiple directions, the number of distinct directions is shown instead. Here is the same diagram but instead only showing whether a tile is energized (#) or not (.):

######....
.#...#....
.#...#####
.#...##...
.#...##...
.#...##...
.#..####..
########..
.#######..
.#...#.#..

Ultimately, in this example, 46 tiles become energized.

The light isn't energizing enough tiles to produce lava; to debug the contraption, you need to start by analyzing the current situation. With the beam starting in the top-left heading right, how many tiles end up being energized?

Your puzzle answer was 7979.

--- Part Two ---
As you try to work out what might be wrong, the reindeer tugs on your shirt and leads you to a nearby control panel. There, a collection of buttons lets you align the contraption so that the beam enters from any edge tile and heading away from that edge. (You can choose either of two directions for the beam if it starts on a corner; for instance, if the beam starts in the bottom-right corner, it can start heading either left or upward.)

So, the beam could start on any tile in the top row (heading downward), any tile in the bottom row (heading upward), any tile in the leftmost column (heading right), or any tile in the rightmost column (heading left). To produce lava, you need to find the configuration that energizes as many tiles as possible.

In the above example, this can be achieved by starting the beam in the fourth tile from the left in the top row:

.|<2<\....
|v-v\^....
.v.v.|->>>
.v.v.v^.|.
.v.v.v^...
.v.v.v^..\
.v.v/2\\..
<-2-/vv|..
.|<<<2-|.\
.v//.|.v..

Using this configuration, 51 tiles are energized:

.#####....
.#.#.#....
.#.#.#####
.#.#.##...
.#.#.##...
.#.#.##...
.#.#####..
########..
.#######..
.#...#.#..

Find the initial beam configuration that energizes the largest number of tiles; how many tiles are energized in that configuration?

Your puzzle answer was 8437.
*/

using AdventOfCode2023;

class Day16()
{
    private static List<string> inputFileLines = Utilites.ReadInputFile("16");

    private static char[,] layout;

    private static void parseLayout()
    {
        layout = new char[inputFileLines.Count, inputFileLines[0].Length];
        for (int i = 0; i < layout.GetLength(0); i++)
        {
            for (int j = 0; j < layout.GetLength(1); j++)
            {
                layout[i, j] = inputFileLines[i][j];
            }
        }
    }

    private static int[] getNextTile(int[] beam)
    {
        int[] next = [-1, -1, -1];
        if (beam[2] == 0)
        {
            if (beam[1] + 1 < layout.GetLength(1))
            {
                next = [beam[0], beam[1] + 1, beam[2]];
            }
        }
        else if (beam[2] == 1)
        {
            if (beam[0] + 1 < layout.GetLength(0))
            {
                next = [beam[0] + 1, beam[1], beam[2]];
            }
        }
        else if (beam[2] == 2)
        {
            if (beam[1] > 0)
            {
                next = [beam[0], beam[1] - 1, beam[2]];
            }
        }
        else if (beam[2] == 3)
        {
            if (beam[0] > 0)
            {
                next = [beam[0] - 1, beam[1], beam[2]];
            }
        }

        return next;
    }

    private static int getEnergizedTiles(int[] initialBeam)
    {
        char[,] energizedTiles = new char[layout.GetLength(0), layout.GetLength(1)];

        Stack<int[]> beams = new Stack<int[]>();
        beams.Push(initialBeam);

        while (beams.Count > 0)
        {
            var beam = beams.Pop();
            bool terminal = false;
            while (!terminal)
            {
                beam = getNextTile(beam);
                if (beam.Contains(-1))
                {
                    break;
                }

                switch (layout[beam[0], beam[1]])
                {
                    case '.':
                        break;
                    case '|':
                        // loop detection
                        if (energizedTiles[beam[0], beam[1]] == '#')
                        {
                            terminal = true;
                            break;
                        }
                        if (beam[2] == 0 || beam[2] == 2)
                        {
                            beams.Push([beam[0], beam[1], 3]);
                            beam[2] = 1;
                        }
                        break;
                    case '\\':
                        switch (beam[2])
                        {
                            case 0:
                                beam[2] = 1;
                                break;
                            case 1:
                                beam[2] = 0;
                                break;
                            case 2:
                                beam[2] = 3;
                                break;
                            case 3:
                                beam[2] = 2;
                                break;
                        }
                        break;
                    case '/':
                        switch (beam[2])
                        {
                            case 0:
                                beam[2] = 3;
                                break;
                            case 1:
                                beam[2] = 2;
                                break;
                            case 2:
                                beam[2] = 1;
                                break;
                            case 3:
                                beam[2] = 0;
                                break;
                        }
                        break;
                    case '-':
                        // loop detection
                        if (energizedTiles[beam[0], beam[1]] == '#')
                        {
                            terminal = true;
                            break;
                        }
                        if (beam[2] == 3 || beam[2] == 1)
                        {
                            beams.Push([beam[0], beam[1], 0]);
                            beam[2] = 2;
                        }
                        break;
                }

                energizedTiles[beam[0], beam[1]] = '#';
            }
        }

        int sum = 0;
        for (int i = 0; i < energizedTiles.GetLength(0); i++)
        {
            for (int j = 0; j < energizedTiles.GetLength(1); j++)
            {
                if (energizedTiles[i, j] == '#')
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    private static int solvePartOne()
    {
        return getEnergizedTiles([0, -1, 0]);
    }

    private static int solvePartTwo()
    {
        int maxEnergizedTiles = 0;
        for (int i = 0; i < layout.GetLength(0); i++)
        {
            int energy = getEnergizedTiles([i, -1, 0]);
            maxEnergizedTiles = (energy > maxEnergizedTiles) ? energy : maxEnergizedTiles;
        }
        for (int i = 0; i < layout.GetLength(0); i++)
        {
            int energy = getEnergizedTiles([i, layout.GetLength(1), 2]);
            maxEnergizedTiles = (energy > maxEnergizedTiles) ? energy : maxEnergizedTiles;
        }
        for (int i = 0; i < layout.GetLength(1); i++)
        {
            int energy = getEnergizedTiles([-1, i, 1]);
            maxEnergizedTiles = (energy > maxEnergizedTiles) ? energy : maxEnergizedTiles;
        }
        for (int i = 0; i < layout.GetLength(1); i++)
        {
            int energy = getEnergizedTiles([layout.GetLength(0), i, 3]);
            maxEnergizedTiles = (energy > maxEnergizedTiles) ? energy : maxEnergizedTiles;
        }

        return maxEnergizedTiles;
    }

    public static void solve()
    {
        parseLayout();
        Console.WriteLine($"Part 1: {solvePartOne()}");
        Console.WriteLine($"Part 2: {solvePartTwo()}");
    }
}