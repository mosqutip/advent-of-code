namespace AdventOfCode2023
{
    public class Utilites
    {
        public static List<string> ReadInputFile(string day)
        {
            string workingDirectory = System.AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName ?? string.Empty;
            string inputFilePath = Path.Combine(projectDirectory, $"day{day}", "input.txt");

            return File.ReadLines(inputFilePath).ToList();
        }

        public static void WriteOutputFile(string day, List<string> lines)
        {
            string workingDirectory = System.AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName ?? string.Empty;
            string outputFilePath = Path.Combine(projectDirectory, $"day{day}", "output.txt");

            File.WriteAllLines(outputFilePath, lines);
        }

        public static void Print2DArray<T>(T[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}