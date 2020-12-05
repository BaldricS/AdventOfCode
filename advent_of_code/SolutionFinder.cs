using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AOC
{
    public class SolutionFinder
    {
        private readonly List<Solution> _solutions;

        public SolutionFinder()
        {
            _solutions = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Select(t => new
                {
                    Type = t,
                    Attribute = t.GetCustomAttribute<AdventOfCodeAttribute>()
                })
                .Where(t => t.Attribute != null)
                .SelectMany(t => TypeToSolutions(t.Type, t.Attribute))
                .ToList();
        }

        public Solution Find(int year, int day, int puzzle) =>
            _solutions
                .FirstOrDefault(sol => sol.Year == year && sol.Day == day && sol.Puzzle == puzzle);

        private static Solution[] TypeToSolutions(Type t, AdventOfCodeAttribute aoc)
        {
            var methods = t.GetMethods();

            var mapFunc = methods
                .FirstOrDefault(m => m.GetCustomAttribute<MapInputAttribute>() != null);

            return methods
                .Select(m => new
                {
                    Method = m,
                    Attribute = m.GetCustomAttribute<SolverAttribute>()
                })
                .Where(m => m.Attribute != null)
                .OrderBy(m => m.Attribute.Puzzle)
                .Select(t => new Solution(
                    mapFunc,
                    t.Method,
                    aoc.Year,
                    aoc.Day,
                    t.Attribute.Puzzle
                ))
                .ToArray();
        }
    }
}
