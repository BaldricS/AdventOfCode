using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    using ChallengeType = String;

    [AdventOfCode(2015, 10)]
    public static class Day10_2015
    {
        public static string LookAndSay(string word)
        {
            int seen = 1;
            char curr = word[0];
            var result = new List<string>();
            for (int i = 1; i < word.Length; ++i)
            {
                if (word[i] == curr)
                {
                    ++seen;
                }
                else
                {
                    result.Add($"{seen}");
                    result.Add($"{curr}");

                    curr = word[i];
                    seen = 1;
                }
            }

            result.Add($"{seen}");
            result.Add($"{curr}");

            return string.Join("", result);
        }

        [Solver(1)]
        public static long Solve1(ChallengeType seed)
        {
            for (int i = 0; i < 40; ++i)
            {
                seed = LookAndSay(seed);
            }

            return seed.Length;
        }

        [Solver(2)]
        public static long Solve2(ChallengeType seed)
        {
            for (int i = 0; i < 50; ++i)
            {
                seed = LookAndSay(seed);
            }

            return seed.Length;
        }
    }
}
