using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AOC
{
    public record Input222422()
    {
        public List<List<long>> Numbers { get; set; }
    }

    [AdventOfCode(2024, 22)]
    public static class Day22_2024
    {
        [MapInput]
        public static Input222422 Map(string[] lines)
        {
            return new Input222422(){
                Numbers = lines.Select(long.Parse).Select(l => new List<long>() { l }).ToList()
            };
        }

        [Solver(1)]
        public static long Solve1(Input222422 input)
        {
            for (int i = 0; i < 2000; ++i)
            {
                for (int n = 0; n < input.Numbers.Count; ++n)
                {
                    input.Numbers[n][0] = MixAndPrune(input.Numbers[n][0]);
                }
            }

            return input.Numbers.Select(n => n.First()).Sum();
        }

        public static long MixAndPrune(long secret)
        {
            const long pruneMask = (1 << 24) - 1;

            secret = ((secret << 6) ^ secret) & pruneMask;
            secret = ((secret >> 5) ^ secret) & pruneMask;
            secret = ((secret << 11) ^ secret) & pruneMask;

            return secret;
        }

        [Solver(2)]
        public static long Solve2(Input222422 input)
        {
            for (int i = 0; i < 2000; ++i)
            {
                for (int n = 0; n < input.Numbers.Count; ++n)
                {
                    input.Numbers[n].Add(MixAndPrune(input.Numbers[n][^1]));
                }
            }

            for (int m = 0; m < input.Numbers.Count; ++m)
            {
                input.Numbers[m] = input.Numbers[m].Select(n => n % 10).ToList();
            }

            List<Dictionary<(long, long, long, long), long>> bananas = [];

            foreach (var m in input.Numbers)
            {
                Dictionary<(long, long, long, long), long> merchantDictionary = [];
                for (int b = 4; b < m.Count; ++b)
                {
                    var key = (m[b - 3] - m[b - 4], m[b - 2] - m[b - 3], m[b - 1] - m[b - 2], m[b] - m[b - 1]);
                    if (!merchantDictionary.ContainsKey(key))
                    {
                        merchantDictionary[key] = m[b];
                    }
                }
                bananas.Add(merchantDictionary);
            }

            return bananas.SelectMany(d => d).GroupBy(items => items.Key, items => items.Value).Select(items => items.Sum()).Max();
        }
    }
}