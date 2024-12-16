using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;

namespace AOC
{
    public record Input202416(string[] lines);

    [AdventOfCode(2024, 16)]
    public static class Day16_2024
    {
        [MapInput]
        public static Input202416 Map(string[] lines)
        {
            return new Input202416(lines);
        }

        [Solver(1)]
        public static int Solve1(Input202416 input)
        {
            return 1;
        }

        [Solver(2)]
        public static long Solve2(Input202416 input)
        {
            return 1;
        }
    }
}