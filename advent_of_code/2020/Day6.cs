using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2020, 6)]
    public static class Day6_2020
    {

        [MapInput]
        public static IEnumerable<List<HashSet<char>>> GetGroups(string[] input) =>
            input.GatherByNewline(s => s.ToHashSet());


        [Solver(1)]
        public static int Solve1(IEnumerable<List<HashSet<char>>> input) =>
            input.Select(l => l.Aggregate((acc, hs) =>
                {
                    acc.UnionWith(hs);
                    return acc;
                }).Count
            ).Sum();

        [Solver(2)]
        public static int Solve2(IEnumerable<List<HashSet<char>>> input) =>
            input.Select(l => l.Aggregate((acc, hs) =>
                {
                    acc.IntersectWith(hs);
                    return acc;
                }).Count
            ).Sum();
    }
}