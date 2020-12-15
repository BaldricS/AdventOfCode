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

            if (solution == null)
            {
                Console.WriteLine("Solution not found.");
                return;
            }

            var result = SolutionRunner.Go(solution);

            Console.WriteLine($"  | {"Solution",-15} | {"Solve (ms)",-10} | {"Input (ms)",-10} | {"Total (ms)",-10}");
            Console.WriteLine(new string('-', 58));
            Console.WriteLine($"  | {result.Value,-15} | {result.SolveTimeMs,-10} | {result.InputTimeMs,-10} | {result.TotalTimeMs,-10}");
        }
    }
}
