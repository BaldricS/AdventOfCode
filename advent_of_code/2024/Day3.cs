using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2024, 3)]
    public static class Day3_2024
    {
        [MapInput]
        public static IEnumerable<string> Map(string[] lines) => [string.Join("", lines)];

        [Solver(1)]
        public static int Solve1(IEnumerable<string> mem) =>
            new Regex(@"mul\((\d+),(\d+)\)").Matches(mem.First()).Select(mg => int.Parse(mg.Groups[1].Value) * int.Parse(mg.Groups[2].Value)).Sum();

        [Solver(2)]
        public static int Solve2(IEnumerable<string> mem)
        {
            bool enabled = true;
            var sum = 0;
            var tokens = new Regex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");
            var matches = tokens.Matches(mem.First());

            foreach(Match token in matches) {
                if (token.Groups[0].Value == "do()") {
                    enabled = true;
                } else if (token.Groups[0].Value == "don't()") {
                    enabled = false;
                } else if (token.Groups[0].Value.StartsWith("mul(") && enabled) {
                    sum += int.Parse(token.Groups[1].Value) * int.Parse(token.Groups[2].Value);
                }
            }

            return sum;
        }

        private static int[] Diffs(int[] input) {
            return input.Zip(input.Skip(1)).Select(pr => pr.First - pr.Second).ToArray();
        }

        private static bool Check(int[] arrs) {
            return arrs.All(n => n >= 1 &&  n <= 3) || arrs.All(n => n >= -3 && n <= -1);
        }
    }
}