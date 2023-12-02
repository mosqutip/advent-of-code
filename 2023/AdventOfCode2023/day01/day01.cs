/*
--- Day 1: Trebuchet?! ---
You try to ask why they can't just use a weather machine ("not powerful enough") and where they're even sending you ("the sky") and why your map looks mostly blank ("you sure ask a lot of questions") and hang on did you just say the sky ("of course, where do you think snow comes from") when you realize that the Elves are already loading you into a trebuchet ("please hold still, we need to strap you in").

As they're making the final adjustments, they discover that their calibration document (your puzzle input) has been amended by a very young Elf who was apparently just excited to show off her art skills. Consequently, the Elves are having trouble reading the values on the document.

The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.

For example:

1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet

In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.

Consider your entire calibration document. What is the sum of all of the calibration values?

Your puzzle answer was 56506.

--- Part Two ---
Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".

Equipped with this new information, you now need to find the real first and last digit on each line. For example:

two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen

In this example, the calibration values are 29, 83, 13, 24, 42, 14, and 76. Adding these together produces 281.

What is the sum of all of the calibration values?

Your puzzle answer was 56017.
*/

using AdventOfCode2023;

class Day01()
{
    private static List<string> inputFileLines = Utilites.ReadInputFile("01");

    private static string[] textNumbers =
    {
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
    };

    private static string[] textNumbersReversed =
    {
        "eno", "owt", "eerht", "ruof", "evif", "xis", "neves", "thgie", "enin"
    };

    private static int solvePartOne()
    {
        List<int> calibrationValues = new List<int>(inputFileLines.Count);

        foreach (var line in inputFileLines)
        {
            int firstDigit = -1, secondDigit = -1;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] >= '0' && line[i] <= '9')
                {
                    firstDigit = line[i] - '0';
                    break;
                }
            }
            for (int i = line.Length - 1; i >= 0; i--)
            {
                if (line[i] >= '0' && line[i] <= '9')
                {
                    secondDigit = line[i] - '0';
                    break;
                }
            }

            calibrationValues.Add((firstDigit * 10) + secondDigit);
        }

        return calibrationValues.Sum();
    }

    private static int solvePartTwo()
    {
        List<int> calibrationValues = new List<int>(inputFileLines.Count);

        foreach (var line in inputFileLines)
        {
            int firstDigit = -1;
            bool found = false;
            for (int i = 0; i < line.Length && !found; i++)
            {
                if (line[i] >= '0' && line[i] <= '9')
                {
                    firstDigit = line[i] - '0';
                    found = true;
                }

                for (int j = 0; j < textNumbers.Length && !found; j++)
                {
                    if ((i + textNumbers[j].Length) >= line.Length)
                    {
                        continue;
                    }

                    if (line.Substring(i, textNumbers[j].Length) == textNumbers[j])
                    {
                        firstDigit = j + 1;
                        found = true;
                    }
                }
            }

            string reversedLine = new string(line.Reverse().ToArray());
            int secondDigit = -1;
            found = false;
            for (int i = 0; i < reversedLine.Length && !found; i++)
            {
                if (reversedLine[i] >= '0' && reversedLine[i] <= '9')
                {
                    secondDigit = reversedLine[i] - '0';
                    found = true;
                }

                for (int j = 0; j < textNumbersReversed.Length && !found; j++)
                {
                    if ((i + textNumbersReversed[j].Length) >= reversedLine.Length)
                    {
                        continue;
                    }

                    if (reversedLine.Substring(i, textNumbersReversed[j].Length) == textNumbersReversed[j])
                    {
                        secondDigit = j + 1;
                        found = true;
                    }
                }
            }

            calibrationValues.Add((firstDigit * 10) + secondDigit);
        }

        return calibrationValues.Sum();
    }

    public static void solve()
    {
        Console.WriteLine($"Part 1: {solvePartOne()}");
        Console.WriteLine($"Part 2: {solvePartTwo()}");
    }
}