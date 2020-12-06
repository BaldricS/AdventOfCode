using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2015, 5)]
    public static class Day5_2015
    {
        public static bool IsNice(string line)
        {
            var vowels = new Regex(@"[aiueo].*[aiueo].*[aiueo]");
            var doubleLetter = new Regex(@"(\w)\1");
            var evilPairs = new Regex(@"ab|cd|pq|xy");

            return vowels.IsMatch(line) && doubleLetter.IsMatch(line) && !evilPairs.IsMatch(line);
        }

        public static bool IsActuallyNice(string line)
        {
            var doublePair = new Regex(@"(\w\w).*\1");
            var huggingLetters = new Regex(@"(\w).\1");

            return doublePair.IsMatch(line) && huggingLetters.IsMatch(line);
        }

        [Solver(1)]
        public static int Solve1(IEnumerable<string> lines) => lines.Count(IsNice);

        [Solver(2)]
        public static int Solve2(IEnumerable<string> lines) => lines.Count(IsActuallyNice);
    }
}
