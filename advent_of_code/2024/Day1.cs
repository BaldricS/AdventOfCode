using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2024, 1)]
    public static class Day1_2024
    {
        [MapInput]
        public static IEnumerable<int[]> Map(string[] lines) => lines.Select(l => l.Split("   ").Select(int.Parse).ToArray());

        [Solver(1)]
        public static int Solve1(IEnumerable<int[]> nums)
        {
            var left = new List<int>();
            var right = new List<int>();

            foreach (var pair in nums) {
                left.Add(pair[0]);
                right.Add(pair[1]);
            }

            left.Sort();
            right.Sort();
        
            return left.Zip(right).Select(p => Math.Abs(p.First - p.Second)).Sum();
        }

        [Solver(2)]
        public static int Solve2(IEnumerable<int[]> nums)
        {
            var left = new List<int>();
            var right = new List<int>();

            foreach (var pair in nums) {
                left.Add(pair[0]);
                right.Add(pair[1]);
            }
        
            return left.Select(n => n * right.Count(k => k == n)).Sum();
        }
    }
}