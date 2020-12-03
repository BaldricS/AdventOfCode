using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AOC
{
    public class AdventOfCode
    {
        private readonly int _day;
        private readonly int _puzzle;

        public AdventOfCode(int day, int puzzle)
        {
            _day = day;
            _puzzle = puzzle;
        }

        public void Run<R>(Func<string[], R> solver) => Run((x) => x, solver);

        public void Run<T, R>(Func<string, T> inputMap, Func<T[], R> solver)
        {
            var input = GetInput().Select(inputMap).ToArray();

            var sw = new Stopwatch();

            sw.Start();
            var solution = solver(input);
            sw.Stop();

            Console.WriteLine($"=== Day {_day} Puzzle {_puzzle} ===");
            Console.WriteLine($"Solution: {solution}");
            Console.WriteLine($"Ellapsed Time: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine();
        }

        private string[] GetInput() => File.ReadAllLines("inputs/in.txt");
    }
}
