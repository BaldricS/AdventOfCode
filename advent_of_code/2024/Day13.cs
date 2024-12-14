using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC
{
    public record Input202413(((long, long) A, (long, long) B, (long, long) Prize)[] Machines);

    [AdventOfCode(2024, 13)]
    public static class Day13_2024
    {
        [MapInput]
        public static Input202413 Map(string[] lines)
        {
            var r = new Regex(@"(\d+)");
            var numbers = lines.SelectMany(l => r.Matches(l).Select(m => m.Value)).Select(long.Parse).ToList();
            var list = new List<((long, long), (long, long), (long, long))>();
            for (int i = 0; i < numbers.Count; i += 6)
            {
                list.Add(
                    (
                        (numbers[i], numbers[i + 1]),
                        (numbers[i + 2], numbers[i + 3]),
                        (numbers[i + 4], numbers[i + 5])
                    )
                );
            }

            return new Input202413([.. list]);
        }

        public static long Cost((long, long) muls)
        {
            return muls.Item1 * 3 + muls.Item2;
        }

        public static bool IsValid((long, long) muls, (long, long) a, (long, long) b, (long, long) prize)
        {
            return muls.Item1 * a.Item1 + muls.Item2 * b.Item1 == prize.Item1 && muls.Item1 * a.Item2 + muls.Item2 * b.Item2 == prize.Item2;
        }

        public static IEnumerable<(long, long)> GetFactors(long a, long b, long goal)
        {
            long mostPresses = (goal - b) / a;

            for (long i = 1; i <= mostPresses; ++i)
            {
                long tmp = goal - (i * a);
                double mul = (double)tmp / b;

                if (mul > 100 || i > 100)
                {
                    continue;
                }

                if (mul == (long)mul)
                {
                    yield return (i, (long)mul);
                }
            }
        }

        [Solver(1)]
        public static long Solve1(Input202413 input)
        {
            return input.Machines.Select(m => GetFactors2(m.A, m.B, m.Prize).Where(f => IsValid(f, m.A, m.B, m.Prize)).Select(Cost).DefaultIfEmpty(0).Min()).Sum();
        }


        public static IEnumerable<(long, long)> GetFactors2((long, long) a, (long, long) b, (long, long) prize)
        {
            double xMul = (prize.Item1 * b.Item2 - b.Item1 * prize.Item2) / (b.Item2 * a.Item1 - b.Item1 * a.Item2);

            if (xMul == (long)xMul)
            {
                long yMul = (prize.Item1 - a.Item1 * (long)xMul) / b.Item1;
                yield return ((long)xMul, yMul);
            }
        }

        [Solver(2)]
        public static long Solve2(Input202413 input)
        {
            return input.Machines.Select(m => GetFactors2(m.A, m.B, (m.Prize.Item1 + 10000000000000, m.Prize.Item2 + 10000000000000))
                .Where(f => IsValid(f, m.A, m.B, (m.Prize.Item1 + 10000000000000, m.Prize.Item2 + 10000000000000)))
                .Select(Cost).DefaultIfEmpty(0).Min())
                .Sum();
        }
    }
}