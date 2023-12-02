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
    }
}