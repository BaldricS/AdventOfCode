using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Dictionary<string, Bag>;

    public class Bag
    {
        public Bag (string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public List<Bag> ContainedBy { get; set; } = new List<Bag>();
        public List<(int, Bag)> Contains { get; set; } = new List<(int, Bag)>();
    }

    [AdventOfCode(2020, 7)]
    public static class Day7_2020
    {
        public static void GetBag(string line, ChallengeType bags)
        {
            var pieces = line.Split(" bags contain ");
            var bag = bags.GetValueOrDefault(pieces[0]) ?? new Bag(pieces[0]);

            var additionalBags = Regex.Matches(pieces[1], @"(\d+) (.*?) bags?");
            foreach (Match match in additionalBags)
            {
                var numBags = match.Get(1).AsInt();
                var bagName = match.Get(2);
                var childBag = bags.GetValueOrDefault(bagName) ?? new Bag(bagName);

                childBag.ContainedBy.Add(bag);
                bag.Contains.Add((numBags, childBag));

                bags[bagName] = childBag;
            }

            bags[pieces[0]] = bag;
        }

        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines)
        {
            var bags = new ChallengeType();

            foreach (var line in lines)
            {
                GetBag(line, bags);
            }

            return new[] { bags };
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var visited = new HashSet<string>();

            void MarkChildren(Bag bag)
            {
                foreach (var b in bag.ContainedBy)
                {
                    if (visited.Contains(b.Name))
                    {
                        continue;
                    }

                    MarkChildren(b);
                    visited.Add(b.Name);
                }
            }

            MarkChildren(input.First()["shiny gold"]);
            return visited.Count;
        }

        public static long CountBags(Bag bag) =>
            bag.Contains.Sum(b => b.Item1 * (1 + CountBags(b.Item2)));

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input) =>
            CountBags(input.First()["shiny gold"]);
        
    }
}
