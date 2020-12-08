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
        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines)
        {
            var cities = new Dictionary<string, City>();

            var matches = lines.Select(l => Regex.Match(l, @"(.*?) to (.*?) = (\d+)"));
            foreach (Match match in matches)
            {
                var cityName = match.Get(1);
                var targetCity = match.Get(2);
                var distance = match.Get(3).AsInt();

                var city = cities.GetValueOrDefault(cityName) ?? new City(cityName);
                var target = cities.GetValueOrDefault(targetCity) ?? new City(targetCity);

                city.Connections.Add((distance, target));
                target.Connections.Add((distance, city));

                cities[cityName] = city;
                cities[targetCity] = target;
            }

            return new[] { cities };
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
                .Skip(1)
                .Zip(path)
                .Sum(ps => input[ps.First].Connections.First(c => c.Item2.Name == ps.Second).Item1);

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var graph = input.First();
            return graph
                .SelectMany(kvp => GetAllPaths(kvp.Value))
                .Select(p => SumPath(p, graph))
                .Min();
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var graph = input.First();
            return graph
                .SelectMany(kvp => GetAllPaths(kvp.Value))
                .Select(p => SumPath(p, graph))
                .Max();
        }
    }
}
