using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    public record Input202415(string[] Lines);

    [AdventOfCode(2024, 15)]
    public static class Day15_2024
    {
        [MapInput]
        public static Input202415 Map(string[] lines)
        {
            return new Input202415(lines);
        }

        [Solver(1)]
        public static int Solve1(Input202415 input)
        {
            return 1;
        }

        [Solver(2)]
        public static int Solve2(Input202415 input)
        {
            return 1;
        }
    }
}