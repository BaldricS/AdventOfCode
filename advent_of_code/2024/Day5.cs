using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AOC
{
    public record PageRules202405(Dictionary<int, HashSet<int>> Rules, List<int[]> Pages) { }

    [AdventOfCode(2024, 5)]
    public static class Day5_2024
    {
        [MapInput]
        public static PageRules202405 Map(string[] lines)
        {
            PageRules202405 input = new([], []);

            int i = 0;
            for (; i < lines.Length; ++i)
            {
                if (lines[i] == "")
                {
                    ++i;
                    break;
                }

                int[] pages = lines[i].Split('|').Select(int.Parse).ToArray();
                if (!input.Rules.ContainsKey(pages[0]))
                {
                    input.Rules.Add(pages[0], []);
                }

                input.Rules[pages[0]].Add(pages[1]);
            }

            for (; i < lines.Length; ++i)
            {
                input.Pages.Add(lines[i].Split(',').Select(int.Parse).ToArray());
            }

            return input;
        }

        [Solver(1)]
        public static int Solve1(PageRules202405 input)
        {
            return input.Pages
                .Where(p => IsGoodPage(p, input.Rules))
                .Select(p => p[p.Length / 2])
                .Sum();
        }

        [Solver(2)]
        public static int Solve2(PageRules202405 input)
        {
            return input.Pages
                .Where(p => !IsGoodPage(p, input.Rules))
                .Select(p => {
                    var l = p.ToList();
                    l.Sort((p1, p2) => input.Rules[p1].Contains(p2) ? -1 : 1);
                    return l;
                })
                .Select(p => p[p.Count / 2])
                .Sum();
        }

        private static bool IsGoodPage(int[] page, Dictionary<int, HashSet<int>> rules)
        {
            var current = page.First();
            foreach (var p in page.Skip(1))
            {
                rules.TryGetValue(current, out HashSet<int> hops);
                if (hops == null)
                {
                    return false;
                }

                if (!hops.Contains(p))
                {
                    return false;
                }

                current = p;
            }

            return true;
        }
    }
}