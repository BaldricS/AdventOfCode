using System;
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

        public static List<List<string>> GetAllPaths(City start)
        {
            var paths = new List<List<string>>();

            void GetPaths(City city, HashSet<string> visited, List<string> path)
            {
                var toVisit = city.Connections.Where(c => !visited.Contains(c.Item2.Name));
                if (!toVisit.Any())
                {
                    paths.Add(new List<string>(path));
                    return;
                }

                foreach (var conn in city.Connections)
                {
                    if (visited.Contains(conn.Item2.Name))
                    {
                        continue;
                    }

                    visited.Add(conn.Item2.Name);
                    path.Add(conn.Item2.Name);

                    GetPaths(conn.Item2, visited, path);

                    path.RemoveAt(path.Count - 1);
                    visited.Remove(conn.Item2.Name);
                }
            }

            GetPaths(
                start,
                new HashSet<string> { start.Name },
                new List<string> { start.Name }
            );

            return paths;
        }

        public static long SumPath(List<string> path, ChallengeType input)
        {
            long distance = 0;

            for (int i = 1; i < path.Count; ++i)
            {
                var prev = path[i - 1];
                var current = path[i];

                var connections = input[prev].Connections;
                distance += input[prev].Connections.First(c => c.Item2.Name == current).Item1;
            }

            return distance;
        }

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
