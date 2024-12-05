using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AOC
{
    public static class SolutionRunner
    {
        public static RunResult Go(Solution solution)
        {
            var sw = new Stopwatch();
            sw.Start();
            var input = GetInput(solution.Year, solution.Day, solution.MapFunc, ExpectsSingleInput(solution.Solver));
            sw.Stop();

            var inputFetchMs = sw.ElapsedMilliseconds;

            sw.Reset();
            sw.Start();
            var result = solution.Solver.Invoke(null, [input]);
            sw.Stop();

            return new RunResult(
                result,
                inputFetchMs,
                sw.ElapsedMilliseconds
            );
        }

        private static bool ExpectsSingleInput(MethodInfo solverFunc) =>
            ExpectsSingleInput(solverFunc.GetParameters()[0].ParameterType);

        private static bool ExpectsSingleInput(Type t) =>
            t switch
            {
                IDictionary => true,
                _ when t.IsPrimitive => true,
                _ when t.Name == typeof(string).Name => true,
                _ when t.IsArray => false,
                _ when t.IsAssignableTo(typeof(IEnumerable)) => false,
                _ => true
            };


        private static object GetInput(int year, int day, MethodInfo mapFunc, bool firstLineOnly)
        {
            var lines = GetInput(year, day);
            if (mapFunc == null)
            {
                return firstLineOnly ? lines.First() : lines;
            }

            return mapFunc.Invoke(null, [lines]);
        }

        private static string[] GetInput(int year, int day) =>
            File.ReadAllLines(Path.Join("inputs", $"{year}", $"{day}"));
    }
}
