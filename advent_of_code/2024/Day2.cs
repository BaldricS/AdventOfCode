using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2024, 2)]
    public static class Day2_2024
    {
        [MapInput]
        public static IEnumerable<int[]> Map(string[] lines) => lines.Select(l => l.Split(" ").Select(int.Parse).ToArray());

        [Solver(1)]
        public static int Solve1(IEnumerable<int[]> nums)
        {
            return nums.Where(ns => Check(Diffs(ns))).Count();
        }

        [Solver(2)]
        public static int Solve2(IEnumerable<int[]> nums)
        {
            return nums.Where(ns => {
                if (Check(Diffs(ns))) {
                    return true;
                }

                for (int i = 0; i < ns.Length; ++i) {
                    var slice = ns.Take(i).Concat(ns.Skip(i + 1)).ToArray();
                    if (Check(Diffs(slice))) {
                        return true;
                    }
                }

                return false;
            }).Count();
        }

        private static int[] Diffs(int[] input) {
            return input.Zip(input.Skip(1)).Select(pr => pr.First - pr.Second).ToArray();
        }

        private static bool Check(int[] arrs) {
            return arrs.All(n => n >= 1 &&  n <= 3) || arrs.All(n => n >= -3 && n <= -1);
        }
    }
}