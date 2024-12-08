using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public record Grid20248(int Row, int Col, Dictionary<char, List<(int, int)>> Nodes);

    [AdventOfCode(2024, 8)]
    public static class Day8_2024
    {
        [MapInput]
        public static Grid20248 Map(string[] lines)
        {
            Dictionary<char, List<(int, int)>> locations = [];

            for (int r = 0; r < lines.Length; ++r)
            {
                for (int c = 0; c < lines[r].Length; ++c)
                {
                    if (lines[r][c] != '.')
                    {
                        locations.TryGetValue(lines[r][c], out List<(int, int)> locs);
                        if (locs == null)
                        {
                            locs = [(r, c)];
                            locations.Add(lines[r][c], [(r, c)]);
                        }
                        else
                        {
                            locs.Add((r, c));
                        }
                    }
                }
            }

            return new Grid20248(lines.Length, lines[0].Length, locations);
        }

        [Solver(1)]
        public static long Solve1(Grid20248 input)
        {
            return input.Nodes.SelectMany(kvp => GetAntinodes(kvp.Value)).Where(n => IsOnGrid(n, input.Row, input.Col)).Distinct().Count();
        }

        public static IEnumerable<(int, int)> GetAntinodes(List<(int, int)> nodes)
        {
            for (int first = 0; first < nodes.Count - 1; ++first)
            {
                for (int second = first + 1; second < nodes.Count; ++second)
                {
                    (int, int) a = nodes[first];
                    (int, int) b = nodes[second];

                    int deltaX = b.Item1 - a.Item1;
                    int deltaY = b.Item2 - a.Item2;

                    yield return (a.Item1 - deltaX, a.Item2 - deltaY);
                    yield return (b.Item1 + deltaX, b.Item2 + deltaY);
                }
            }
        }

        public static IEnumerable<(int, int)> GetAntinodes2(List<(int, int)> nodes, int Row, int Col)
        {
            for (int first = 0; first < nodes.Count - 1; ++first)
            {
                for (int second = first + 1; second < nodes.Count; ++second)
                {
                    (int, int) a = nodes[first];
                    (int, int) b = nodes[second];

                    int deltaX = b.Item1 - a.Item1;
                    int deltaY = b.Item2 - a.Item2;

                    do {
                        yield return a;
                        a.Item1 -= deltaX;
                        a.Item2 -= deltaY;
                    } while (IsOnGrid(a, Row, Col));

                    do {
                        yield return b;
                        b.Item1 += deltaX;
                        b.Item2 += deltaY;
                    } while (IsOnGrid(b, Row, Col));
                }
            }
        }

        public static bool IsOnGrid((int, int) n, int r, int c)
        {
            return n.Item1 >= 0 && n.Item1 < c && n.Item2 >= 0 && n.Item2 < r;
        }

        [Solver(2)]
        public static long Solve2(Grid20248 input)
        {
            return input.Nodes.SelectMany(kvp => GetAntinodes2(kvp.Value, input.Row, input.Col)).Distinct().Count();
        }
    }
}