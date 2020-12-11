using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2020, 10)]
    public static class Day10_2020
    {
        [MapInput]
        public static IEnumerable<int> Map(string[] lines) => lines.Select(int.Parse);
        

        [Solver(1)]
        public static long Solve1(IEnumerable<int> input)
        {
            var jolts = input.ToSortedList().Prepend(0);
            jolts = jolts.Append(jolts.Last() + 3);

            var diffOf1 = jolts.Pair().Where(j => j.Second - j.First == 1).Count();
            var diffOf2 = jolts.Pair().Where(j => j.Second - j.First == 3).Count();

            return diffOf1 * diffOf2;
        }



        public static long PathsToEnd(List<int> jolts, int prev, int curr)
        {
            var validDiff = jolts[curr] - prev <= 3;
            if (!validDiff)
            {
                return 0;
            }
            else if (jolts.Count == curr + 1)
            {
                return 1;
            }

            return PathsToEnd(jolts, prev, curr + 1) + PathsToEnd(jolts, jolts[curr], curr + 1);
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<int> input)
        {
            List<int> sortedList = input.Append(0).ToSortedList();
            sortedList = sortedList.Append(sortedList.Last() + 3).ToList();

            var sublists = new List<List<int>> { new List<int>() };

            for (int i = 0; i < sortedList.Count - 1; ++i)
            {
                var curr = sortedList[i];
                var next = sortedList[i + 1];

                sublists.Last().Add(curr);
                if (next - curr == 3)
                {
                    sublists.Add(new List<int>());
                }
            }

            return sublists
                .Where(l => l.Count > 2)
                .Select(l => PathsToEnd(l.Skip(1).ToList(), l.First(), 0))
                .Where(s => s > 0)
                .Aggregate((a, b) => a * b);
        }
    }
}
