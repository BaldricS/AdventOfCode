using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AOC
{
    public static class SolutionRunner
    {
        public static RunResult Go(Solution solution)
        {
            var sw = new Stopwatch();
            sw.Start();
            var input = GetInput(solution.Year, solution.Day, solution.MapFunc);
            sw.Stop();

            var inputFetchMs = sw.ElapsedMilliseconds;

            sw.Reset();
            sw.Start();
            var result = solution.Solver.Invoke(null, new[] { input });
            sw.Stop();

            return new RunResult
            {
                Value = result,
                InputTimeEllapsedMs = inputFetchMs,
                SolveTimeEllapsedMs = sw.ElapsedMilliseconds
            };
        }

        private static object GetInput(int year, int day, MethodInfo mapFunc)
        {
            var lines = GetInput(year, day);
            if (mapFunc == null)
            {
                return lines;
            }

            return mapFunc.Invoke(null, new[] { lines });
        }

        private static string[] GetInput(int year, int day) =>
            File.ReadAllLines(Path.Join("inputs", $"{year}", $"{day}"));
    }
}
