using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2020, 6)]
    public static class Day6_2020
    {
        [MapInput]
        public static IEnumerable<string> GetGroups(string[] input)
        {
            var groups = new List<string>();
            var currentGroup = "";
            for (int i = 0; i < input.Length; ++i)
            {
                if (input[i].Length == 0)
                {
                    groups.Add(currentGroup);
                    currentGroup = "";
                }
                else
                {
                    currentGroup += " " + input[i];
                }
            }

            if (currentGroup != "")
            {
                groups.Add(currentGroup);
            }

            return groups;
        }


        [Solver(1)]
        public static int Solve1(IEnumerable<string> input) =>
            input.Select(l => l.ToHashSet().Count()).Sum();

        [Solver(2)]
        public static int Solve2(IEnumerable<string> input)
        {
            int count = 0;

            foreach (var group in input)
            {
                var sections = group.Trim().Split(' ').Select(g => g.ToHashSet()).ToArray();
                count += sections[0].Where(g => sections.All(hs => hs.Contains(g))).Count();
            }
            return count;
        }
    }
}