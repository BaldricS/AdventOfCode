using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC
{
    public record Input202419()
    {
        public Trie202419 AvailableTowels { get; set; }
        public string[] Patterns { get; set; }
    }

    public record Trie202419()
    {
        public bool IsTowel { get; set; }
        public Trie202419[] Next { get; set; }
    }

    [AdventOfCode(2024, 19)]
    public static class Day19_2024
    {
        [MapInput]
        public static Input202419 Map(string[] lines)
        {
            var root = MakeNode();

            foreach (var towel in lines[0].Split(", "))
            {
                AddToTrie(root, towel);
            }

            return new Input202419(){
                AvailableTowels = root,
                Patterns = lines.Skip(2).ToArray()
            };
        }

        public static Trie202419 MakeNode()
        {
            return new Trie202419()
            {
                IsTowel = false,
                Next = Enumerable.Range(0, 26).Select(_ => (Trie202419)null).ToArray()
            };
        }

        public static void AddToTrie(Trie202419 root, string towel)
        {
            foreach (var c in towel)
            {
                var next = root.Next[c - 'a'];
                if (next == null)
                {
                    root.Next[c - 'a'] = next = MakeNode();
                }

                root = next;
            }

            root.IsTowel = true;
        }

        [Solver(1)]
        public static int Solve1(Input202419 input)
        {
            return input.Patterns.Where(p => CanMakePattern(p, input.AvailableTowels, []) > 0).Count();
        }

        public static IEnumerable<int> CanReachTowel(string pattern, Trie202419 root)
        {
            for (int i = 0; i < pattern.Length && root != null; ++i)
            {
                root = root.Next[pattern[i] - 'a'];
                if (root?.IsTowel ?? false)
                {
                    yield return i + 1;
                }
            }
        }

        public static long CanMakePattern(string pattern, Trie202419 node, Dictionary<string, long> seenPatterns)
        {
            if (seenPatterns.TryGetValue(pattern, out var val))
            {
                return val;
            }

            if (pattern.Length == 0)
            {
                return 1;
            }

            long waysToMake = 0;
            foreach (var i in CanReachTowel(pattern, node))
            {
                string newPattern = pattern[i..];
                waysToMake += CanMakePattern(newPattern, node, seenPatterns);
            }

            return seenPatterns[pattern] = waysToMake;
        }

        [Solver(2)]
        public static long Solve2(Input202419 input)
        {
            return input.Patterns.Select(p => CanMakePattern(p, input.AvailableTowels, [])).Sum();
        }
    }
}