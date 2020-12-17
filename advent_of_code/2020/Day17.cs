using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = HashSet<Cube>;
    public record Cube(int X, int Y, int Z);
    public record Quad(int X, int Y, int Z, int W);

    [AdventOfCode(2020, 17)]
    public static class Day17_2020
    {
        [MapInput]
        public static ChallengeType Map(string[] lines) =>
            lines.SelectMany((l, x) => l.Select((c, y) => c switch {
                '#' => new Cube(x, y, 0),
                _ => null
            }))
            .Where(c => c != null)
            .ToHashSet();

        public static IEnumerable<Cube> Neighbors(Cube c)
        {
            for (int x = -1; x <=1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    for (int z = -1; z <= 1; ++z)
                    {
                        if (x == 0 && y == 0 && z == 0)
                        {
                            continue;
                        }

                        yield return c with { X = c.X + x, Y = c.Y + y, Z = c.Z + z };
                    }
                }
            }
        }

        public static (Cube, Cube) BoundingBox(HashSet<Cube> c)
        {
            int maxX = -10000;
            int maxY = -10000;
            int maxZ = -10000;
            int minX = 10000;
            int minY = 10000;
            int minZ = 10000;

            foreach (var cube in c)
            {
                if (cube.X > maxX)
                {
                    maxX = cube.X;
                }
                if (cube.Y > maxY)
                {
                    maxY = cube.Y;
                }
                if (cube.Z > maxZ)
                {
                    maxZ = cube.Z;
                }

                if (cube.X < minX)
                {
                    minX = cube.X;
                }
                if (cube.Y < minY)
                {
                    minY = cube.Y;
                }
                if (cube.Z < minZ)
                {
                    minZ = cube.Z;
                }
            }

            return (new Cube(minX - 1, minY - 1, minZ - 1), new Cube(maxX + 1, maxY + 1, maxZ + 1));
        }

        public static IEnumerable<Cube> AllCubes(Cube min, Cube max)
        {
            for (int x = min.X; x <= max.X; ++x)
            {
                for (int y = min.Y; y <= max.Y; ++y)
                {
                    for (int z = min.Z; z <= max.Z; ++z)
                    {
                        yield return new Cube(x, y, z);
                    }
                }
            }
        }

        public static HashSet<Cube> PerformCycle(HashSet<Cube> current)
        {
            var newActive = current.Where(c =>
            {
                var count = Neighbors(c).Count(c => current.Contains(c));
                return count == 2 || count == 3;
            });

            var aabb = BoundingBox(current);
            var flippedInactive = AllCubes(aabb.Item1, aabb.Item2)
                .Where(c => !current.Contains(c))
                .Where(c =>
                {
                    var count = Neighbors(c).Count(c => current.Contains(c));

                    return count == 3;
                });

            return newActive.Union(flippedInactive).ToHashSet();
        }

        public static IEnumerable<Quad> Neighbors(Quad c)
        {
            for (int x = -1; x <=1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    for (int z = -1; z <= 1; ++z)
                    {
                        for (int w = -1; w <= 1; ++w)
                        {
                        if (x == 0 && y == 0 && z == 0 && w == 0)
                        {
                            continue;
                        }

                        yield return c with { X = c.X + x, Y = c.Y + y, Z = c.Z + z, W = c.W + w };

                        }
                    }
                }
            }
        }

        public static (Quad, Quad) BoundingBox(HashSet<Quad> c)
        {
            int maxX = -10000;
            int maxY = -10000;
            int maxZ = -10000;
            int maxW = -10000;
            int minX = 10000;
            int minY = 10000;
            int minZ = 10000;
            int minW = 10000;

            foreach (var cube in c)
            {
                if (cube.X > maxX)
                {
                    maxX = cube.X;
                }
                if (cube.Y > maxY)
                {
                    maxY = cube.Y;
                }
                if (cube.Z > maxZ)
                {
                    maxZ = cube.Z;
                }
                if (cube.W > maxW)
                {
                    maxW = cube.W;
                }

                if (cube.X < minX)
                {
                    minX = cube.X;
                }
                if (cube.Y < minY)
                {
                    minY = cube.Y;
                }
                if (cube.Z < minZ)
                {
                    minZ = cube.Z;
                }
                if (cube.W < minW)
                {
                    minW = cube.W;
                }
            }

            return (new Quad(minX - 1, minY - 1, minZ - 1, minW - 1), new Quad(maxX + 1, maxY + 1, maxZ + 1, maxW + 1));
        }

        public static IEnumerable<Quad> AllQuads(Quad min, Quad max)
        {
            for (int x = min.X; x <= max.X; ++x)
            {
                for (int y = min.Y; y <= max.Y; ++y)
                {
                    for (int z = min.Z; z <= max.Z; ++z)
                    {
                        for (int w = min.W; w <= max.W; ++w)
                        {
                            yield return new Quad(x, y, z, w);
                        }
                    }
                }
            }
        }

        public static HashSet<Quad> PerformCycle(HashSet<Quad> current)
        {
            var newActive = current.Where(c =>
            {
                var count = Neighbors(c).Count(c => current.Contains(c));
                return count == 2 || count == 3;
            });

            var aabb = BoundingBox(current);
            var flippedInactive = AllQuads(aabb.Item1, aabb.Item2)
                .Where(c => !current.Contains(c))
                .Where(c =>
                {
                    var count = Neighbors(c).Count(c => current.Contains(c));

                    return count == 3;
                });

            return newActive.Union(flippedInactive).ToHashSet();
        }

        [Solver(1)]
        public static long Solve1(ChallengeType input)
        {
            for (int i = 0; i < 6; ++i)
            {
                input = PerformCycle(input);
            }

            return input.Count;
        }

        [Solver(2)]
        public static long Solve2(ChallengeType input)
        {
            var quads = input.Select(c => new Quad(c.X, c.Y, c.Z, 0)).ToHashSet();

            for (int i = 0; i < 6; ++i)
            {
                quads = PerformCycle(quads);
            }

            return quads.Count;
        }
    }
}
