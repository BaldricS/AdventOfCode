using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2015, 15)]
    public static class Template
    {
        public static IEnumerable<int[]> GenQuads(int threshhold)
        {
            for (int a = 0; a <= threshhold; ++a)
            {
                for (int b = 0; b <= threshhold - a; ++b)
                {
                    for (int c = 0; c <= threshhold - (a + b); ++c)
                    {
                        int d = threshhold - a - b - c;
                        yield return new[] { a, b, c, d };
                    }
                }
            }
        }

        public static int[] FromMatch(Match m) =>
            new int[] { m.Get(1).AsInt(), m.Get(2).AsInt(), m.Get(3).AsInt(), m.Get(4).AsInt(), m.Get(5).AsInt() };

        [MapInput]
        public static int[][] Map(string[] lines) =>
            lines
                .Select(l => Regex.Match(l, @"(-?\d+), .* (-?\d+), .* (-?\d+), .* (-?\d+), .* (-?\d+)$"))
                .Select(FromMatch)
                .ToArray();

        public static long ScoreRecipe(int[] quad, int[][] ingredient)
        {
            long score = 1;

            for (int i = 0; i < 4; ++i)
            {
                long temp = 0;
                for (int q = 0; q < quad.Length; ++q)
                {
                    temp += quad[q] * ingredient[q][i];
                }

                if (temp <= 0)
                {
                    return 0;
                }

                score *= temp;
            }

            return score;
        }

        public static long CalculateCalories(int[] q, int[][] ingredient)
        {
            int cals = 0;

            for (int i = 0; i < ingredient.Length; ++i)
            {
                cals += ingredient[i].Last() * q[i];
            }

            return cals;
        }

        [Solver(1)]
        public static long Solve1(int[][] ingredients) =>
            GenQuads(100).Max(q => ScoreRecipe(q, ingredients));

        [Solver(2)]
        public static long Solve2(int[][] input) =>
            GenQuads(100)
                .Select(q => (q, CalculateCalories(q, input)))
                .Where(ps => ps.Item2 == 500)
                .Max(ps => ScoreRecipe(ps.q, input));
        
    }
}
