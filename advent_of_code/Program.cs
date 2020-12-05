using System;

namespace AOC
{
    class Program
    {
        static void Main(string[] args)
        {
            int year = int.Parse(args[0]);
            int day = int.Parse(args[1]);
            int puzzle = int.Parse(args[2]);

            var solutions = new SolutionFinder();
            var solution = solutions.Find(year, day, puzzle);

            if (!solution.HasValue)
            {
                Console.WriteLine("Solution not found.");
                return;
            }

            var result = SolutionRunner.Go(solution.Value);

            Console.WriteLine($"===   {year} {day,2} {puzzle,2}   ===");
            Console.WriteLine($"{"Solution",-10} | Time (ms)");
            Console.WriteLine("----------------------");
            Console.WriteLine($"{result.Value,-10}   {result.TimeEllapsedMs}");
        }
    }
}
