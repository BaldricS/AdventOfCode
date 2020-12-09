using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2020, 9)]
    public static class Day9_2020
    {
        private static readonly int Amount = 25;

        [MapInput]
        public static IEnumerable<long> Map(string[] input) => input.Select(long.Parse);

        public static (int, long) FindMismatch(long[] input)
        {
            var ints = input.ToArray();
            var pool = ints.Take(Amount).ToHashSet();

            for (int i = Amount; i < ints.Length; ++i)
            {
                var candidate = ints[i];
                if (!pool.Any(n => pool.Contains(candidate - n)))
                {
                    return (i, candidate);
                }

                pool.Add(ints[i]);
                pool.Remove(ints[i - Amount]);
            }

            return (-1, 0);
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<long> input) => FindMismatch(input.ToArray()).Item2;

        [Solver(2)]
        public static long Solve2(IEnumerable<long> input)
        {
            var ints = input.ToArray();
            var (head, candidate) = FindMismatch(ints);

            --head;
            long sum = 0;
            for (int tail = head; tail >= 0; --tail)
            {
                sum += ints[tail];
                if (sum == candidate)
                {
                    var slice = ints.Skip(tail).Take(head - tail).ToArray();
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
