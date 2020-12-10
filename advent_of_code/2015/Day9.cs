using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Dictionary<string, City>;
    public class City
    {
        public City(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public List<(int, City)> Connections { get; set; } = new List<(int, City)>();
    }

    [AdventOfCode(2015, 9)]
    public static class Day9_2015
    {
        public static (string Name, string Target, int Distance) ToData(Match m) =>
            (m.Get(1), m.Get(2), m.Get(3).AsInt());

        [MapInput]
        public static ChallengeType Map(string[] lines)
        {
            var cities = new Dictionary<string, City>();
            var matches = lines.Select(l => Regex.Match(l, @"(.*?) to (.*?) = (\d+)")).Select(ToData);
            foreach (var data in matches)
            {
                var city = cities.GetValueOrDefault(data.Name) ?? new City(data.Name);
                var target = cities.GetValueOrDefault(data.Target) ?? new City(data.Target);

                city.Connections.Add((data.Distance, target));
                target.Connections.Add((data.Distance, city));

                cities[data.Name] = city;
                cities[data.Target] = target;
            }

            return cities;
        }

        public static List<string[]> GetAllPaths(City start)
        {
            var paths = new List<string[]>();

            void GetPaths(City city, HashSet<string> visited, IEnumerable<string> path)
            {
                var toVisit = city.Connections.Where(c => !visited.Contains(c.Item2.Name));
                if (!toVisit.Any())
                {
                    paths.Add(path.ToArray());
                    return;
                }

                foreach (var conn in toVisit)
                {
                    visited.Add(conn.Item2.Name);

                    GetPaths(conn.Item2, visited, path.Append(conn.Item2.Name));

                    visited.Remove(conn.Item2.Name);
                }
            }

            GetPaths(
                start,
                new HashSet<string> { start.Name },
                new [] { start.Name }
            );

            return paths;
        }

        public static long SumPath(string[] path, ChallengeType input) =>
            path
                .Pair()
                .Sum(ps => input[ps.First].Connections.First(c => c.Item2.Name == ps.Second).Item1);

        [Solver(1)]
        public static long Solve1(ChallengeType input) =>
            input
                .SelectMany(kvp => GetAllPaths(kvp.Value))
                .Select(p => SumPath(p, input))
                .Min();

        [Solver(2)]
        public static long Solve2(ChallengeType input) =>
            input
                .SelectMany(kvp => GetAllPaths(kvp.Value))
                .Select(p => SumPath(p, input))
                .Max();
    }
}
