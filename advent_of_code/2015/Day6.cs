using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Command;

    public record Command(string Action, Pair Start, Pair End);

    [AdventOfCode(2015, 6)]
    public static class Day6_2015
    {
        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines)
        {
            return lines
                .Select(l => Regex.Match(l, @"^(.*) (\d+),(\d+) through (\d+),(\d+)$"))
                .Select(m => new Command(m.Get(1), m.GetPair(2), m.GetPair(4)));
        }

        public static T[,] RunGrid<T>(
            IEnumerable<ChallengeType> input,
            Func<T, T> onFunc,
            Func<T, T> offFunc,
            Func<T, T> toggleFunc
        )
        {
            var lightGrid = new T[1000, 1000];

            void RunSection(Pair start, Pair end, Func<T, T> action)
            {
                for (int r = start.Y; r <= end.Y; ++r)
                {
                    for (int c = start.X; c <= end.X; ++c)
                    {
                        lightGrid[r, c] = action(lightGrid[r, c]);
                    }
                }
            }

            foreach (var command in input)
            {
                RunSection(
                    command.Start,
                    command.End,
                    command.Action switch
                    {
                        "turn on" => onFunc,
                        "turn off" => offFunc,
                        _ => toggleFunc
                    }
                );
            }

            return lightGrid;
        }

        public static int Count<T>(T[,] grid, Func<T, int> toNum)
        {
            int count = 0;

            foreach (var light in grid)
            {
                count += toNum(light);
            }

            return count;
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var lightGrid = RunGrid<bool>(
                input,
                (curr) => true,
                (curr) => false,
                (curr) => !curr
            );

            return Count(lightGrid, (light) => light ? 1 : 0);
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var lightGrid = RunGrid<int>(
                input,
                (curr) => curr + 1,
                (curr) => curr > 0 ? curr - 1 : 0,
                (curr) => curr + 2
            );

            return Count(lightGrid, (light) => light);
        }
    }
}
