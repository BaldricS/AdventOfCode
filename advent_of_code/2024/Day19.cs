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
            return input.Patterns.Where(p => CanMakePattern(p, 0, input.AvailableTowels, Enumerable.Repeat(-1L, 70).ToArray()) > 0).Count();
        }

        public static long CanMakePattern(string pattern, int idx, Trie202419 root, long[] seenPatterns)
        {
            if (seenPatterns[idx] >= 0)
            {
                return seenPatterns[idx];
            }

            if (idx == pattern.Length)
            {
                return 1;
            }

            long waysToMake = 0;
            var node = root;
            for (int i = idx; i < pattern.Length && node != null; ++i)
            {
                node = node.Next[pattern[i] - 'a'];
                if (node?.IsTowel ?? false)
                {
                    waysToMake += CanMakePattern(pattern, i + 1, root, seenPatterns);
                }
            }

            seenPatterns[idx] = waysToMake;
            return waysToMake;
        }

        [Solver(2)]
        public static long Solve2(Input202419 input)
        {
            return input.Patterns.Select(p => CanMakePattern(p, 0, input.AvailableTowels, Enumerable.Repeat(-1L, 70).ToArray())).Sum();
        }
    }
}