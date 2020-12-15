using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2020, 13)]
    public static class Day13_2020
    {
        [MapInput]
        public static (int, IEnumerable<(int, long)>, int) Map(string[] lines)
        {
            var timeToWait = lines.First().AsInt();
            var allBusses = lines.Skip(1).SelectMany(l => l.Split(','));
            var busTimes = allBusses.Select((b, i) => new
            {
                Slot = i,
                Bus = b
            })
            .Where(bs => bs.Bus != "x")
            .Select(bs => (
                bs.Slot,
                long.Parse(bs.Bus)
            ));

            return (timeToWait, busTimes, allBusses.Count());
        }

        [Solver(1)]
        public static long Solve1((int, IEnumerable<(int, long)>, int) input)
        {
            var smallestWait = input.Item2
                .Select(m => new { mul = (int)Math.Ceiling(input.Item1 / (double)m.Item2), id = m.Item2 })
                .OrderBy(b => b.mul * b.id)
                .First();

            return (smallestWait.id * smallestWait.mul - input.Item1) * smallestWait.id;
        }

        public static (long, long) ExtendedGCD(long a, long b)
        {
            (long oldR, long r) = (a, b);
            (long oldS, long s) = (1, 0);
            (long oldT, long t) = (0, 1);

            while (r != 0)
            {
                var quot = oldR / r;

                (oldR, r) = (r, oldR - quot * r);
                (oldS, s) = (s, oldS - quot * s);
                (oldT, t) = (t, oldT - quot * t);
            }

            return (oldS, oldT);
        }

        [Solver(2)]
        public static long Solve2((int, IEnumerable<(int, long)>, int) input)
        {
            var nums = input.Item2.ToArray();
            var totalNums = input.Item3;
            var N = nums.Aggregate(1L, (acc, seed) => acc * seed.Item2);
            var ms = nums.Select(n => N / n.Item2).ToArray();
            var vs = ms.Select((n, i) => ExtendedGCD(nums[i].Item2, n).Item2);
            var es = vs.Zip(ms).Select(pr => pr.First * pr.Second);
            var aS = nums.Select(n => (n.Item2 - n.Item1) % n.Item2);

            return es.Zip(aS).Select(pr => pr.First * pr.Second).Sum() % N;
        }
    }
}
