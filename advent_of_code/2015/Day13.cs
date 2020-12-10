using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Dictionary<string, Dictionary<string, int>>;

    [AdventOfCode(2015, 13)]
    public static class Template
    {
        [MapInput]
        public static ChallengeType Map(string[] lines)
        {
            var result = new ChallengeType();

            foreach (var l in lines)
            {
                var data = Regex.Match(l, @"^(\w+) .* (gain|lose) (\d+) .* (\w+).");

                var person = data.Get(1);
                var target = data.Get(4);
                var pos = data.Get(2) == "gain";
                var amount = (pos ? 1 : -1) * data.Get(3).AsInt();

                var stats = result.GetValueOrDefault(person) ?? new Dictionary<string, int>();

                stats[target] = amount;
                result[person] = stats;
            }

            return result;
        }

        public static List<List<string>> GeneratePermutations(List<string> items)
        {
            if (items.Count == 1)
            {
                return new List<List<string>>
                {
                    items.ToList()
                };
            }

            var result = new List<List<string>>();

            foreach (var item in items)
            {
                var without = items.Where(i => item != i).ToList();
                var childPerms = GeneratePermutations(without);

                result.AddRange(childPerms.Select(p => p.Prepend(item).ToList()));
            }

            return result;
        }

        public static int ScoreArrangement(List<string> seating, ChallengeType scores)
        {
            int sum = 0;

            for (int i = 0; i < seating.Count - 1; ++i)
            {
                var left = seating[i];
                var right = seating[i + 1];

                sum += scores[left][right];
                sum += scores[right][left];
            }

            return sum + scores[seating.First()][seating.Last()] + scores[seating.Last()][seating.First()];
        }

        [Solver(1)]
        public static long Solve1(ChallengeType input)
        {
            var permutations = GeneratePermutations(input.Keys.ToList());

            return permutations.Select(p => ScoreArrangement(p, input)).Max();
        }

        [Solver(2)]
        public static long Solve2(ChallengeType input)
        {
            foreach (var scores in input.Values)
            {
                scores["scott"] = 0;
            }

            input["scott"] = input.Keys.ToDictionary(k => k, _ => 0);

            return GeneratePermutations(input.Keys.ToList()).Select(p => ScoreArrangement(p, input)).Max();
        }
    }
}
