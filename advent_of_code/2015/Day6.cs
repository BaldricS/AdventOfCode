using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Command;
    public enum Action
    {
        Toggle,
        Off,
        On
    }

    public record Command(Action Action, Pair Start, Pair End);

    [AdventOfCode(2015, 6)]
    public static class Day6_2015
    {

        public static Action GetAction(string action)
        {
            switch (action)
            {
                case "turn off":
                    return Action.Off;
                case "turn on":
                    return Action.On;
                default:
                    return Action.Toggle;
            }
        }

        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines)
        {
            return lines
                .Select(l => Regex.Match(l, @"^(.*) (\d+),(\d+) through (\d+),(\d+)$"))
                .Select(m => new Command(
                    GetAction(m.Groups[1].Value),
                    new Pair(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value)),
                    new Pair(int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value))
                ));
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

            Func<T, T> ChooseAction(Action action)
            {
                switch (action)
                {
                    case Action.On:
                        return onFunc;
                    case Action.Off:
                        return offFunc;
                    default:
                        return toggleFunc;
                }
            }

            foreach (var command in input)
            {
                RunSection(command.Start, command.End, ChooseAction(command.Action));
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
