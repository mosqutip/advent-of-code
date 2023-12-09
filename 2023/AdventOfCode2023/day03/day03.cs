/*
--- Day 3: Gear Ratios ---
You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source, but this is as far as he can bring you. You go inside.

It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.

"Aaah!"

You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone! The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.

The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one. If you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.

The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of numbers and symbols you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)

Here is an example engine schematic:

467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..

In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and 58 (middle right). Every other number is adjacent to a symbol and so is a part number; their sum is 4361.

Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?

Your puzzle answer was 546563.

--- Part Two ---
The engineer finds the missing part and installs it in the engine! As the engine springs to life, you jump in the closest gondola, finally ready to ascend to the water source.

You don't seem to be going very fast, though. Maybe something is still wrong? Fortunately, the gondola has a phone labeled "help", so you pick it up and the engineer answers.

Before you can explain the situation, she suggests that you look out the window. There stands the engineer, holding a phone in one hand and waving with the other. You're going so slowly that you haven't even left the station. You exit the gondola.

The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.

This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out which gear needs to be replaced.

Consider the same engine schematic again:

467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..

In this schematic, there are two gears. The first is in the top left; it has part numbers 467 and 35, so its gear ratio is 16345. The second gear is in the lower right; its gear ratio is 451490. (The * adjacent to 617 is not a gear because it is only adjacent to one part number.) Adding up all of the gear ratios produces 467835.

What is the sum of all of the gear ratios in your engine schematic?

Your puzzle answer was 91031374.
*/

using AdventOfCode2023;

struct PartNumber
{
    public int lineNumber;
    public int startColumn;
    public int length;

    public int number;
}

class Day03()
{
    private static List<string> inputFileLines = Utilites.ReadInputFile("03");

    private static List<PartNumber> partNumbers = new List<PartNumber>();

    private static int[,] symbolMap;

    private static void parseSchematic()
    {
        symbolMap = new int[inputFileLines.Count, inputFileLines.First().Count()];
        for (int i = 0; i < inputFileLines.Count(); i++)
        {
            var line = inputFileLines[i];
            bool isNumber = false;
            int startColumn = -1;
            int length = 0;

            for (int j = 0; j < line.Length; j++)
            {
                if (isNumber)
                {
                    if (line[j] < '0' || line[j] > '9')
                    {
                        isNumber = false;
                        int number = int.Parse(line.Substring(startColumn, length));
                        partNumbers.Add(new PartNumber { lineNumber = i, startColumn = startColumn, length = length, number = number });

                        if (line[j] == '*')
                        {
                            symbolMap[i, j] = 2;
                        }
                        else if (line[j] != '.')
                        {
                            symbolMap[i, j] = 1;
                        }
                    }
                    else
                    {
                        length++;
                    }
                }
                else
                {
                    if (line[j] >= '0' && line[j] <= '9')
                    {
                        isNumber = true;
                        startColumn = j;
                        length = 1;
                    }
                    else
                    {
                        if (line[j] == '*')
                        {
                            symbolMap[i, j] = 2;
                        }
                        else if (line[j] != '.')
                        {
                            symbolMap[i, j] = 1;
                        }
                    }
                }
            }

            // Special case to handle numbers that abut the right-hand boundary of the schematic
            if (isNumber)
            {
                int number = int.Parse(line.Substring(startColumn, length));
                partNumbers.Add(new PartNumber { lineNumber = i, startColumn = startColumn, length = length, number = number });
            }
        }
    }

    private static bool isPartNumber(PartNumber partNumber)
    {
        int startLine = partNumber.lineNumber - 1;
        int endLine = startLine + 2;
        int startColumn = partNumber.startColumn - 1;
        int endColumn = startColumn + partNumber.length + 1;

        for (int i = startLine; i <= endLine && i < symbolMap.GetLength(0); i++)
        {
            if (i < 0)
            {
                continue;
            }
            for (int j = startColumn; j <= endColumn && j < symbolMap.GetLength(1); j++)
            {
                if (j < 0)
                {
                    continue;
                }

                if (symbolMap[i, j] > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static void findGearRatios(PartNumber partNumber, Dictionary<string, List<int>> gearRatios)
    {
        int startLine = partNumber.lineNumber - 1;
        int endLine = startLine + 2;
        int startColumn = partNumber.startColumn - 1;
        int endColumn = startColumn + partNumber.length + 1;

        for (int i = startLine; i <= endLine && i < symbolMap.GetLength(0); i++)
        {
            if (i < 0)
            {
                continue;
            }
            for (int j = startColumn; j <= endColumn && j < symbolMap.GetLength(1); j++)
            {
                if (j < 0)
                {
                    continue;
                }

                if (symbolMap[i, j] == 2)
                {
                    var coord = $"{i},{j}";
                    if (gearRatios.ContainsKey(coord))
                    {
                        gearRatios[coord].Add(partNumber.number);
                    }
                    else
                    {
                        gearRatios[coord] = new List<int> { partNumber.number };
                    }

                    return;
                }
            }
        }
    }

    private static int solvePartOne()
    {
        int sum = 0;
        foreach (var partNumber in partNumbers)
        {
            if (isPartNumber(partNumber))
            {
                sum += partNumber.number;
            }
        }

        return sum;
    }

    private static int solvePartTwo()
    {
        Dictionary<string, List<int>> gearRatios = new Dictionary<string, List<int>>();
        foreach (var partNumber in partNumbers)
        {
            findGearRatios(partNumber, gearRatios);
        }

        int sum = 0;
        foreach (var gearRatio in gearRatios)
        {
            if (gearRatio.Value.Count == 2)
            {
                sum += gearRatio.Value[0] * gearRatio.Value[1];
            }
        }

        return sum;
    }

    public static void solve()
    {
        parseSchematic();
        Console.WriteLine($"Part 1: {solvePartOne()}");
        Console.WriteLine($"Part 2: {solvePartTwo()}");
    }
}