using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2020, 13)]
    public static class Day13_2020
    {
        [MapInput]
        public static (int, IEnumerable<int>) Map(string[] lines)
        {
            var timeToWait = lines.First().AsInt();
            var busTimes = lines.Skip(1).SelectMany(l => l.Split(',')).Where(c => c != "x").Select(int.Parse);

            return (timeToWait, busTimes);
        }

        [Solver(1)]
        public static long Solve1((int, IEnumerable<int>) input)
        {
            var smallestWait = input.Item2
                .Select(m => new { mul = (int)Math.Ceiling(input.Item1 / (double)m), id = m })
                .OrderBy(b => b.mul * b.id)
                .First();

            return (smallestWait.id * smallestWait.mul - input.Item1) * smallestWait.id;
        }

        [Solver(2)]
        public static long Solve2((int, IEnumerable<int>) input)
        {
            return 1;
        }
    }
}
