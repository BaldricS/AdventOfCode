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

            Console.WriteLine($"  | {"Solution",-10} | {"Solve (ms)",-10} | {"Input (ms)",-10} | {"Total (ms)",-10}");
            Console.WriteLine(new string('-', 53));
            Console.WriteLine($"  | {result.Value,-10} | {result.SolveTimeEllapsedMs,-10} | {result.InputTimeEllapsedMs,-10} | {result.TotalTimeEllapsedMs,-10}");
        }
    }
}
