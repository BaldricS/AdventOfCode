using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2015, 15)]
    public static class Day15_2015
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

        public static int Clamp(int i) => i < 0 ? 0 : i;

        public static long ScoreRecipe(int[] quad, int[][] ingredient) =>
            ingredient
                .Select((_, i) => quad.Select((v, q) => v * ingredient[q][i]).Sum())
                .Aggregate((a, b) => Clamp(a) * Clamp(b));

        public static long CalculateCalories(int[] q, int[][] ingredient) =>
            ingredient.Select((ing, ind) => ing.Last() * q[ind]).Sum();

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
