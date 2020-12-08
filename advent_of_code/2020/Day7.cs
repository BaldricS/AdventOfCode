using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Dictionary<string, Bag>;

    public class Bag
    {
        public string Name { get; set; }
        public List<Bag> ContainedBy { get; set; }
        public List<(int, Bag)> Contains { get; set; }
    }

    [AdventOfCode(2020, 7)]
    public static class Day7_2020
    {
        public static void GetBag(string line, ChallengeType bags)
        {
            var pieces = line.Split(" bags contain ");
            var additionalBags = Regex.Matches(pieces[1], @"(\d+) (.*?) bags?");

            if (!bags.ContainsKey(pieces[0]))
            {
                bags.Add(pieces[0], new Bag
                {
                    Name = pieces[0],
                    Contains = new List<(int, Bag)>(),
                    ContainedBy = new List<Bag>()
                });
            }

            var bag = bags[pieces[0]];

            foreach (Match match in additionalBags)
            {
                var numBags = match.Get(1).AsInt();
                var bagName = match.Get(2);

                if (!bags.ContainsKey(bagName))
                {
                    bags.Add(bagName, new Bag
                    {
                        Name = bagName,
                        Contains = new List<(int, Bag)>(),
                        ContainedBy = new List<Bag>()
                    });
                }

                var childBag = bags[bagName];
                childBag.ContainedBy.Add(bag);

                bag.Contains.Add((numBags, childBag));
            }
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
            var bagGraph = input.First();

            var shiningBag = bagGraph["shiny gold"];
            var containedSet = new HashSet<string>();

            var toVisit = new List<Bag>(shiningBag.ContainedBy);
            while (toVisit.Count > 0)
            {
                var next = toVisit.First();
                toVisit.RemoveAt(0);

                if (containedSet.Contains(next.Name))
                {
                    continue;
                }

                containedSet.Add(next.Name);
                toVisit.AddRange(next.ContainedBy);
            }

            return containedSet.Count;
        }

        public static long CountBags(Bag bag) =>
            bag.Contains.Sum(b => b.Item1 * (1 + CountBags(b.Item2)));

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var bagGraph = input.First();

            var shiningBag = bagGraph["shiny gold"];
            Console.WriteLine(shiningBag.Contains.First().Item1);

            return CountBags(shiningBag);
        }
    }
}
