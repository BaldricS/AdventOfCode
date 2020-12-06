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

    public record Pair(int X, int Y);

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

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var lightGrid = new bool[1000,1000];

            foreach (var command in input)
            {
                switch (command.Action)
                {
                    case Action.On:
                        for (int r = command.Start.Y; r <= command.End.Y; ++r)
                        {
                            for (int c = command.Start.X; c <= command.End.X; ++c)
                            {
                                lightGrid[r,c] = true;
                            }
                        }

                        break;
                    case Action.Off:
                        for (int r = command.Start.Y; r <= command.End.Y; ++r)
                        {
                            for (int c = command.Start.X; c <= command.End.X; ++c)
                            {
                                lightGrid[r,c] = false;
                            }
                        }

                        break;
                    case Action.Toggle:
                        for (int r = command.Start.Y; r <= command.End.Y; ++r)
                        {
                            for (int c = command.Start.X; c <= command.End.X; ++c)
                            {
                                lightGrid[r,c] = !lightGrid[r,c];
                            }
                        }

                        break;
                }
            }

            int count = 0;

            foreach (var light in lightGrid)
            {
                if (light)
                {
                    ++count;
                }
            }

            return count;
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var lightGrid = new int[1000,1000];

            foreach (var command in input)
            {
                switch (command.Action)
                {
                    case Action.On:
                        for (int r = command.Start.Y; r <= command.End.Y; ++r)
                        {
                            for (int c = command.Start.X; c <= command.End.X; ++c)
                            {
                                ++lightGrid[r, c];
                            }
                        }

                        break;
                    case Action.Off:
                        for (int r = command.Start.Y; r <= command.End.Y; ++r)
                        {
                            for (int c = command.Start.X; c <= command.End.X; ++c)
                            {
                                if (lightGrid[r,c] > 0)
                                {
                                    --lightGrid[r, c];
                                }
                            }
                        }

                        break;
                    case Action.Toggle:
                        for (int r = command.Start.Y; r <= command.End.Y; ++r)
                        {
                            for (int c = command.Start.X; c <= command.End.X; ++c)
                            {
                                lightGrid[r,c] += 2;
                            }
                        }

                        break;
                }
            }

            int count = 0;

            foreach (var light in lightGrid)
            {
                if (light > 0)
                {
                    count += light;
                }
            }

            return count;
        }
    }
}
