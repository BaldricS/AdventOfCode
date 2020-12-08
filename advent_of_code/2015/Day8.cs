using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = String;

    [AdventOfCode(2015, 8)]
    public static class Day8_2015
    {
        public static long GetCharacters(string line) =>
            Regex.Replace(line, @"\\\\|\\""|\\x[a-f0-9]{2}", "_").Length - 2;

        public static ChallengeType Encode(string line) =>
            $"\"{line.Replace(@"\", @"\\").Replace("\"", @"\""")}\"";
            

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input) =>
            input.Sum(l => l.Length) - input.Sum(GetCharacters);

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input) =>
            input.Select(Encode).Sum(l => l.Length) - input.Sum(l => l.Length);
    }
}
