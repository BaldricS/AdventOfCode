using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public record Input232423()
    {
        public Dictionary<string, HashSet<string>> Connections { get; set; }
        public List<List<string>> OrigConnections { get; set; }
    }

    [AdventOfCode(2024, 23)]
    public static class Day23_2024
    {
        [MapInput]
        public static Input232423 Map(string[] lines)
        {
            return new Input232423(){
                Connections = lines.Select(l => l.Split("-")).Aggregate(new Dictionary<string, HashSet<string>>(), (dic, n) => {
                    if (!dic.ContainsKey(n[0])) {
                        dic[n[0]] = [n[1]];
                    } else {
                        dic[n[0]].Add(n[1]);
                    }

                    if (!dic.ContainsKey(n[1])) {
                        dic[n[1]] = [n[0]];
                    } else {
                        dic[n[1]].Add(n[0]);
                    }

                    return dic;
                })
            };
        }

        [Solver(1)]
        public static long Solve1(Input232423 input)
        {
            return FindConnections(input.Connections).Count();
        }

        public static HashSet<(string, string, string)> FindConnections(Dictionary<string, HashSet<string>> g)
        {
            var set = new HashSet<(string, string, string)>();

            foreach (var kvp in g.Where(k => k.Key.StartsWith('t')))
            {
                var first = kvp.Key;

                foreach (var second in kvp.Value)
                {
                    foreach (var third in g[second])
                    {
                        if (g[third].Contains(first))
                        {
                            var items = new string[] { first, second, third };
                            var sorted = items.ToSortedList();
                            set.Add((sorted[0], sorted[1], sorted[2]));
                        }
                    }
                }
            }

            return set;
        }

        [Solver(2)]
        public static string Solve2(Input232423 input)
        {
            var allNetworks = input.Connections.Keys.Select(k => new HashSet<string>(){ k }).ToList();

            foreach (var kvp in input.Connections)
            {
                foreach (var triangle in allNetworks)
                {
                    if (triangle.IsProperSubsetOf(kvp.Value))
                    {
                        triangle.Add(kvp.Key);
                    }
                }
            }

            return string.Join(",", allNetworks.MaxBy(t => t.Count).ToSortedList());
        }
    }
}