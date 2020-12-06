using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2015, 5)]
    public static class Day5_2015
    {
        public static bool IsNice(string line) =>
            Regex.IsMatch(line, @"[aeiou].*[aeiou].*[aeiou]") &&
            Regex.IsMatch(line, @"(\w)\1") &&
            !Regex.IsMatch(line, @"ab|cs|pq|xy");

        public static bool IsActuallyNice(string line) =>
            Regex.IsMatch(line, @"(\w\w).*\1") &&
            Regex.IsMatch(line, @"(\w).\1");

        [Solver(1)]
        public static int Solve1(IEnumerable<string> lines) => lines.Count(IsNice);

        [Solver(2)]
        public static int Solve2(IEnumerable<string> lines) => lines.Count(IsActuallyNice);
    }
}
