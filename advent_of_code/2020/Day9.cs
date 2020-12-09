using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2020, 9)]
    public static class Day9_2020
    {
        public static int Amount { get; } = 25;

        [MapInput]
        public static IEnumerable<long> Map(string[] input) => input.Select(long.Parse);

        [Solver(1)]
        public static long Solve1(IEnumerable<long> input)
        {
            var ints = input.ToArray();
            for (int i = Amount; i < ints.Length; ++i)
            {
                var candidate = ints[i];
                var slice = new Span<long>(ints, i - Amount, Amount).ToArray().ToHashSet();

                if (!slice.Any(n => slice.Contains(candidate - n)))
                {
                    return candidate;
                }
            }

            return 1;
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<long> input)
        {
            int head = 0;
            long candidate = 0;
            var ints = input.ToArray();
            for (int i = Amount; i < ints.Length; ++i)
            {
                var c = ints[i];
                var slice = new Span<long>(ints, i - Amount, Amount).ToArray().ToHashSet();

                if (!slice.Any(n => slice.Contains(c - n)))
                {
                    head = i;
                    candidate = c;
                    break;
                }
            }

            --head;
            long sum = 0;
            for (int tail = head; tail >= 0; --tail)
            {
                sum += ints[tail];
                if (sum == candidate)
                {
                    var slice = new Span<long>(ints, tail, head - tail).ToArray();
                    return slice.Min() + slice.Max();
                }
                else if (sum > candidate)
                {
                    sum -= ints[head--];
                }
            }



            return 1;
        }
    }
}
