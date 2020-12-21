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
    public record Node(int Index, HashSet<Node> Connected, bool Accepting);
    public record NFA(List<List<HashSet<int>>> Transitions, HashSet<int> StartNodes, List<bool> Accepting);

    [AdventOfCode(2020, 19)]
    public static class Day19_2020
    {
        public static List<int> ToInts(string line) => line.Split(" ").Select(int.Parse).ToList();

        public static Rule MakeRule(string line)
        {
            var parts = line.Split(": ");
            var orSplit = parts[1].Split(" | ");
            var isLetter = orSplit[0][0] == '"';

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

        public static NFA MakeStartingDFA(Rule r)
        {
            var val = r.Value.First();
            var accepting = new List<bool> { false, true };

            var transitions = new List<List<HashSet<int>>>
            {
                new List<HashSet<int>> { new HashSet<int>(), new HashSet<int>() },
                new List<HashSet<int>> { new HashSet<int>(), new HashSet<int>() }
            };

            var index = val[0] - 'a';
            transitions[0][index].Add(1);

            return new NFA(transitions, new HashSet<int> { 0 }, accepting);
        }

        public static NFA ConcatNFAs(NFA left, NFA right)
        {
            var transitionCopy = left.Transitions
                .Select(letters => letters.Select(hs => hs.ToHashSet()).ToList())
                .ToList();

            if (right == null)
            {
                return new NFA(transitionCopy, left.StartNodes.ToHashSet(), left.Accepting.ToList());
            }

            int offset = transitionCopy.Count - 1;

            for (int e = 0; e < left.Accepting.Count; ++e)
            {
                if (!left.Accepting[e])
                {
                    continue;
                }

                foreach (var start in right.StartNodes)
                {
                    for (int l = 0; l < 2; ++l)
                    {
                        transitionCopy[e][l] = transitionCopy[e][l]
                            .Union(right.Transitions[start][l].Select(i => i + offset).ToHashSet())
                            .ToHashSet();
                    }
                }

            }

            var newStartNodes = left.StartNodes.ToHashSet();
            var newAccepting = left.Accepting.Select(_ => false).Concat(right.Accepting.Skip(1)).ToList();
            var updatedRightTransitions = right.Transitions
                .Skip(1)
                .Select(letters => letters.Select(hs => hs.Select(i => i + offset).ToHashSet()).ToList());

            var finalTransitions = transitionCopy.Concat(updatedRightTransitions).ToList();

            return new NFA(finalTransitions, newStartNodes, newAccepting);
        }

        public static NFA OrNFAs(NFA left, NFA right)
        {
            int offset = left.Transitions.Count;

            var allTransitions = left.Transitions
                .Select(letters => letters.Select(hs => hs.ToHashSet()).ToList())
                .Concat(
                    right.Transitions
                        .Select(letters => letters.Select(hs => hs.Select(i => i + offset).ToHashSet()).ToList())
                )
                .ToList();

            var allStarts = left.StartNodes.Union(right.StartNodes.Select(i => i + offset)).ToHashSet();

            var allAccepting = left.Accepting.Concat(right.Accepting).ToList();

            return new NFA(allTransitions, allStarts, allAccepting);
        }

        public static NFA RuleToNFA(Rule r, Dictionary<int, NFA> existing)
        {
            var first = r.First.Select(i => existing[i]).Aggregate(ConcatNFAs);

            if (r.Second.Any())
            {
                var beforeSelfRef = r.Second.TakeWhile(i => i != r.Index).Select(i => existing[i]).ToList();
                var afterSelfRef = r.Second.SkipWhile(i => i != r.Index).Skip(1).Select(i => existing[i]).ToList();

                // No self refs
                if (beforeSelfRef.Count == r.Second.Count)
                {
                    return OrNFAs(first, beforeSelfRef.Aggregate(ConcatNFAs));
                }

                NFA merged = first;
                for (int i = 0; i < 5; ++i)
                {
                    merged = OrNFAs(merged, beforeSelfRef.Append(merged).Concat(afterSelfRef).Aggregate(ConcatNFAs));
                }

                return merged;
            }

            return first;
        }

        [Solver(2)]
        public static long Solve2(ChallengeType input)
        {
            var nfas = input.Rules.Where(r => r.Value.Any())
                .ToDictionary(r => r.Index, r => MakeStartingDFA(r));

            var toExpand = input.Rules.Where(r => r.Value.Count == 0).ToList();

            var rule8 = toExpand.Find(r => r.Index == 8);
            rule8.Second.Add(42);
            rule8.Second.Add(8);

            var rule11 = toExpand.Find(r => r.Index == 11);
            rule11.Second.Add(42);
            rule11.Second.Add(11);
            rule11.Second.Add(31);

            while (toExpand.Any())
            {
                var toExpandNext = new List<Rule>();

                foreach (var r in toExpand)
                {
                    if (!r.DependsOn.All(d => nfas.ContainsKey(d)))
                    {
                        toExpandNext.Add(r);
                        continue;
                    }

                    nfas[r.Index] = RuleToNFA(r, nfas);
                }

                toExpand = toExpandNext;
            }

            var nfa = nfas[0];

            int matching = 0;

            foreach (var line in input.Lines)
            {
                if (line.Length >= nfa.Transitions.Count)
                {
                    continue;
                }

                var currentNodes = nfa.StartNodes;
                foreach (var c in line)
                {
                    currentNodes = currentNodes.SelectMany(n => nfa.Transitions[n][c - 'a']).ToHashSet();
                }

                if (currentNodes.Any(n => nfa.Accepting[n]))
                {
                    ++matching;
                }
            }

            return matching;
        }
    }
}
