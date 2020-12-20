using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Data;

    public record Rule(int Index, List<string> Value, List<int> First, List<int> Second, HashSet<int> DependsOn, bool IsSelfRef);
    public record Data(List<string> Lines, List<Rule> Rules);

    [AdventOfCode(2020, 19)]
    public static class Day19_2020
    {
        public static List<int> ToInts(string line) => line.Split(" ").Select(int.Parse).ToList();

        public static Rule MakeRule(string line)
        {
            var parts = line.Split(": ");
            var orSplit = parts[1].Split(" | ");
            var isLetter = orSplit[0][0] == '"';

            var index = parts[0].AsInt();
            var first = isLetter ? new List<int>() : ToInts(orSplit[0]);
            var second = orSplit.Length == 2 ? ToInts(orSplit[1]) : new List<int>();
            var depends = first.Concat(second).ToHashSet();

            return new Rule(
                parts[0].AsInt(),
                isLetter ? new List<string> { $"{orSplit[0][1]}" } : new List<string>(),
                first,
                second,
                depends,
                false
            );
        }

		[MapInput]
        public static ChallengeType Map(string[] lines)
        {
            var strings = lines.SkipWhile(l => l != "").Skip(1).ToList();
            var expandedRules = new Dictionary<int, string>();
            var unmatchedRules = new Dictionary<int, string>();

            var rules = lines.TakeWhile(l => l != "");

            return new ChallengeType(
                lines.SkipWhile(l => l != "").Skip(1).ToList(),
                lines.TakeWhile(l => l != "").Select(MakeRule).ToList()
            );
        }

        public static List<string> Permute(List<List<string>> strings)
        {
            if (strings.Count == 0)
            {
                return new List<string>();
            }
            if (strings.Count == 1)
            {
                return strings.First();
            }

            Console.WriteLine(strings.Count);
            var results = new List<string>();
            foreach (var s in strings.First())
            {
                var perms = Permute(strings.Skip(1).ToList());
                results = results.Concat(perms.Select(p => $"{s}{p}")).ToList();
            }

            return results;
        }

        public static List<string> Expand(int index, List<int> items, Dictionary<int, Rule> expanded) =>
            Permute(items.Where(i => i != index).Select(i => expanded[i].Value).ToList());

        public static List<string> Expand2(Rule r, List<int> items, Dictionary<int, Rule> expanded, int depth)
        {
            Console.WriteLine("Blerg");
            var perms = Permute(items.Select(i =>
            {
                if (expanded.ContainsKey(i))
                {
                    return expanded[i].Value;
                }

                return new List<string> { "" };
            })
            .ToList());

            if (!r.IsSelfRef || !items.Contains(r.Index) || depth == 1)
            {
                return perms;
            }

            expanded[r.Index] = r with { Value = perms };
            return Expand2(expanded[r.Index], items, expanded, depth - 1);
        }

        [Solver(1)]
        public static long Solve1(ChallengeType input)
        {
            var expanded = input.Rules.Where(r => r.Value.Any()).ToDictionary(r => r.Index, r => r);
            var toBeExpanded = input.Rules.Where(r => r.Value.Count == 0).ToHashSet();

            while (toBeExpanded.Any())
            {
                var toExpand = new HashSet<Rule>();
                foreach (var rule in toBeExpanded)
                {
                    if (!rule.DependsOn.All(d => expanded.ContainsKey(d)))
                    {
                        toExpand.Add(rule);
                        continue;
                    }

                    var expandedValues = Expand(rule.Index, rule.First, expanded)
                        .Concat(Expand(rule.Index, rule.Second, expanded))
                        .ToList();

                    expanded.Add(rule.Index, rule with { Value = expandedValues });
                }

                toBeExpanded = toExpand;
            }

            return input.Lines.Intersect(expanded[0].Value).Count();
        }

        [Solver(2)]
       public static long Solve2(ChallengeType input)
        {
            var expanded = input.Rules.Where(r => r.Value.Any()).ToDictionary(r => r.Index, r => r);
            var toBeExpanded = input.Rules
                .Where(r => r.Value.Count == 0)
                .Select(r =>
                {
                    if (r.Index == 8)
                    {
                        return r with { Second = new List<int> { 42, 8 }, IsSelfRef = true };
                    }

                    if (r.Index == 11)
                    {
                        return r with { Second = new List<int> { 42, 11, 31 }, IsSelfRef = true };
                    }

                    return r;
                })
                .ToHashSet();

            while (toBeExpanded.Any())
            {
                var toExpand = new HashSet<Rule>();
                foreach (var rule in toBeExpanded)
                {
                    if (!rule.DependsOn.All(d => expanded.ContainsKey(d)))
                    {
                        toExpand.Add(rule);
                        continue;
                    }

                    var expandedValues = Expand(rule.Index, rule.First, expanded)
                        .Concat(Expand2(rule, rule.Second, expanded, 2))
                        .ToList();

                    if (expanded.ContainsKey(rule.Index))
                    {
                        continue;
                    }

                    expanded[rule.Index] = rule with { Value = expandedValues };
                }

                toBeExpanded = toExpand;
            }

            Console.WriteLine("Blarg");

            return input.Lines.Intersect(expanded[0].Value).Count();
        }
    }
}
