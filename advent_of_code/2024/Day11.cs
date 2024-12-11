using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AOC
{
    public record Input202411(List<long> Lines);

    [AdventOfCode(2024, 11)]
    public static class Day11_2024
    {
        [MapInput]
        public static Input202411 Map(string[] lines)
        {
            return new(lines.First().Split(' ').Select(long.Parse).ToList());
        }

        [Solver(1)]
        public static long Solve1(Input202411 input)
        {
            Dictionary<int, Dictionary<long, long>> cache = [];
            return input.Lines.Select(s => CountForStone(s, 0, 25, cache)).Sum();
        }

        [Solver(2)]
        public static long Solve2(Input202411 input)
        {
            Dictionary<int, Dictionary<long, long>> cache = [];
            return input.Lines.Select(s => CountForStone(s, 0, 75, cache)).Sum();
        }

        public static long CountForStone(long startingStone, int depth, int max, Dictionary<int, Dictionary<long, long>> cache)
        {
            if (cache.TryGetValue(depth, out var count) && count.TryGetValue(startingStone, out var nStones))
            {
                return nStones;
            }

            if (depth == max)
            {
                cache[depth] = new() {
                    [startingStone] = 1
                };

                return 1;
            }

            long result;
            if (startingStone == 0)
            {
                result = CountForStone(1, depth + 1, max, cache);
            }
            else
            {
                string digits = $"{startingStone}";
                if (digits.Length % 2 == 0)
                {
                    int half = digits.Length / 2;
                    result = CountForStone(long.Parse(digits[..half]), depth + 1, max, cache) + CountForStone(long.Parse(digits[half..]), depth + 1, max, cache);
                }
                else
                {
                    result = CountForStone(startingStone * 2024, depth + 1, max, cache);
                }
            }

            if (cache.TryGetValue(depth, out var updateCount))
            {
                updateCount.Add(startingStone, result);
            }
            else
            {
                cache[depth] = new() {
                    [startingStone] = result
                };
            }

            return result;
        }
    }
}