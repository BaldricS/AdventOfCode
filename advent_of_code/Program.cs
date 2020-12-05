using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AOC
{
    class Program
    {
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
                .FirstOrDefault(m => m.GetCustomAttribute<MapInputAttribute>() != null);

            var solverFunc = methods
                .First(m => (m.GetCustomAttribute<SolverAttribute>()?.Puzzle ?? 0) == puzzle);

            var input = GetInput(year, day, mapFunc);

            var sw = new Stopwatch();
            sw.Start();
            var solution = solverFunc.Invoke(null, new[] { input });
            sw.Stop();

            Console.WriteLine($"===   {year} {day,2} {puzzle,2}   ===");
            Console.WriteLine($"{"Solution",-10} | Time (ms)");
            Console.WriteLine("----------------------");
            Console.WriteLine($"{solution,-10}   {sw.ElapsedMilliseconds}");
        }

        public static Array GetInput(int year, int day, MethodInfo mapFunc)
        {
            var lines = GetInput(year, day);
            if (mapFunc == null)
            {
                return lines;
            }

            var mapped = Array.CreateInstance(mapFunc.ReturnType, lines.Length);
            var param = new object[] { null };

            for (int i = 0; i < lines.Length; ++i)
            {
                param[0] = lines[i];
                mapped.SetValue(mapFunc.Invoke(null, param), i);
            }

            return mapped;
        }

        private static string[] GetInput(int year, int day) =>
            File.ReadAllLines(Path.Join("inputs", $"{year}", $"{day}"));
    }
}
