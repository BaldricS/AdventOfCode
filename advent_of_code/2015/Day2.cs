using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public record Prism(int Length, int Width, int Height)
    {
        private int Face1 { get => Length * Width; }
        private int Face2 { get => Length * Height; }
        private int Face3 { get => Width * Height; }

        private int Perm1 { get => 2 * (Length + Width); }
        private int Perm2 { get => 2 * (Length + Height); }
        private int Perm3 { get => 2 * (Width + Height); }

        public int SmallestPerimeter { get => Math.Min(Perm1, Math.Min(Perm2, Perm3)); }
        public int SmallestSurfaceArea { get => Math.Min(Face1, Math.Min(Face2, Face3)); }

        public int Volume { get => Length * Width * Height; }
        public int SurfaceArea { get => 2 * (Face1 + Face2 + Face3); }
    };

    [AdventOfCode(2015, 2)]
    public static class Day2_2015
    {
        [MapInput]
        public static IEnumerable<Prism> Map(string[] lines) =>
            lines
                .Select(l => l.Split('x').Select(int.Parse).ToArray())
                .Select(dims => new Prism(dims[0], dims[1], dims[2]));

        [Solver(1)]
        public static long TotalArea(IEnumerable<Prism> prisms) => prisms.Sum(p => p.SurfaceArea + p.SmallestSurfaceArea);

        [Solver(2)]
        public static long TotalRibbonLength(IEnumerable<Prism> prisms) => prisms.Sum(p => p.Volume + p.SmallestPerimeter);

    }
}
