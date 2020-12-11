using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2020, 10)]
    public static class Day10_2020
    {
        [MapInput]
        public static List<int> Map(string[] lines)
        {
            var items = lines.Select(int.Parse);

            return items.Append(0).Append(items.Max() + 3).ToSortedList();
        }


        [Solver(1)]
        public static long Solve1(List<int> jolts) =>
            jolts.Pair().Where(p => p.Second - p.First == 1).Count() *
            jolts.Pair().Where(p => p.Second - p.First == 3).Count();

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
        public static long Solve2(List<int> jolts)
        {
            var sublists = new List<List<int>> { new List<int>() };

            foreach (var pairs in jolts.Pair())
            {
                sublists.Last().Add(pairs.First);

                if (pairs.Second - pairs.First == 3)
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
