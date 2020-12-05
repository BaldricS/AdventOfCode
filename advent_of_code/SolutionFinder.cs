using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AOC
{
    public struct Solution
    {
        public MethodInfo MapFunc;
        public MethodInfo Solver;
    }

    public class SolutionFinder
    {

        private readonly Dictionary<int, List<Solution[]>> _solutions;

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
                .OrderBy(t => t.Attribute.Year)
                .ToLookup(t => t.Attribute.Year, t => t)
                .ToDictionary(
                    t => t.Key,
                    t => t
                        .OrderBy(t => t.Attribute.Day)
                        .Select(t => TypeToSolutions(t.Type))
                        .ToList()
                );
        }

        public Solution? FindSolution(int year, int day, int puzzle)
        {
            if (!_solutions.ContainsKey(year))
            {
                return null;
            }

            --day;
            --puzzle;

            var solutionsInYear = _solutions[year];
            if (solutionsInYear.Count >= day)
            {
                return null;
            }

            var solutionsForDay = solutionsInYear[day];
            if (solutionsForDay.Length >= puzzle)
            {
                return null;
            }

            return solutionsForDay[puzzle];
        }

        private static Solution[] TypeToSolutions(Type t)
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
                .Select(t => new Solution
                {
                    MapFunc = mapFunc,
                    Solver = t.Method
                })
                .ToArray();
        }
    }
}
