using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Dictionary<string, Dictionary<string, int>>;

    [AdventOfCode(2015, 13)]
    public static class Template
    {
        public static (string Person, string Target, int Happiness) ToData(Match m) =>
            (m.Get(1), m.Get(4), (m.Get(2) == "gain" ? 1 : -1) * m.Get(3).AsInt());

        [MapInput]
        public static ChallengeType Map(string[] lines) =>
            lines
                .Select(l => Regex.Match(l, @"^(\w+) .* (gain|lose) (\d+) .* (\w+)."))
                .Select(ToData)
                .GroupBy(d => d.Person, d => d)
                .ToDictionary(d => d.Key, d => d.ToDictionary(p => p.Target, p => p.Happiness));

        public static int ScoreArrangement(List<string> seating, ChallengeType scores) =>
            seating
                .PairWithWrap()
                .Sum(pair => scores[pair.First][pair.Second] + scores[pair.Second][pair.First]);

        public static int TopScore(ChallengeType scores) =>
            scores.Keys
            .Permutations()
            .Select(p => ScoreArrangement(p, scores))
            .Max();

        [Solver(1)]
        public static long Solve1(ChallengeType input) => TopScore(input);

        [Solver(2)]
        public static long Solve2(ChallengeType input)
        {
            foreach (var scores in input.Values)
            {
                scores["scott"] = 0;
            }

            input["scott"] = input.Keys.ToDictionary(k => k, _ => 0);

            return TopScore(input);
        }
    }
}
