using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AOC
{
    class Program
    {
        public static string Identity(string l) => l;

        static void Main(string[] args)
        {
            int year = int.Parse(args[0]);
            int day = int.Parse(args[1]);
            int puzzle = int.Parse(args[2]);

            var solverType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Select(t =>
                {
                    return new
                    {
                        Type = t,
                        Attribute = t.GetCustomAttribute<AdventOfCodeAttribute>()
                    };
                })
                .Where(t => t.Attribute != null)
                .First(t => t.Attribute.Year == year && t.Attribute.Day == day);

            var methods = solverType.Type.GetMethods();
            var mapFunc = methods
                .FirstOrDefault(m => m.GetCustomAttribute<MapInputAttribute>() != null)
                ?? typeof(Program).GetMethod("Identity");

            var solverFunc = methods
                .First(m => (m.GetCustomAttribute<SolverAttribute>()?.Puzzle ?? 0) == puzzle);

            var result = GetSolution(year, day, mapFunc, solverFunc);

            Console.WriteLine($"=== {year} {day} {puzzle} ===");
            Console.WriteLine("Solution:");
            Console.WriteLine(result);
        }

        private static object GetSolution(int year, int day, MethodInfo mapFunc, MethodInfo solverFunc)
        {
            var lines = GetInput(year, day);
            var mapped = lines
                .Select(l => mapFunc.Invoke(null, new[] { l }))
                .ToArray();

            return solverFunc.Invoke(null, new[] { mapped });
        }

        private static string[] GetInput(int year, int day) =>
            File.ReadAllLines(Path.Join("inputs", $"{year}", $"{day}"));
    }
}
